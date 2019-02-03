using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeRefactorings;

namespace SpeachFollower.Fix
{
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(RefactorIsServiceLocator))]
    public class RefactorIsServiceLocator : CodeRefactoringProvider
    {
        public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            var helper = await Helper.TryCreate(context, "IsServiceLocator");
            if (helper == null)
                return;

            helper.Add("07. Check if expression is MemberAccessExpression", @"
            if (node.Expression is MemberAccessExpressionSyntax member &&
                member.Expression is IdentifierNameSyntax className &&
                member.Name is GenericNameSyntax genericMethod)
            {
                return className.Identifier.Text == ""ServiceLocator"" &&
                    genericMethod.Identifier.Text == ""Resolve"";
            }
            ");
        }
    }
}
