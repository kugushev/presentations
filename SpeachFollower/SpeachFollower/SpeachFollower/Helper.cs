using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace SpeachFollower
{
    class Helper
    {
        public static async Task<Helper> TryCreate(CodeRefactoringContext context, string methodName)
        {
            SyntaxNode root = await context.Document.GetSyntaxRootAsync();
            var node = root.FindNode(context.Span);
            var method = node.FirstAncestorOrSelf<MethodDeclarationSyntax>(m => m.Identifier.Text == methodName);

            return method != null
                ? new Helper { Context = context, Root = root, Method = method, Block = method.Body }
                : null;
        }

        public MethodDeclarationSyntax Method { get; set; }
        public BlockSyntax Block { get; set; }
        public SyntaxNode Root { get; set; }
        public CodeRefactoringContext Context { get; set; }

        public void Add(string name, string statement)
        {
            var action = CodeAction.Create(name, t =>
            {
                var returnStatement = Block.Statements.OfType<ReturnStatementSyntax>().LastOrDefault();
                var newNode = SyntaxFactory.ParseStatement(statement);

                var updated = Method.InsertNodesBefore(returnStatement,
                    new[] { newNode.WithLeadingTrivia(SyntaxFactory.ElasticEndOfLine(Environment.NewLine)) });

                var newRoot = Root.ReplaceNode(Method, updated);
                var newdoc = Context.Document.WithSyntaxRoot(newRoot);
                return Task.FromResult(newdoc);
            });
            Context.RegisterRefactoring(action);
        }

        public void AddToEnd(string name, string statement)
        {
            var action = CodeAction.Create(name, t =>
            {
                var newNode = SyntaxFactory.ParseStatement(statement);
                var updated = Block.AddStatements(newNode.WithLeadingTrivia(SyntaxFactory.ElasticEndOfLine(Environment.NewLine)));

                var newRoot = Root.ReplaceNode(Block, updated);
                var newdoc = Context.Document.WithSyntaxRoot(newRoot);
                return Task.FromResult(newdoc);
            });
            Context.RegisterRefactoring(action);
        }

        public void Add(string name, Action<DocumentEditor> edit)
        {
            var action = CodeAction.Create(name, async t =>
            {
                var editor = await DocumentEditor.CreateAsync(Context.Document);
                edit(editor);
                return editor.GetChangedDocument();
            });
            Context.RegisterRefactoring(action);
        }
    }
}
