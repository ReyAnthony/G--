
using System;
using System.IO;
using Antlr4.Runtime;
using GMinusMinus;

namespace Interpreter
{
    public class A : IAntlrErrorListener<IToken>
    {
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
            string msg, RecognitionException e)
        {
            Console.WriteLine("{0}: line {1}/column {2} {3}", e, line, charPositionInLine, msg);
        }
    }
    
    internal static class Program
    { 
        public static void Main(string[] args)
        {
            if (args.Length > 0 && File.Exists(args[0]))
            {
                var interpreter = new GMinusInterpreter();
                try
                {
                    var text = File.ReadAllText(args[0]);
                    interpreter.SetCustomErrorListener(new A());
                    interpreter.Eval(text);   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
    }
}