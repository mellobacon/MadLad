using System;
using System.Collections.Generic;
using System.Linq;
using MadLad.MadLad.Compiler.ErrorReporting;
using MadLad.MadLad.Compiler.Syntax;
using MadLad.MadLad.Compiler.Syntax.Lexer;

namespace MadLad.MadLad
{
    internal static class Program
    {
        static string prompt = "> ";
        const string command_prompt = "#";
        static void Main()
        {
            Console.WriteLine("MadLad Compooler but its a REPL instead");
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return;
                }

                if (input.StartsWith("#"))
                {
                    ProcessCommand(input);
                }
                else
                {
                    if (showbasiclexer || showfullexer)
                    {
                        var Lexer = new Lexer(input);
                        while (true)
                        {
                            // TODO lexer may or may not be working right idk
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
                                PrintErrors(errors, input);
                                break;
                            }
                        }
                    }
                    else
                    {
                        // blah blah compiler stuff
                        var syntaxtree = SyntaxTree.Parse(input);
                        // TODO a single error still persists (null ref for some reason)
                        var errors = syntaxtree.Errors;
                        if (showtree)
                        {
                            ShowTree(syntaxtree.Root);   
                        }
                        PrintErrors(errors, input);
                    }
                }
            }
        }
        
        static bool showbasiclexer;
        static bool showfullexer;
        static bool showtree;
        static bool debug;
        private static void ProcessCommand(string command)
        {
            if (command.Contains($"{command_prompt}clear"))
            {
                Console.Clear();
            }
            else if (command.Contains($"{command_prompt}exit"))
            {
                Console.Write("Exiting...");
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

        static void ShowBasicLexer(SyntaxToken token)
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

        static void ShowFullLexer(SyntaxToken token)
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
        
        static void ShowTree(SyntaxNode node, string indent = "", bool isLast = true)
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
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
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

        static void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Commands:");
            Console.WriteLine("#help");
            Console.WriteLine("#DEBUG");
            Console.WriteLine("#showlexer --basic (DEBUG MODE ONLY)");
            Console.WriteLine("#showlexer --basic (DEBUG MODE ONLY)");
            Console.WriteLine("#showtree (DEBUG MODE ONLY)");
            Console.WriteLine("#clear");
            Console.WriteLine("#exit");
            Console.ResetColor();
        }

        static void PrintErrors(IEnumerable<Error> errors, string input)
        {
            foreach (var error in errors)
            {
                // Prevent it from trying to highlight an empty token
                if (error.Details.Contains("EOF"))
                {
                    break;
                }
                
                // Print the message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{error} at: ");
                Console.ResetColor();

                var prefix = input.Substring(0, error.Span.Start);
                var occurrence = input.Substring(error.Span.Start, error.Span.Length);
                var suffix = input.Substring(error.Span.End);

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
            }
        }
    }
}
