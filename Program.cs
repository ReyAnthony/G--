using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Interpreter1
{
    internal class Program
    {
        public static void Main(string[] args)
        {

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