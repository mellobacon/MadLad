﻿using System;
using System.Collections.Generic;
using System.Linq;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Evaluator;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Lexer;

namespace MadLad
{
    internal static class Program
    {
        private static string prompt = "> ";
        private const string command_prompt = "#";

        private static void Main()
        {
            var variables = new Dictionary<Variable, object>();
            
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
                        PrintErrors(errors, input);
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

        private static void PrintErrors(IEnumerable<Error> errors, string input)
        {
            foreach (var error in errors)
            {
                
                // Print the message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{error} at: ");
                Console.ResetColor();
                
                // Prevent it from trying to highlight an empty token
                if (error.Details.Contains("EOF"))
                {
                    Console.WriteLine();
                    // Print arrow
                    Console.WriteLine(input);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    for (int _ = 0; _ < input.Substring(0, error.Span.Start).Length; _++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine("^");
                    Console.ResetColor();
                    break;
                }
                
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
                
                // Print arrow
                Console.ForegroundColor = ConsoleColor.DarkRed;
                for (int _ = 0; _ < prefix.Length; _++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("^");
                Console.ResetColor();
            }
        }
    }
}