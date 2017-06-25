using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System.Composition;

namespace Helper.Fix
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public class Do01CreateSourceTreeFixer : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(Do01CreateSourceTreeAnalyzer.Id);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync().ConfigureAwait(false);

            var node = (MethodDeclarationSyntax)root.FindNode(context.Span);

            context.RegisterCodeFix(CodeAction.Create("Create syntax tree", c => DoFix(c, node, context.Document)), context.Diagnostics);
        }

        private async Task<Document> DoFix(CancellationToken token, MethodDeclarationSyntax node, Document document)
        {
            var returns = node.Body.Statements.OfType<ReturnStatementSyntax>().Single();

            var statement = SyntaxFactory.ParseStatement("SyntaxTree tree = CSharpSyntaxTree.ParseText(source);");

            var editor = await DocumentEditor.CreateAsync(document);
            editor.InsertBefore(returns, statement);
            return editor.GetChangedDocument();
        }
    }
}
