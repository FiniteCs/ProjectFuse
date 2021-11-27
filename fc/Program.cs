using Fuse.CodeAnalysis;
using Fuse.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fuse
{
    internal static class Program
    {
        private static void Main()
        {
            bool showTree = false;
            Dictionary<VariableSymbol, object> variables = new();

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                if (line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing Parse Trees" : "Not Showing Parse Trees");
                    continue;
                }
                else if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                SyntaxTree syntaxTree = SyntaxTree.Parse(line);
                Compilation compilation = new(syntaxTree);
                EvaluationResult result = compilation.Evaluate(variables);

                IReadOnlyList<Diagnostic> diagnostics = result.Diagnostics;
                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    syntaxTree.Root.WriteTo(Console.Out);
                    Console.ResetColor();
                }
                if (!diagnostics.Any())
                    Console.WriteLine(result.Value);
                else
                {
                    foreach (Diagnostic diagnostic in diagnostics)
                    {
                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        string prefix = line[..diagnostic.Span.Start];
                        string error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                        string suffix = line[diagnostic.Span.End..];

                        Console.Write("    ");
                        Console.Write(prefix);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();

                        Console.Write(suffix);

                        Console.WriteLine();
                    }

                    Console.WriteLine();
                }
            }
        }
    }
}
