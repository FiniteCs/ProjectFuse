using Fuse.CodeAnalysis;
using Fuse.CodeAnalysis.Syntax;
using Fuse.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse
{
    internal static class Program
    {
        private static void Main()
        {
            bool showTree = false;
            Dictionary<VariableSymbol, object> variables = new();
            var textBuilder = new StringBuilder();

            while (true)
            {
                if (textBuilder.Length == 0)
                    Console.Write("> ");
                else
                    Console.Write("| ");

                string input = Console.ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);

                if (textBuilder.Length == 0)
                {
                    if (isBlank)
                    {
                        break;
                    }
                    else if (input == "#showTree")
                    {
                        showTree = !showTree;
                        Console.WriteLine(showTree ? "Showing Parse Trees" : "Not Showing Parse Trees");
                        continue;
                    }
                    else if (input == "#cls")
                    {
                        Console.Clear();
                        continue;
                    }
                }

                textBuilder.AppendLine(input);
                var text = textBuilder.ToString();

                SyntaxTree syntaxTree = SyntaxTree.Parse(text);
                if (!isBlank && syntaxTree.Diagnostics.Any())
                {
                    continue;
                }

                Compilation compilation = new(syntaxTree);
                EvaluationResult result = compilation.Evaluate(variables);

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    syntaxTree.Root.WriteTo(Console.Out);
                    Console.ResetColor();
                }

                if (!result.Diagnostics.Any())
                    Console.WriteLine(result.Value);
                else
                {

                    foreach (Diagnostic diagnostic in result.Diagnostics)
                    {
                        var lineIndex = syntaxTree.Text.GetLineIndex(diagnostic.Span.Start);
                        var lineNumber = lineIndex + 1;
                        var line = syntaxTree.Text.Lines[lineIndex];
                        var character = diagnostic.Span.Start - line.Start + 1;

                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write($"({lineNumber}, {character}): ");
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        var prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Start);
                        var suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.End);

                        string prefix = syntaxTree.Text.ToString(prefixSpan);
                        string error = syntaxTree.Text.ToString(diagnostic.Span);
                        string suffix = syntaxTree.Text.ToString(suffixSpan);

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

                textBuilder.Clear();
            }
        }
    }
}
