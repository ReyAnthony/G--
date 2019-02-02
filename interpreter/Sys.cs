using System;
using System.Linq;

namespace Interpreter1
{
    internal class StatementsGroup : InterpreterFunc
    {
        public override Value Execute()
        {
           return new Value(null, Types.EmptyList);
        }
    }
    
    internal class Print : InterpreterFunc
    {
        public override Value Execute()
        {
            Console.WriteLine(Arguments.First().Val);
            return new Value(null, Types.EmptyList);
        }
    }
}