using System;

namespace MadLad
{
    internal static class Program
    {
        private static void Main()
        {
            // set and run the repl
            REPL repl = new REPL
            {
                Prompt = "» ",
                Multiline_Prompt = "· ",
                Command_Prompt = "#",
                IsColored = true,
                Prompt_Color = ConsoleColor.Yellow,
                Multiline_Color = ConsoleColor.Yellow,
            };
            repl.Run();
        }
    }
}
