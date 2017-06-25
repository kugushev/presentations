using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SpeachFollower.Fix
{
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(RefactorFixClassMethod))]
    public class RefactorFixClassMethod : CodeRefactoringProvider
    {
        public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            var helper = await Helper.TryCreate(context, "FixClass");
            if (helper == null)
                return;

            helper.Add("06. Find ServiceLocator usages", e =>
            {
                e.InsertBefore(helper.Method.Body.Statements.LastOrDefault(),
                    ParseStatement(@"
            var executions = cls.DescendantNodes().OfType<InvocationExpressionSyntax>()
                .Where(IsServiceLocator);
"));
                e.InsertAfter(helper.Method, MethodDeclaration(IdentifierName("bool"), "IsServiceLocator")
                    .WithParameterList(ParameterList(SeparatedList(new[] { Parameter(Identifier("node")).WithType(IdentifierName("InvocationExpressionSyntax")) })))
                    .WithBody(Block(new[] { ParseStatement("return false;") })));
            });

            helper.Add("08. Get all injections", e =>
            {
                e.InsertBefore(helper.Method.Body.Statements.Last(), ParseStatement(@"
            foreach (SimpleNameSyntax type in executions.Select(GetServiceType).Distinct())
            {
                
            }"));
                e.InsertAfter(helper.Method, MethodDeclaration(IdentifierName("SimpleNameSyntax"), "GetServiceType")
                    .WithParameterList(ParameterList(SeparatedList(new[] { Parameter(Identifier("node")).WithType(IdentifierName("InvocationExpressionSyntax")) })))
                    .WithBody(Block(new[] { ParseStatement(@"
            if (node.Expression is MemberAccessExpressionSyntax member &&
                member.Expression is IdentifierNameSyntax className &&
                member.Name is GenericNameSyntax genericMethod)
            {
                return (SimpleNameSyntax)genericMethod.TypeArgumentList.Arguments.Single();
            }
            else
                throw new Exception(""Something wrong"");
") })));
            });

            helper.Add("09. Get all injection with comparer", e =>
            {
                if(helper.Method.DescendantNodes().OfType<ForEachStatementSyntax>().Any())
                    e.ReplaceNode(helper.Method.DescendantNodes().OfType<ForEachStatementSyntax>().Single(), ParseStatement(@"
            foreach (SimpleNameSyntax type in executions.Select(GetServiceType).Distinct(new SyntaxNodeEquivalenceComparer()))
            {
                
            }"));
            });

            helper.Add("16. Replace all ServiceLocator executions", e =>
            {
                e.InsertAfter(helper.Block.Statements.First(), 
                    ParseStatement("            cls = cls.ReplaceNodes(executions, (orig, same) => SyntaxFactory.IdentifierName(\"_\" + GetFieldName(GetServiceType(same))));"));
            });
        }
    }
}
