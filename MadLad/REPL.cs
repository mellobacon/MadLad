using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MadLad.Compiler.CodeAnalysis.ErrorReporting;
using MadLad.Compiler.CodeAnalysis.Evaluator;
using MadLad.Compiler.CodeAnalysis.Syntax;
using MadLad.Compiler.CodeAnalysis.Syntax.Lexer;
using MadLad.Compiler.CodeAnalysis.Syntax.Symbols;
using MadLad.Compiler.CodeAnalysis.Syntax.Text;

namespace MadLad
{
    public class REPL
    {
        /**
         * The REPL prompt
         */
        public string Prompt = "> ";
        /**
         * The prompt for multi lines
         */
        public string Multiline_Prompt = "- ";
        /**
         * The prompt for calling commands
         */
        public string Command_Prompt = "#";

        /**
         * Enables the repl to be colored. True for color. False for no color (white).
         * This must be true to set custom colors
         */
        public bool IsColored = false;
        /**
         * The REPL prompt color
         */
        public ConsoleColor Prompt_Color = ConsoleColor.White;
        /**
         * The multiline prompt color
         */
        public ConsoleColor Multiline_Color = ConsoleColor.White;
        /**
         * The color of the printed evaluation result
         */
        public ConsoleColor Result_Color = ConsoleColor.Green;

        private string defaultprompt;
        private Compilation previous;

        /**
         * Run the REPL
         */
        public void Run()
        {
            defaultprompt = Prompt;
            
            var variables = new Dictionary<VariableSymbol, object>();
            var textbuilder = new StringBuilder();

            Console.WriteLine("MadLad Compooler but its a REPL instead");
            // start getting input
            while (true)
            {
                // checks if single line
                if (textbuilder.Length == 0)
                {
                    if (IsColored)
                    {
                        Console.ForegroundColor = Prompt_Color;
                        Console.Write(Prompt);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(Prompt);
                    }
                }
                // checks if multiline
                else
                {
                    if (IsColored)
                    {
                        Console.ForegroundColor = Multiline_Color;
                        Console.Write(Multiline_Prompt);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(Multiline_Prompt);
                    }
                }
                
                var input = Console.ReadLine();
                var isblank = string.IsNullOrWhiteSpace(input);
                
                if (isblank && textbuilder.Length == 0)
                {
                    break; // exit the program
                }
                
                // checks if input is a command
                if (!isblank && input.StartsWith("#"))
                {
                    ProcessCommand(input);
                }
                // process the input for the compiler
                else
                {
                    // append /r/n to the end of the input
                    textbuilder.AppendLine(input);
                
                    var text = textbuilder.ToString();
                    
                    // debug options for showing the lexer
                    if (showbasiclexer || showfullexer)
                    {
                        var Lexer = new Lexer(SourceText.From(text));
                        
                        // get every token in the input
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
                        // parse the tree
                        var syntaxtree = SyntaxTree.Parse(text);
                        
                        // go back to the start to take input on another line
                        if (!isblank && syntaxtree.Errors.Any())
                        {
                            continue;
                        }
                        
                        // can show the syntax tree if needed
                        if (showtree)
                        {
                            ShowTree(syntaxtree.Root);   
                        }
                        
                        Compilation compilation;
                        // if the input is a single line, compile it
                        // else gather the multiline inputs and compile those together
                        if (previous == null)
                        {
                            compilation = new Compilation(syntaxtree);
                        }
                        else
                        {
                            compilation = previous.Continue(syntaxtree);   
                        }

                        // evaluate input(s) and error(s)
                        var result = compilation.Evaluate(variables);
                        var errors = result.Errors;

                        // print the result if there are no errors
                        if (!errors.Any())
                        {
                            if (IsColored)
                            {
                                Console.ForegroundColor = Result_Color;
                                Console.WriteLine(result.Value);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine(result.Value);
                            }

                            previous = compilation;
                        }

                        // if there are errors get the input where the error applies
                        // and print them
                        var sourcetext = syntaxtree.Text;
                        
                        PrintErrors(errors, sourcetext);
                        
                        // clear the textbuilder for the next input
                        textbuilder.Clear();
                    }
                }
            }
        }
        #region Commands and stuff
        private static bool showbasiclexer;
        private static bool showfullexer;
        private static bool showtree;
        private static bool debug;
        private void ProcessCommand(string command)
        {
            if (command.Contains($"{Command_Prompt}clear"))
            {
                Console.Clear();
            }
            else if (command.Contains($"{Command_Prompt}exit"))
            {
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
            }
            else if (command.Contains($"{Command_Prompt}DEBUG"))
            {
                debug = !debug;
                if (debug)
                {
                    Console.WriteLine("Debug Mode Enabled");
                    Prompt = "DEBUG> ";
                }
                else
                {
                    Console.WriteLine("Debug Mode Disabled");
                    Prompt = defaultprompt;
                }
                showbasiclexer = false;
                showfullexer = false;
                showtree = false;
            }
            else if (command.Contains($"{Command_Prompt}showlexer --basic") && debug)
            {
                showbasiclexer = !showbasiclexer;
                showfullexer = false;
                Console.WriteLine(showbasiclexer ? "Basic Lexer Enabled" : "Basic Lexer Disabled");
            }
            else if (command.Contains($"{Command_Prompt}showlexer --full") && debug)
            {
                showfullexer = !showfullexer;
                showbasiclexer = false;
                Console.WriteLine(showfullexer ? "Full Lexer Enabled" : "Full Lexer Disabled");
            }
            else if (command.Contains($"{Command_Prompt}showtree") && debug)
            {
                showtree = !showtree;
                showbasiclexer = false;
                showfullexer = false;
                Console.WriteLine(showtree ? "Syntax Tree Enabled" : "Syntax Tree Disabled");
            }
            else if (command.Contains($"{Command_Prompt}reset") && debug)
            {
                previous = null;
            }
            else if (command.Contains($"{Command_Prompt}help"))
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
            Console.WriteLine("#reset (DEBUG MODE ONLY)");
            Console.WriteLine("#clear");
            Console.WriteLine("#exit");
            Console.ResetColor();
        }

        private static void PrintErrors(IEnumerable<Error> errors, SourceText text)
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
                Console.Write($"({linenumber}, {character}): {error}");
                Console.ResetColor();

                // Prevent it from trying to highlight an empty token
                if (error.Details.Contains("Unexpected token <EOFToken>"))
                {
                    Console.WriteLine();
                    break;
                }
                
                var prefixspan = TextSpan.FromBounds(line.Start, error.Span.Start);
                var suffixspan = TextSpan.FromBounds(error.Span.End, line.End);

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
        #endregion
    }
}