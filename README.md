# ConsoleWriteLine Analyzer

The **ConsoleWriteLine Analyzer** is a Roslyn-based diagnostic tool designed to identify and comment out occurrences of `System.Console.WriteLine` in C# source code. This is particularly useful in scenarios where `Console.WriteLine` is used for debugging or diagnostic purposes but should not remain in production code, as it may:

- Leak sensitive information to standard output.
- Cause performance issues due to unnecessary I/O operations.
- Disrupt logging consistency when structured logging frameworks are in use.

## Features

- **Detection**: Scans code for `Console.WriteLine` statements.
- **Code Fix**: Provides an automated fix to comment out the detected statements.

## Installation

To integrate the ConsoleWriteLine Analyzer into your project:

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/ConsoleWriteLineAnalyzer.git
```

Replace `yourusername` with your actual GitHub username.

### 2. Build the Analyzer

Navigate to the cloned directory and build the project:

```bash
cd ConsoleWriteLineAnalyzer
dotnet build
```

This will generate the necessary DLL files for the analyzer.

### 3. Add the Analyzer to Your Project

#### For .NET Core or .NET 5+ Projects:

Create a `Directory.Build.props` file in the root of your solution (if it doesn't exist) and add the following:

```xml
<Project>
  <ItemGroup>
    <Analyzer Include="path\to\ConsoleWriteLineAnalyzer.dll" />
  </ItemGroup>
</Project>
```

Replace `path\to\ConsoleWriteLineAnalyzer.dll` with the actual path to the DLL generated in the build step.

#### For .NET Framework Projects:

Manually add the analyzer DLL to your project:

1. Right-click on your project in Visual Studio and select **"Add" > "Existing Item..."**.
2. Navigate to the analyzer DLL and add it.
3. Right-click the added DLL in the **Solution Explorer**, select **"Properties"**, and set **"Build Action"** to **"Analyzer"**.

## Usage

Once the analyzer is referenced in your project:

- **Automatic Analysis**: Visual Studio will automatically run the analyzer as you write code, highlighting any `Console.WriteLine` usages.
- **Applying Code Fixes**: To apply the suggested fix:
  1. Hover over the highlighted `Console.WriteLine` statement.
  2. Click on the light bulb icon that appears.
  3. Select the option to comment out the `Console.WriteLine` statement.

### Example

#### Before applying the fix:

```csharp
Console.WriteLine("Debugging...");
```

#### After applying the fix:

```csharp
// Console.WriteLine("Debugging...");
;
```

## Implementation Details

### 1. Analyzer Detection
- The analyzer scans `InvocationExpressionSyntax` nodes in the Abstract Syntax Tree (AST) to detect method calls.
- It uses the `SemanticModel` to verify that the method being invoked is `System.Console.WriteLine`.

### 2. Code Fix
- When a `Console.WriteLine` statement is detected, the code fix comments out the entire statement to neutralize its effect.
- The statement is replaced with an `EmptyStatementSyntax`, and a comment is added as leading trivia.

## How the Fix Addresses the Issue

- **Removes Potential Output**: Commenting out the `Console.WriteLine` statement prevents its execution, ensuring no sensitive information is output to the console.
- **Code Preservation**: By commenting instead of deleting, developers retain the original statement for reference or future debugging purposes.
- **Improves Production Readiness**: Ensures that temporary debugging aids do not inadvertently make it to production code, enhancing the maintainability and security of the codebase.

---

By integrating the ConsoleWriteLine Analyzer directly from the source, you can maintain a clean codebase, free from unintended `Console.WriteLine` statements, thereby enhancing both security and performance.