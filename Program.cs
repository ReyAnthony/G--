using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using GMinusMinus.interpreter;

namespace GMinusMinus
{
    internal static class Program
    { 
        private static void Parse(string line)
        {
            var stream = CharStreams.fromstring(line);
            var lexer = new ExprLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ExprParser(tokens) {BuildParseTree = true};
            IParseTree tree = parser.prog();
            ParseTreeWalker.Default.Walk(new Listener(), tree);  
        }
        
        public static void Main(string[] args)
        {
            if (args.Length > 0 && File.Exists(args[0]))
            {
                try
                {
                    var text = File.ReadAllText(args[0]);
                    Parse(text);  
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("Back to the REPL");
                }
                Console.ReadLine();
            }
            
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                try
                {
                    Parse(line);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }
    }
}