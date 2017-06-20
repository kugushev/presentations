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

            /*TODO: 
             * copypaste templates
             * DoWork tasks
             * Fix Distinct
             * add GetSuitableCtor to Fog of War
             * remove IsEquivalent from slides and code
             */

            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);

            SyntaxNode root = tree.GetRoot();

            ClassDeclarationSyntax cls = FindClassDeclarations(root).First();
            SyntaxNode newCls = DoWork(cls);

            var replaced = root.ReplaceNode(cls, newCls);

            return null;
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

        private SyntaxNode DoWork(ClassDeclarationSyntax cls)
        {
            var expressions = cls.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(IsServiceLocatorUsage);

            var distinct = expressions.Select(GetServiceType).Distinct(new SyntaxNodeEquivalenceComparer()).ToList();

            // create ctro if not exitst
            // add ctor injection
            // add field
            // reapply service locator executions
            return null;
        }

        private bool IsServiceLocatorUsage(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax member &&
                member.Expression is IdentifierNameSyntax classname &&
                member.Name is GenericNameSyntax genericmethod)
            {
                return classname.Identifier.ToString() == "ServiceLocator" &&
                    genericmethod.Identifier.ToString() == "Resolve";
            }
            return false;
        }

        #region Fog of war

        private static string toCamelCase(TypeSyntax s)
            => char.ToLowerInvariant(s.ToFullString()[0]) + s.ToFullString().Substring(1);

        private static string toFieldName(TypeSyntax s)
            => "_" + toCamelCase(s);

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
