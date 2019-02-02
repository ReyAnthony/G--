 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Net.Configuration;
 using Antlr4.Runtime;
 using Antlr4.Runtime.Tree;

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

    public class Value
    {
        public string Val { get; }
        public Types Type { get; }

        public Value(string value, Types type)
        {
            Val = value;
            Type = type;
        }
    }

    public class LazyValue
    {
        private Func<Value> _lazy;

        public LazyValue(Func<Value> val)
        {
            _lazy = val;
        }

        public Value Execute()
        {
            return _lazy.Invoke();
        }
    }

    public enum Types
    {
        Int,
        FloatingPoint,
        Name,
        String,
        EmptyList
    }

    public abstract class InterpreterFunc
    {
        protected readonly List<LazyValue> Arguments = new List<LazyValue>();
        private readonly Dictionary<string, Value> _context = new Dictionary<string, Value>();
        private InterpreterFunc _parent;
        
        public abstract Value Execute();

        public void AddArgument(LazyValue arg)
        {
            Arguments.Add(arg);
        }

        public void SetParent(InterpreterFunc parent)
        {
            _parent = parent;
        }

        public Value RetrieveValueFromContext(string name)
        {  
            try
            {
                return _context[name];
            }
            catch (KeyNotFoundException ex)
            {
                if (_parent == null)
                {
                    throw new UndefinedVariable(name);
                }
                return _parent.RetrieveValueFromContext(name);
            }          
        }
        
        public void AddValueToLocalContext(string name, Value val)
        {
            _context.Add(name, val);
        }
    }
    
    public class Listener : IExprListener
    {

        private Dictionary<string, Func<InterpreterFunc>> _functions;
        private Stack<InterpreterFunc> _callStack = new Stack<InterpreterFunc>();

        public Listener(Dictionary<string, Func<InterpreterFunc>> functions)
        {
            _functions = functions;
        }
        
        public void EnterExpr(ExprParser.ExprContext context)
        {
            var funcName = context.NAME().GetText();
            try
            {
                var func = _functions[funcName]();
                if (_callStack.Count > 0)
                    func.SetParent(_callStack.Peek());
                _callStack.Push(func);
            }
            catch (KeyNotFoundException ex)
            {
                throw new UndefinedFunction(funcName);
            }
        }

        public void ExitExpr(ExprParser.ExprContext context)
        {
            var lastExpr = _callStack.Pop();
           
            if (_callStack.Count > 0)
            {
                var stackTop = _callStack.Peek();
                stackTop.AddArgument(new LazyValue(() => lastExpr.Execute()));
            }
            else
            {
                Console.WriteLine(lastExpr.Execute().Val);
            } 
        }

        public void EnterArgs(ExprParser.ArgsContext context)
        {
            var func = _callStack.Peek();
            
            if (context.INT() != null)
            {
                func.AddArgument(new LazyValue(() => new Value(context.INT().GetText(), Types.Int)));
            }
            else if (context.FLOATING() != null)
            {
                func.AddArgument(new LazyValue(() => new Value(context.FLOATING().GetText(), Types.FloatingPoint)));
            }
            else if (context.STRING() != null)
            {
                func.AddArgument(new LazyValue(() => new Value(context.STRING().GetText(), Types.String)));
            }
            else if (context.NAME() != null)
            {
                func.AddArgument(new LazyValue(() => new Value(context.NAME().GetText(), Types.Name)));
            }
            else if (context.EMPTY_LIST() != null)
            {
                func.AddArgument(new LazyValue(() => new Value(context.EMPTY_LIST().GetText(), Types.EmptyList)));
            }
                
            //if it's a function, then we do nothing,
            //we wil rewind the call stack and manage it at that moment
        }

        public void ExitArgs(ExprParser.ArgsContext context)
        {
           
        }
        
        public void VisitTerminal(ITerminalNode node)
        {
            
        }

        public void VisitErrorNode(IErrorNode node)
        {

        }

        public void EnterEveryRule(ParserRuleContext ctx)
        {
          
        }

        public void ExitEveryRule(ParserRuleContext ctx)
        {
           
        }

        public void EnterProg(ExprParser.ProgContext context)
        {
            
        }

        public void ExitProg(ExprParser.ProgContext context)
        {
            
        }
    }
}