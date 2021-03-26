using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Evaluator;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Lexer;
using MadLad.Compiler.CodeAnalysis.Syntax.Text;

namespace MadLad
{
    internal static class Program
    {
        private static string prompt = "→ ";
        private const string multi_line_prompt = "· ";
        private const string command_prompt = "#";

        private static void Main()
        {
            var variables = new Dictionary<Variable, object>();
            var textbuilder = new StringBuilder();
            
            Console.WriteLine("MadLad Compooler but its a REPL instead");
            while (true)
            {
                if (textbuilder.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(prompt);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(multi_line_prompt);
                    Console.ResetColor();
                }
                
                var input = Console.ReadLine();
                var isblank = string.IsNullOrWhiteSpace(input);

                if (isblank && textbuilder.Length == 0)
                {
                    break;
                }
                
                if (!isblank && input.StartsWith("#"))
                {
                    ProcessCommand(input);
                }
                else
                {
                    textbuilder.AppendLine(input);
                
                    var text = textbuilder.ToString();
                    if (showbasiclexer || showfullexer)
                    {
                        var Lexer = new Lexer(SourceText.From(text));
                        while (true)
                        {
                            var errors = Lexer.Errors;
                            var token = Lexer.Lex();
                            if (token.Kind == SyntaxKind.EOFToken)
                            {
                                Console.WriteLine();
                                break;
                            }

                            if (showbasiclexer)
                            {
                                ShowBasicLexer(token);
                            }

                            if (showfullexer)
                            {
                                ShowFullLexer(token);
                            }
                            if (errors.Any())
                            {
                                if (showbasiclexer)
                                {
                                    Console.WriteLine();
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        // blah blah compiler stuff
                        var syntaxtree = SyntaxTree.Parse(text);
                        if (!isblank && syntaxtree.Errors.Any())
                        {
                            continue;
                        }
                            
                        if (showtree)
                        {
                            ShowTree(syntaxtree.Root);   
                        }

                        // if there are errors dont evaluate
                        var compilation = new Compilation(syntaxtree);
                        var result = compilation.Evaluate(variables);
                        var errors = result.Errors;

                        if (!errors.Any())
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(result.Value);
                            Console.ResetColor();
                        }

                        var sourcetext = syntaxtree.Text;
                            
                        PrintErrors(errors, sourcetext, text);
                        textbuilder.Clear();
                    }
                }
            }
        }

        private static bool showbasiclexer;
        private static bool showfullexer;
        private static bool showtree;
        private static bool debug;
        private static void ProcessCommand(string command)
        {
            if (command.Contains($"{command_prompt}clear"))
            {
                Console.Clear();
            }
            else if (command.Contains($"{command_prompt}exit"))
            {
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
            }
            else if (command.Contains($"{command_prompt}DEBUG"))
            {
                debug = !debug;
                if (debug)
                {
                    Console.WriteLine("Debug Mode Enabled");
                    prompt = "DEBUG> ";
                }
                else
                {
                    Console.WriteLine("Debug Mode Disabled");
                    prompt = "> ";
                }
                showbasiclexer = false;
                showfullexer = false;
                showtree = false;
            }
            else if (command.Contains($"{command_prompt}showlexer --basic") && debug)
            {
                showbasiclexer = !showbasiclexer;
                showfullexer = false;
                Console.WriteLine(showbasiclexer ? "Basic Lexer Enabled" : "Basic Lexer Disabled");
            }
            else if (command.Contains($"{command_prompt}showlexer --full") && debug)
            {
                showfullexer = !showfullexer;
                showbasiclexer = false;
                Console.WriteLine(showfullexer ? "Full Lexer Enabled" : "Full Lexer Disabled");
            }
            else if (command.Contains($"{command_prompt}showtree") && debug)
            {
                showtree = !showtree;
                showbasiclexer = false;
                showfullexer = false;
                Console.WriteLine(showtree ? "Syntax Tree Enabled" : "Syntax Tree Disabled");
            }
            else if (command.Contains($"{command_prompt}help"))
            {
                ShowHelp();
            }
        }
        
        private static void ShowBasicLexer(SyntaxToken token)
        {
            if (token.Kind != SyntaxKind.WhitespaceToken && token.Value != null)
            {
                Console.Write($"[{token.Kind}:{token.Value}]");   
            }
            else if (token.Kind != SyntaxKind.WhitespaceToken)
            {
                Console.Write($"[{token.Kind}]");
            }
        }

        private static void ShowFullLexer(SyntaxToken token)
        {
            object value;
            if (token.Value == null)
            {
                value = "";
            }
            else
            {
                value = $"({token.Value.GetType()})";
            }
            Console.WriteLine($"Text:'{token.Text}'---Type:{token.Kind}---Value{value}:{token.Value}");   
        }

        private static void ShowTree(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";
            
            Console.Write(indent);
            Console.Write(marker);

            switch (node.Kind)
            {
                case SyntaxKind.BinaryExpression:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(node.Kind);
                    Console.ResetColor();
                    break;
                case SyntaxKind.GroupedExpression:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(node.Kind);
                    Console.ResetColor();
                    break;
                case SyntaxKind.UnaryExpression:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(node.Kind);
                    Console.ResetColor();
                    break;
                case SyntaxKind.LiteralExpression:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(node.Kind);
                    Console.ResetColor();
                    break;
                case SyntaxKind.AssignmentExpression:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write(node.Kind);
                    Console.ResetColor();
                    break;
                case SyntaxKind.NameExpression:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(node.Kind);
                    Console.ResetColor();
                    break;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(node.Kind);
                    Console.ResetColor();
                    break;
                default:
                    Console.ResetColor();
                    Console.Write(node.Kind);
                    break;
            }

            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += isLast ? "   " : "│  ";

            var last = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                ShowTree(child, indent, child == last);
            }
        }

        private static void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Commands:");
            Console.WriteLine("#help");
            Console.WriteLine("#DEBUG");
            Console.WriteLine("#showlexer --basic (DEBUG MODE ONLY)");
            Console.WriteLine("#showlexer --full (DEBUG MODE ONLY)");
            Console.WriteLine("#showtree (DEBUG MODE ONLY)");
            Console.WriteLine("#clear");
            Console.WriteLine("#exit");
            Console.ResetColor();
        }

