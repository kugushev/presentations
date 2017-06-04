using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var cls = SyntaxFactory.ClassDeclaration("MyDto");

            cls = cls.AddModifiers(
                SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                SyntaxFactory.Token(SyntaxKind.VirtualKeyword));

            AAA();

            Console.WriteLine(cls.NormalizeWhitespace());
            Console.ReadLine();

        }

        static void AAA()
        {
            var tree = CSharpSyntaxTree.ParseText(
                File.ReadAllText("TestClass.cs"));
            SyntaxNode node = tree.GetRoot().ChildNodes().Single();

            if (node is ClassDeclarationSyntax cls)
            {
                Console.WriteLine(cls.Identifier);
                foreach (var kinder in cls.ChildNodes())
                {
                    if (kinder is MethodDeclarationSyntax meth)
                    {
                        var cmt = kinder.GetLeadingTrivia()
                            .Select(t => t.GetStructure())
                            .OfType<DocumentationCommentTriviaSyntax>()
                            .FirstOrDefault();
                        Console.WriteLine(cmt);
                        Console.WriteLine(meth.Identifier);

                        var aaa = kinder.GetTrailingTrivia().Select(tt => tt.GetStructure());
                        foreach (var item in aaa)
                        {
                            Console.WriteLine(item);
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
