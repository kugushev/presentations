using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AnalyzeAndCreate
{
    class Program
    {
        static void Main(string[] args)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText("TestClass.cs"));
            var root = tree.GetRoot();
            foreach (var node in root.ChildNodes())
            {               
                if (node is ClassDeclarationSyntax cls)
                {
                    foreach (var child in cls.ChildNodes())
                    {
                        if (child is MethodDeclarationSyntax meth)
                        {
                            var comment = child.GetLeadingTrivia()
                                .Select(t => t.GetStructure())
                                .OfType<DocumentationCommentTriviaSyntax>()
                                .First();
                            Console.WriteLine(comment.ToFullString());
                            Console.WriteLine(meth.Identifier.ToFullString());
                        }
                    }
                }
            }
            //   Console.ReadLine();

            {
                Console.WriteLine();
                Console.WriteLine();

                ClassDeclarationSyntax cls = SyntaxFactory.ClassDeclaration("MyDto");
                cls = cls.AddMembers(
                    SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.ParseTypeName("string"), SyntaxFactory.Identifier("Name"))
                        .AddAccessorListAccessors(new []
                        {
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                        }));

                var t = CSharpSyntaxTree.Create(cls.NormalizeWhitespace());

                Console.WriteLine(t.ToString());
                Console.ReadLine();
            }
        }
    }
}
