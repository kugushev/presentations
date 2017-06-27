using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLocatorKiller
{
    class ServiceLocatorFixer
    {
        public string Fix(string source)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);

            SyntaxNode root = tree.GetRoot();

            ClassDeclarationSyntax cls = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();

            if (cls != null)
            {
                SyntaxNode fixedClass = FixClass(cls);
                SyntaxNode fixedRoot = root.ReplaceNode(cls, fixedClass);

                return fixedRoot.NormalizeWhitespace().ToFullString();
            }
            return source;
        }

        private SyntaxNode FixClass(ClassDeclarationSyntax cls)
        {

            var executions = cls.DescendantNodes().OfType<InvocationExpressionSyntax>()
                .Where(IsServiceLocatorUsage);

            // replace all occurencies of that invocation
            /*at the last*/
            cls = cls.ReplaceNodes(executions, (orig, same) => SyntaxFactory.IdentifierName(GetFieldName(GetServiceType(same))));

            foreach (SimpleNameSyntax type in executions.Select(GetServiceType).Distinct(new SyntaxNodeEquivalenceComparer()))
            {
                // if not exists add ctor
                if (GetSuitableCtor(cls) == null)
                {
                    cls = cls.InsertNodesBefore(cls.Members.First(), new[]
                    {
                        SyntaxFactory.ConstructorDeclaration(cls.Identifier)
                        .WithBody(SyntaxFactory.Block())
                        .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                    });
                }

                string typeName = GetFieldName(type);
                string fieldName = "_" + typeName;

                // add field
                cls = cls.InsertNodesAfter(GetSuitableCtor(cls), new[]
                {
                        SyntaxFactory.FieldDeclaration(
                            SyntaxFactory.VariableDeclaration(type).AddVariables(
                                SyntaxFactory.VariableDeclarator(fieldName)))
                });

                var ctor = GetSuitableCtor(cls);
                cls = cls.ReplaceNode(ctor, ctor
                    .AddParameterListParameters(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier(typeName)).WithType(type))
                    .AddBodyStatements(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(fieldName),
                            SyntaxFactory.IdentifierName(typeName)))));
            }

            return cls;
        }

        string GetFieldName(SimpleNameSyntax type)
        {
            return char.ToLowerInvariant(type.Identifier.Text[0]) + type.Identifier.Text.Substring(1);
        }

        private bool IsServiceLocatorUsage(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax member &&
                member.Expression is IdentifierNameSyntax className &&
                member.Name is GenericNameSyntax genericMethod)
            {
                return className.Identifier.Text == "ServiceLocator" &&
                    genericMethod.Identifier.Text == "Resolve";

            }
            return false;
        }

        private SimpleNameSyntax GetServiceType(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax member &&
                member.Expression is IdentifierNameSyntax className &&
                member.Name is GenericNameSyntax genericMethod)
            {
                return (SimpleNameSyntax)genericMethod.TypeArgumentList.Arguments.Single();
            }
            else
                throw new Exception("Something wrong");
        }

        private static ConstructorDeclarationSyntax GetSuitableCtor(ClassDeclarationSyntax cls)
            => cls.Members.OfType<ConstructorDeclarationSyntax>().OrderBy(c => c.ParameterList.Parameters.Count).FirstOrDefault();

        private IEnumerable<ClassDeclarationSyntax> FindClassDeclarations(SyntaxNode parent)
        {
            foreach (var child in parent.ChildNodes())
            {
                if (child is ClassDeclarationSyntax cls)
                    yield return cls;
                else
                    foreach (var subcls in FindClassDeclarations(child))
                        yield return subcls;
            }
        }

        private SyntaxNode FindNode(ClassDeclarationSyntax cls, SyntaxNode node)
        {
            if (node is ClassDeclarationSyntax)
                return cls;
            SyntaxNode parent = FindNode(cls, node.Parent);

            return parent.ChildNodes().Where(n => n.IsEquivalentTo(node, true)).Single();
        }

        public string GetSampleClass()
        {
            var node = SyntaxFactory.ClassDeclaration("MyClass")
                .WithLeadingTrivia(
                SyntaxFactory.TriviaList(
                    SyntaxFactory.SyntaxTrivia(
                        SyntaxKind.SingleLineCommentTrivia, "//This is my class")))
                .WithTrailingTrivia(
                SyntaxFactory.TriviaList(
                    SyntaxFactory.SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, "//My class is amazing")));
            return node.NormalizeWhitespace().ToFullString();
        }
    }
}
