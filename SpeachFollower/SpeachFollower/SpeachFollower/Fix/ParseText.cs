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
                var action = CodeAction.Create("Create source tree", t =>
                {
                    var returnStatement = method.Body.Statements.OfType<ReturnStatementSyntax>().LastOrDefault();
                    var newNode = createNoteToEnqueue();

                    var updated = method.InsertNodesBefore(returnStatement, new[] { newNode });

                    var newRoot = root.ReplaceNode(method, updated);
                    var newdoc = context.Document.WithSyntaxRoot(newRoot);
                    return Task.FromResult(newdoc);
                });
                context.RegisterRefactoring(action);
            }

            Register("Create source tree",
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

            //#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            //            var action = CodeAction.Create("Create source tree", async t =>
            //                {
            //                    var returnStatement = method.Body.Statements.OfType<ReturnStatementSyntax>().LastOrDefault();
            //                    var tree = LocalDeclarationStatement(
            //                        VariableDeclaration(
            //                            ParseTypeName("SyntaxTree"),
            //                            SeparatedList(new[]
            //                            {
            //                                VariableDeclarator("tree").WithInitializer(
            //                                    EqualsValueClause(
            //                                        InvocationExpression(
            //                                            MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName("CSharpSyntaxTree"), IdentifierName("ParseText")),
            //                                            ArgumentList(SeparatedList(new [] { Argument(IdentifierName("source")) }))
            //                                            )))
            //                            }))).NormalizeWhitespace();

            //                    var updated = method.InsertNodesBefore(returnStatement, new[] { tree });

            //                    var newRoot = root.ReplaceNode(method, updated);                    
            //                    return context.Document.WithSyntaxRoot(newRoot);
            //                });
            //#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            //            context.RegisterRefactoring(action);
        }

        //private static void Register(string name, Func<SyntaxNode> createNoteToEnqueue)
        //{
        //    var action = CodeAction.Create("Create source tree", t =>
        //    {
        //        var returnStatement = method.Body.Statements.OfType<ReturnStatementSyntax>().LastOrDefault();
        //        var node = createNoteToEnqueue();

        //        var updated = method.InsertNodesBefore(returnStatement, new[] { node });

        //        var newRoot = root.ReplaceNode(method, updated);
        //        return context.Document.WithSyntaxRoot(newRoot);
        //    });
        //}
    }
}
