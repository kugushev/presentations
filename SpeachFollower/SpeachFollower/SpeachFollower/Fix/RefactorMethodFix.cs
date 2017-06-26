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
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(RefactorMethodFix)), Shared]
    public class RefactorMethodFix : CodeRefactoringProvider
    {
        public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            var helper = await Helper.TryCreate(context, "Fix");
            if (helper == null)
                return;
            
            helper.Add("01. Create source tree", "SyntaxTree tree = CSharpSyntaxTree.ParseText(source);");
            helper.Add("02. Get root", "SyntaxNode root = tree.GetRoot();");
            helper.Add("03. Find class", "ClassDeclarationSyntax cls = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();");
            helper.Add("04. Create FixClass", e =>
            {
                e.InsertBefore(helper.Method.Body.Statements.Last(), ParseStatement("SyntaxNode fixedClass = FixClass(cls);").WithLeadingTrivia(ElasticEndOfLine(Environment.NewLine)));
                e.InsertAfter(helper.Method, MethodDeclaration(ParseTypeName("SyntaxNode"), "FixClass")
                    .AddParameterListParameters(Parameter(Identifier("cls")).WithType(ParseTypeName("ClassDeclarationSyntax")))
                    .AddBodyStatements(new[] { ReturnStatement(IdentifierName("cls")) })
                    .WithLeadingTrivia(ElasticEndOfLine(Environment.NewLine)));
            });
            helper.Add("05. Create fixed class", "SyntaxNode fixedRoot = root.ReplaceNode(cls, fixedClass);");
            helper.Add("N. Return fixed class", e =>
            {
                e.ReplaceNode(helper.Method.Body.Statements.Last(),
                    ParseStatement("return fixedRoot.ToFullString();").WithLeadingTrivia(ElasticEndOfLine(Environment.NewLine)));
            });

            helper.Add("N+1. Return fixed class with normalized whitespaces", e =>
            {
                e.ReplaceNode(helper.Method.Body.Statements.Last(),
                    ParseStatement("return fixedRoot.NormalizeWhitespace().ToFullString();").WithLeadingTrivia(ElasticEndOfLine(Environment.NewLine)));
            });
        }

    }
}
