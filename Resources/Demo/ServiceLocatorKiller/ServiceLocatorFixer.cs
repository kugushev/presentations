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
            string toCamelCase(TypeSyntax s) => char.ToLowerInvariant(s.ToFullString()[0]) + s.ToFullString().Substring(1);
            string toFieldName(TypeSyntax s) => "_" + toCamelCase(s);

            var executions = cls.DescendantNodes().OfType<InvocationExpressionSyntax>()
                .Where(IsServiceLocatorUsage); ;

            // replace all occurencies of that invocation
            /*at the last*/
            cls = cls.ReplaceNodes(executions, (orig, same) => SyntaxFactory.IdentifierName(toFieldName(GetServiceType(same))));

            foreach (TypeSyntax type in executions.Select(GetServiceType).Distinct(new SyntaxNodeEquivalenceComparer()))
            {
                // if not exists add ctor
                ConstructorDeclarationSyntax ctor = GetSuitableCtor(cls);
                if (ctor == null)
                {
                    cls = cls.InsertNodesBefore(cls.Members.First(), new[]
                    {
                        SyntaxFactory.ConstructorDeclaration(cls.Identifier)
                            .WithBody(SyntaxFactory.Block())
                            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                    });
                }

                string typeName = toCamelCase(type);
                string fieldName = toFieldName(type);
                // add field
                cls = cls.InsertNodesAfter(GetSuitableCtor(cls), new[]
                {
                        SyntaxFactory.FieldDeclaration(
                            SyntaxFactory.VariableDeclaration(type).AddVariables(
                                SyntaxFactory.VariableDeclarator(fieldName)))
                });

                ctor = GetSuitableCtor(cls);

                cls = cls.ReplaceNode(ctor, ctor
                // add argument to ctor
                    .AddParameterListParameters(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier(typeName)).WithType(type))
                // add assignments to ctor
                    .AddBodyStatements(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(fieldName),
                            SyntaxFactory.IdentifierName(typeName)))));
            }

            return cls;
        }


        private bool IsServiceLocatorUsage(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax member &&
                member.Expression is IdentifierNameSyntax className &&
                member.Name is GenericNameSyntax genericMethod)
            {
                return className.Identifier.ToString() == "ServiceLocator" &&
                    genericMethod.Identifier.ToString() == "Resolve";

            }
            return false;
        }

        private TypeSyntax GetServiceType(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax member &&
                member.Expression is IdentifierNameSyntax className &&
                member.Name is GenericNameSyntax genericMethod)
            {
                return genericMethod.TypeArgumentList.Arguments.Single();
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
