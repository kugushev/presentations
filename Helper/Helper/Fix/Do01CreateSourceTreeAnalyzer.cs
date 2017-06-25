using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Helper.Fix
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Do01CreateSourceTreeAnalyzer : DiagnosticAnalyzer
    {
        public static readonly string Id = "PH01";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            Id,
            "How to start working?",
            null,
            "Fix",
            DiagnosticSeverity.Warning,
            true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.MethodDeclaration);
        }

        private void Analyze(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is MethodDeclarationSyntax method && method.Identifier.Text == "Fix")
            {
                if (method.Body.Statements.Count == 1)
                {
                    var diagnostic = Diagnostic.Create(Rule, method.Body.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
