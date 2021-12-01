﻿using Fuse.CodeAnalysis;
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
            bool showProgram = false;
            Dictionary<VariableSymbol, object> variables = new();
            StringBuilder textBuilder = new();
            Compilation previous = null;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (textBuilder.Length == 0)
                    Console.Write("» ");
                else
                    Console.Write("· ");

                Console.ResetColor();

                string input = Console.ReadLine();
                bool isBlank = string.IsNullOrWhiteSpace(input);

                if (textBuilder.Length == 0)
                {
                    if (isBlank)
                    {
                        break;
                    }
                    else if (input == "#showTree")
                    {
                        showTree = !showTree;
                        Console.WriteLine(showTree ? "Showing Parse Tree." : "Not Showing Parse Tree.");
                        continue;
                    }
                    else if (input == "#showProgram")
                    {
                        showProgram = !showProgram;
                        Console.WriteLine(showProgram ? "Showing Bound Tree." : "Not Showing Bound Tree.");
                        continue;
                    }
                    else if (input == "#cls")
                    {
                        Console.Clear();
                        continue;
                    }
                    else if (input == "#reset")
                    {
                        previous = null;
                        continue;
                    }
                }

                textBuilder.AppendLine(input);
                string text = textBuilder.ToString();

                SyntaxTree syntaxTree = SyntaxTree.Parse(text);
                if (!isBlank && syntaxTree.Diagnostics.Any())
                {
                    continue;
                }

                Compilation compilation = previous == null 
                                          ? new(syntaxTree)
                                          : previous.ContinueWith(syntaxTree);

                if (showTree)
                {
                    syntaxTree.Root.WriteTo(Console.Out);
                }

                if (showProgram)
                    compilation.EmitTree(Console.Out);

                EvaluationResult result = compilation.Evaluate(variables);

                if (!result.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(result.Value);
                    Console.ResetColor();
                    previous = compilation;
                }
                else
                {

                    foreach (Diagnostic diagnostic in result.Diagnostics)
                    {
                        int lineIndex = syntaxTree.Text.GetLineIndex(diagnostic.Span.Start);
                        int lineNumber = lineIndex + 1;
                        TextLine line = syntaxTree.Text.Lines[lineIndex];
                        int character = diagnostic.Span.Start - line.Start + 1;

                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write($"({lineNumber}, {character}): ");
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        TextSpan prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Start);
                        TextSpan suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.End);

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