        private static void PrintErrors(IEnumerable<Error> errors, SourceText text, string input)
        {
            foreach (var error in errors)
            {
                // Stuff for line numbers
                var lineindex = text.GetLineIndex(error.Span.Start);
                var linenumber = lineindex + 1;
                var line = text.Lines[lineindex];
                var character = error.Span.Start - line.Start + 1;

                // Print the message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"({linenumber}, {character}): {error} at: ");
                Console.ResetColor();
                
                var prefixspan = TextSpan.FromBounds(line.Start, error.Span.Start);
                var suffixspan = TextSpan.FromBounds(error.Span.End, line.End);
                
                // Prevent it from trying to highlight an empty token
                if (error.Details.Contains("Unexpected token <EOFToken>"))
                {
                    Console.WriteLine();
                    // Print arrow
                    Console.WriteLine(text);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    for (int _ = 0; _ < text.ToString(prefixspan).Length; _++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine("^");
                    Console.ResetColor();
                    break;
                }

                var prefix = text.ToString(prefixspan);
                var occurrence = text.ToString(error.Span);
                var suffix = text.ToString(suffixspan);

                // Print what is before the error
                Console.WriteLine("   ");
                Console.Write(prefix);
                            
                // Print where the error occurs
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(occurrence);
                Console.ResetColor();
                
                // Print what is after the error
                Console.Write(suffix);
                Console.WriteLine();
                
                // Print arrow
                Console.ForegroundColor = ConsoleColor.DarkRed;
                for (int _ = 0; _ < prefix.Length; _++)
                {
                    Console.Write(" ");
                }
                for (int _ = 0; _ < occurrence.Length; _++)
                {
                    Console.Write("^");
                }
                Console.WriteLine();
                Console.ResetColor();
            }
        }
    }
}
