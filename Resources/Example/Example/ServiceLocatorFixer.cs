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
            return cls;
        }

        #region Fog of war

        private static string toCamelCase(TypeSyntax s)
            => char.ToLowerInvariant(s.ToFullString()[0]) + s.ToFullString().Substring(1);

        private static string toFieldName(TypeSyntax s)
            => "_" + toCamelCase(s);

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


        #endregion
    }
}
