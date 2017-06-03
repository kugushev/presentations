using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
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

            var classes = FindClassDeclarations(root);

            SyntaxNode fixedRoot = root;
            foreach (var cls in classes)
            {
                SyntaxNode fixedClass = FixClass(cls);
                fixedRoot = root.ReplaceNode(cls, fixedClass);
            }

            return fixedRoot.NormalizeWhitespace().ToFullString();
        }

        private SyntaxNode FixClass(ClassDeclarationSyntax cls)
        {
            var executions = cls.DescendantNodes().OfType<InvocationExpressionSyntax>()
                .Where(IsServiceLocatorUsage)
                .Select(TakeExecutionInfo)
                .GroupBy((t) => t.Item1);

            foreach (var byType in executions)
            {
                ConstructorDeclarationSyntax ctor = cls.Members.OfType<ConstructorDeclarationSyntax>().FirstOrDefault();
                if (ctor == null)
                {
                    cls = cls.InsertNodesBefore(cls.Members.First(), new[]
                    {
                        SyntaxFactory.ConstructorDeclaration(cls.Identifier)
                            .WithBody(SyntaxFactory.Block())
                            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                    });
                    ctor = cls.Members.OfType<ConstructorDeclarationSyntax>().First();
                }

                TypeSyntax dependencyType = byType.Key;
                string dependencyTypeName = dependencyType.ToFullString();
                string fieldName = "_" + dependencyTypeName;
                cls = cls.InsertNodesAfter(ctor, new[]
                {
                        SyntaxFactory.FieldDeclaration(
                            SyntaxFactory.VariableDeclaration(dependencyType).AddVariables(
                                SyntaxFactory.VariableDeclarator(fieldName)))
                });
                ctor = cls.Members.OfType<ConstructorDeclarationSyntax>().First();

                cls = cls.ReplaceNode(ctor, ctor.AddParameterListParameters(
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier(dependencyTypeName)).WithType(dependencyType)));
                ctor = cls.Members.OfType<ConstructorDeclarationSyntax>().First();

                cls = cls.ReplaceNode(ctor, ctor.AddBodyStatements(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName(fieldName),
                            SyntaxFactory.IdentifierName(dependencyTypeName)))));

                foreach (var occurrence in byType)
                {
                    InvocationExpressionSyntax node = occurrence.Item2;
                    SyntaxNode current = FindNode(cls, node);
                    cls = cls.ReplaceNode(current, SyntaxFactory.IdentifierName(fieldName));
                }

                // replace all occurencies of that invocation
            }

            return cls;
        }


        private bool IsServiceLocatorUsage(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax member &&
                member.Expression is IdentifierNameSyntax className &&
                member.Name is GenericNameSyntax genericMethod)
            {
                return className.Identifier.ToFullString() == "ServiceLocator" &&
                    genericMethod.Identifier.ToFullString() == "Resolve";

            }
            return false;
        }

        private (TypeSyntax, InvocationExpressionSyntax) TakeExecutionInfo(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax member &&
                member.Expression is IdentifierNameSyntax className &&
                member.Name is GenericNameSyntax genericMethod)
            {
                return (genericMethod.TypeArgumentList.Arguments.Single(), node);
            }
            else
                throw new Exception("Something wrong");
        }

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
    }
}
