using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SpeachFollower.Fix
{
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(ParseText)), Shared]
    class ParseText : CodeRefactoringProvider
    {
        public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            SyntaxNode root = await context.Document.GetSyntaxRootAsync();
            var node = root.FindNode(context.Span);

            var method = node.FirstAncestorOrSelf<MethodDeclarationSyntax>(s => s.Identifier.ToString() == "Fix");

            //var method = root.DescendantNodes().OfType<MethodDeclarationSyntax>().FirstOrDefault(s => s.Identifier.ToString() == "Fix");
            if (method == null)
                return;

            void Register(string name, Func<SyntaxNode> createNoteToEnqueue)
            {
                var action = CodeAction.Create(name, t =>
                {
                    var returnStatement = method.Body.Statements.OfType<ReturnStatementSyntax>().LastOrDefault();
                    var newNode = createNoteToEnqueue();

                    var updated = method.InsertNodesBefore(returnStatement, new[] { newNode.WithLeadingTrivia(returnStatement.GetLeadingTrivia()) });

                    var newRoot = root.ReplaceNode(method, updated);
                    var newdoc = context.Document.WithSyntaxRoot(newRoot);
                    return Task.FromResult(newdoc);
                });
                context.RegisterRefactoring(action);
            }

            Register("1. Create source tree",
                () => LocalDeclarationStatement(
                        VariableDeclaration(
                            ParseTypeName("SyntaxTree"),
                            SeparatedList(new[]
                            {
                                VariableDeclarator("tree").WithInitializer(
                                    EqualsValueClause(
                                        InvocationExpression(
                                            MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("CSharpSyntaxTree"), IdentifierName("ParseText")),
                                            ArgumentList(SeparatedList(new [] { Argument(IdentifierName("source")) }))
                                            )))
                            }))).NormalizeWhitespace());

            Register("2. Get root",
                () => LocalDeclarationStatement(
                    VariableDeclaration(
                        ParseTypeName("SyntaxNode"),
                        SeparatedList(new[]
                        {
                            VariableDeclarator("root").WithInitializer(
                                EqualsValueClause(
                                    InvocationExpression(
                                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("tree"), IdentifierName("GetRoot"))
                                        )))
                        }))).NormalizeWhitespace());

            Register("3. Find class",
                () => LocalDeclarationStatement(
                    VariableDeclaration(
                        ParseTypeName("ClassDeclarationSyntax"),
                        SeparatedList(new[]
                        {
                            VariableDeclarator("cls").WithInitializer(
                                EqualsValueClause(
                                    InvocationExpression(
                                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                            InvocationExpression(
                                                MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                                    InvocationExpression(
                                                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("root"), IdentifierName("DescendantNodes"))),
                                                    GenericName("OfType").WithTypeArgumentList(TypeArgumentList(SeparatedList(new[]{ ParseTypeName("ClassDeclarationSyntax") })))
                                            )),
                                            IdentifierName("FirstOrDefault")
                                            ))))
                        }))).NormalizeWhitespace());

            {
                var fixClassAction = CodeAction.Create("4. Create FixClass", async t =>
                {
                    var returnStatement = method.Body.Statements.OfType<ReturnStatementSyntax>().LastOrDefault();
                    var fixClassExecution = LocalDeclarationStatement(
                        VariableDeclaration(
                            ParseTypeName("SyntaxNode"),
                            SeparatedList(new[]
                            {
                                VariableDeclarator("fixedClass").WithInitializer(
                                    EqualsValueClause(
                                        InvocationExpression(
                                            IdentifierName("FixClass"),
                                            ArgumentList(SeparatedList(new [] { Argument(IdentifierName("cls")) }))
                                            )))
                            }))).NormalizeWhitespace();

                    var classDeclaration = method.Parent;
                    var fixClassDecalaration = MethodDeclaration(ParseTypeName("SyntaxNode"), "FixClass")
                            .AddParameterListParameters(Parameter(Identifier("cls")).WithType(ParseTypeName("ClassDeclarationSyntax")))
                            .AddBodyStatements(new[]
                            {
                                ReturnStatement(IdentifierName("cls"))
                            });

                    var editor = await DocumentEditor.CreateAsync(context.Document);
                    editor.InsertBefore(returnStatement, fixClassExecution.WithLeadingTrivia(returnStatement.GetLeadingTrivia()));
                    editor.InsertAfter(method, fixClassDecalaration);

                    return editor.GetChangedDocument();
                });
                context.RegisterRefactoring(fixClassAction);
            }

            Register("5. Create fixed class",
                () => LocalDeclarationStatement(
                        VariableDeclaration(
                            ParseTypeName("SyntaxNode"),
                            SeparatedList(new[]
                            {
                                VariableDeclarator("fixedRoot").WithInitializer(
                                    EqualsValueClause(
                                        InvocationExpression(
                                            MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("root"), IdentifierName("ReplaceNode")),
                                            ArgumentList(SeparatedList(new []
                                            {
                                                Argument(IdentifierName("cls")),
                                                Argument(IdentifierName("fixedClass"))
                                            }))
                                            )))
                            }))).NormalizeWhitespace());
            {
                var returnFixedClassAction = CodeAction.Create("n. Return fixed class", async t =>
                {
                    var returnStatement = method.Body.Statements.OfType<ReturnStatementSyntax>().LastOrDefault();
                    var updatedReturnStatement = returnStatement.WithExpression(
                        InvocationExpression(
                            MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("fixedRoot"), IdentifierName("ToFullString"))
                            ));

                    var editor = await DocumentEditor.CreateAsync(context.Document);
                    editor.ReplaceNode(returnStatement, updatedReturnStatement);

                    return editor.GetChangedDocument();
                });
                context.RegisterRefactoring(returnFixedClassAction);
            }

            {
                var returnFixedClassAction = CodeAction.Create("n+1. Return fixed class with normalized whitespaces", async t =>
                {
                    var returnStatement = method.Body.Statements.OfType<ReturnStatementSyntax>().LastOrDefault();
                    var updatedReturnStatement = returnStatement.WithExpression(
                        InvocationExpression(
                            MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("fixedRoot"), IdentifierName("NormalizeWhitespace"))),
                                IdentifierName("ToFullString"))
                            ));

                    var editor = await DocumentEditor.CreateAsync(context.Document);
                    editor.ReplaceNode(returnStatement, updatedReturnStatement);

                    return editor.GetChangedDocument();
                });
                context.RegisterRefactoring(returnFixedClassAction);
            }

            {
                Register("Local Function", () => ParseStatement("string toCamelCase(TypeSyntax s) => char.ToLowerInvariant(s.ToFullString()[0]) + s.ToFullString().Substring(1);"));
            }
        }

    }
}
