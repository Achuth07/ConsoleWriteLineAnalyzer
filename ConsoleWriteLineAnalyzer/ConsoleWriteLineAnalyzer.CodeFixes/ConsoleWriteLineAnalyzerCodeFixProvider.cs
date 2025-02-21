using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace ConsoleWriteLineAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConsoleWriteLineAnalyzerCodeFixProvider))]
    public class ConsoleWriteLineAnalyzerCodeFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ConsoleWriteLineAnalyzerAnalyzer.DiagnosticId);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var invocation = root.FindNode(diagnosticSpan).FirstAncestorOrSelf<InvocationExpressionSyntax>();
            context.RegisterCodeFix(
                CodeAction.Create(
                    "Remove Console.WriteLine",
                    c => RemoveInvocationAsync(context.Document, invocation, c),
                    nameof(ConsoleWriteLineAnalyzerCodeFixProvider)),
                diagnostic);
        }

        private async Task<Document> RemoveInvocationAsync(Document document, InvocationExpressionSyntax invocation, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var newRoot = root.RemoveNode(invocation, SyntaxRemoveOptions.KeepNoTrivia);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
