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


            return null;
        }

        #region Fog of war

        private static string ToCamelCase(SimpleNameSyntax s)
            => char.ToLowerInvariant(s.Identifier.Text[0]) + s.Identifier.Text.Substring(1);

        private static string ToFieldName(SimpleNameSyntax s)
            => "_" + ToCamelCase(s);

        private static ConstructorDeclarationSyntax GetSuitableCtor(ClassDeclarationSyntax cls)
            => cls.Members
            .OfType<ConstructorDeclarationSyntax>()
            .OrderBy(c => c.ParameterList.Parameters.Count)
            .FirstOrDefault();
        
        #endregion
    }
}
