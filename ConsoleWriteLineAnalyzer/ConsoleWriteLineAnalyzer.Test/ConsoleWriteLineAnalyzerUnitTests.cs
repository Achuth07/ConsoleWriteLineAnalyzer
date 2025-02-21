using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ConsoleWriteLineAnalyzer.Test.CSharpCodeFixVerifier<
    ConsoleWriteLineAnalyzer.ConsoleWriteLineAnalyzerAnalyzer,
    ConsoleWriteLineAnalyzer.ConsoleWriteLineAnalyzerCodeFixProvider>;

namespace ConsoleWriteLineAnalyzer.Test
{
    [TestClass]
    public class ConsoleWriteLineAnalyzerUnitTests
    {
        [TestMethod]
        public async Task TestConsoleWriteLine_Diagnostic()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;

class Program
{
    static void Main()
    {
        [|Console.WriteLine(""Hello"");|]
    }
}
", @"
using System;

class Program
{
    static void Main()
    {
    }
}
");
        }

        [TestMethod]
        public async Task TestNoConsoleWriteLine_NoDiagnostic()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;

class Program
{
    static void Main()
    {
        Console.Write(""Hello"");
    }
}
");
        }
    }
}
