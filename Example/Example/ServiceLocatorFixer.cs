using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Example
{
    class ServiceLocatorFixer
    {
        public string Fix(string source)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            SyntaxNode root = tree.GetRoot();
            ClassDeclarationSyntax cls = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            SyntaxNode fixedClass = FixClass(cls);
            SyntaxNode fixedRoot = root.ReplaceNode(cls, fixedClass);
            return fixedRoot.NormalizeWhitespace().ToFullString();
        }

        SyntaxNode FixClass(ClassDeclarationSyntax cls)
        {

            var executions = cls.DescendantNodes().OfType<InvocationExpressionSyntax>()
                .Where(IsServiceLocator);

            cls = cls.ReplaceNodes(executions, (orig, same) => SyntaxFactory.IdentifierName("_" + GetFieldName(GetServiceType(same))));

            foreach (SimpleNameSyntax type in executions.Select(GetServiceType).Distinct(new SyntaxNodeEquivalenceComparer()))
            {
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

        ConstructorDeclarationSyntax GetSuitableCtor(ClassDeclarationSyntax cls)
        {
            return cls.Members.OfType<ConstructorDeclarationSyntax>().OrderBy(c => c.ParameterList.Parameters.Count).FirstOrDefault();
        }

        SimpleNameSyntax GetServiceType(InvocationExpressionSyntax node)
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

        bool IsServiceLocator(InvocationExpressionSyntax node)
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
    }
}
