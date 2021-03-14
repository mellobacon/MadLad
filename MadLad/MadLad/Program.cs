using System;
using System.Linq;
using MadLad.Lexer;

namespace MadLad
{
    internal static class Program
    {
        const string prompt = "> ";
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
                ProcessCommand(input);

                // blah blah compiler stuff
                var Lexer = new Lexer.Lexer(input);
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
                        if (token.Kind != SyntaxKind.WhitespaceToken && token.Value != null)
                        {
                            Console.Write($"[{token.Kind}:{token.Value}]");   
                        }
                        else if (token.Kind != SyntaxKind.WhitespaceToken)
                        {
                            Console.Write($"[{token.Kind}]");
                        }
                    }

                    if (showfullexer)
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
                    if (errors.Any())
                    {
                        foreach (var error in errors)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine(error.Details);
                            Console.ResetColor();
                        }

                        break;
                    }
                }
            }
        }
        
        static bool showbasiclexer;
        static bool showfullexer;
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
                Console.WriteLine(debug ? "Debug Mode Enabled" : "Debug Mode Disabled");
                showbasiclexer = false;
                showfullexer = false;
            }
            else if (command.Contains($"{command_prompt}showlexer --basic") && debug)
            {
                showbasiclexer = true;
                showfullexer = false;
                Console.WriteLine("Basic Lexer Enabled");
            }
            else if (command.Contains($"{command_prompt}showlexer --full") && debug)
            {
                showfullexer = true;
                showbasiclexer = false;
                Console.WriteLine("Full Lexer Enabled");
            }
        }
    }
}
