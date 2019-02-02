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
            Dictionary<string, Func<InterpreterFunc>> functions = new Dictionary<string, Func<InterpreterFunc>>();
            functions.Add("+", () => new Add());
            functions.Add("-", () => new Sub());
            functions.Add("/", () => new Div());
            functions.Add("*", () => new Mult());
            
            functions.Add("%%", () => new StatementsGroup());
            functions.Add("print", () => new Print());
            
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
                    ParseTreeWalker.Default.Walk(new Listener(functions), tree);  
                }
                catch (Exception ex)
                {
                    Console.WriteLine("You tried to do something weird don't you ?");
                }
                Console.ReadLine();
            }
        }
    }
}