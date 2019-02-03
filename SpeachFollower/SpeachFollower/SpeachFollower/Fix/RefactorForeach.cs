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
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(RefactorForeach))]
    public class RefactorForeach : CodeRefactoringProvider
    {
        public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            SyntaxNode root = await context.Document.GetSyntaxRootAsync();
            var node = root.FindNode(context.Span);
            var frch = node.FirstAncestorOrSelf<ForEachStatementSyntax>();
            if (frch == null)
                return;
            if (!(frch.Statement is BlockSyntax block))
                return;
            var method = node.FirstAncestorOrSelf<MethodDeclarationSyntax>();

            var helper = new Helper { Context = context, Root = root, Block = block };

            helper.Add("10. Add method GetSuitableCtor", e =>
            {
                e.InsertAfter(method, MethodDeclaration(IdentifierName("ConstructorDeclarationSyntax"), "GetSuitableCtor")
    .WithParameterList(ParameterList(SeparatedList(new[] { Parameter(Identifier("cls")).WithType(IdentifierName("ClassDeclarationSyntax")) })))
    .WithBody(Block(new[] { ParseStatement(@"return cls.Members.OfType<ConstructorDeclarationSyntax>().OrderBy(c => c.ParameterList.Parameters.Count).FirstOrDefault();") })));
            });

            helper.AddToEnd("11. Check suitable ctor", @"
                if (GetSuitableCtor(cls) == null)
                {
                    cls = cls.InsertNodesBefore(cls.Members.First(), new[]
                    {
                        SyntaxFactory.ConstructorDeclaration(cls.Identifier)
                            .WithBody(SyntaxFactory.Block())
                            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                    });
                }");

            helper.Add("12. Add method GetTypeName", e =>
            {
                e.InsertAfter(method, MethodDeclaration(IdentifierName("string"), "GetFieldName")
                    .WithParameterList(ParameterList(SeparatedList(new[] { Parameter(Identifier("type")).WithType(IdentifierName("SimpleNameSyntax")) })))
                    .WithBody(Block(new[] { ParseStatement(@"            return char.ToLowerInvariant(type.Identifier.Text[0]) + type.Identifier.Text.Substring(1);") })));
            });

            helper.AddToEnd("13. Get type name and field name", @"
                string typeName = GetFieldName(type);
                string fieldName = ""_"" + typeName;
");

            helper.AddToEnd("14. Add field", @"
                cls = cls.InsertNodesAfter(GetSuitableCtor(cls), new[]
                {
                        SyntaxFactory.FieldDeclaration(
                            SyntaxFactory.VariableDeclaration(type).AddVariables(
                                SyntaxFactory.VariableDeclarator(fieldName)))
                });
");

            helper.AddToEnd("15. Get suitable ctor variable", "                var ctor = GetSuitableCtor(cls);");

            helper.AddToEnd("16. Add assignments", @"
                cls = cls.ReplaceNode(ctor, ctor
                    .AddParameterListParameters(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier(typeName)).WithType(type))
                    .AddBodyStatements(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(fieldName),
                            SyntaxFactory.IdentifierName(typeName)))));
");

        }
    }
}
