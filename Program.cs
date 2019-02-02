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
            functions.Add("typeof", () => new TypeOf());
            functions.Add("eq", () => new Eq());
            functions.Add("not", () => new Not());
            functions.Add("when", () => new When());
            functions.Add("def", () => new Def());
            functions.Add("ret", () => new Retrieve());
            
            functions.Add("%%", () => new StatementsGroup());
            
            //side effects
            functions.Add("print", () => new Print());
            functions.Add("read", () => new Read());
            
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
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }
    }
}