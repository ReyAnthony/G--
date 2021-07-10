
using System;
using System.IO;
using GMinusMinus;

namespace Interpreter
{
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