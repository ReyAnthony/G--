using System;
using System.Linq;

namespace Interpreter1
{
    internal class UndefinedFunction : Exception
    {
        public UndefinedFunction(string name) : base($"The function {name} is undefined.")
        {
            
        }
    }
    
    internal class UndefinedVariable : Exception
    {
        public UndefinedVariable(string name) : base($"The variable {name} is undefined.")
        {
            
        }
    }
    
    internal class WrongArgumentCount : Exception
    {
        public WrongArgumentCount(string funcName, int minArgs, int maxArgs = Int32.MaxValue) 
            : base($"{funcName} must have at least {minArgs} argument(s) and at most {maxArgs} argument(s)")
        {
        }
    }
    
    internal class WrongType : Exception
    {
        public WrongType(string funcName, string msg = "", params Types[] excpectedTypes) 
            : base($"{funcName} needs " +
                   $"{excpectedTypes.Aggregate("", (str, next) => $"{str}{next}, ").TrimEnd(',', ' ')} types " +
                   $"{msg}")
        {
        }
    }
}