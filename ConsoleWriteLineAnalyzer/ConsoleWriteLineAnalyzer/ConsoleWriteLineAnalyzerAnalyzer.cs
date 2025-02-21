using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ConsoleWriteLineAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ConsoleWriteLineAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CW0001";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Avoid using Console.WriteLine",
            "Remove the call to 'Console.WriteLine'",
            "Best Practices",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Expression is IdentifierNameSyntax identifier &&
                identifier.Identifier.Text == "Console" &&
                memberAccess.Name.Identifier.Text == "WriteLine")
            {
                var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
