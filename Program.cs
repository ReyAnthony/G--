using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using GMinusMinus.interpreter;

namespace GMinusMinus
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            // TODO bug when error in REPL, then REPL won't work anymore
            if (args.Length > 0 && File.Exists(args[0]))
            {
                try
                {
                    var text = File.ReadAllText(args[0]);
                    var stream = CharStreams.fromstring(text);
                    var lexer = new ExprLexer(stream);
                    var tokens = new CommonTokenStream(lexer);
                    var parser = new ExprParser(tokens) {BuildParseTree = true};
                    IParseTree tree = parser.prog();
                    ParseTreeWalker.Default.Walk(new Listener(), tree); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
             
                Console.WriteLine("Back to the REPL");
                Console.ReadLine();
            }
            
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                try
                {
                    var stream = CharStreams.fromstring(line);
                    var lexer = new ExprLexer(stream);
                    var tokens = new CommonTokenStream(lexer);
                    var parser = new ExprParser(tokens) {BuildParseTree = true};
                    IParseTree tree = parser.prog();
                    ParseTreeWalker.Default.Walk(new Listener(), tree);  
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