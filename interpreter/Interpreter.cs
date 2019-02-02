 using System;
 using System.Collections.Generic;
 using Antlr4.Runtime;
 using Antlr4.Runtime.Tree;

namespace Interpreter1
{
    internal class UndefinedFunction : Exception
    {
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
        protected readonly List<Value> Arguments = new List<Value>();
        public abstract Value Execute();

        public void AddArgument(Value arg)
        {
            Arguments.Add(arg);
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
                foreach (var argsContext in context.args())
                {
                    if (argsContext.INT() != null)
                    {
                        func.AddArgument(new Value(argsContext.INT().GetText(), Types.Int));
                    }
                    else if (argsContext.STRING() != null)
                    {
                        func.AddArgument(new Value(argsContext.STRING().GetText(), Types.String));
                    }
                    else if (argsContext.NAME() != null)
                    {
                        func.AddArgument(new Value(argsContext.NAME().GetText(), Types.Name));
                    }
                    else if (argsContext.EMPTY_LIST() != null)
                    {
                        func.AddArgument(new Value(argsContext.EMPTY_LIST().GetText(), Types.EmptyList));
                    }
                    
                    //if it's a function, then we do nothing,
                    //we wil rewind the call stack and manage it at that moment
                }
                _callStack.Push(func);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("Undefined function " + funcName);
            }
        }

        public void ExitExpr(ExprParser.ExprContext context)
        {
            var popdResult = _callStack.Pop().Execute();
           
            if (_callStack.Count > 0)
            {
                var stackTop = _callStack.Peek();
                stackTop.AddArgument(new Value(popdResult.Val, popdResult.Type));
            }
            else
            {
                Console.WriteLine(popdResult.Val);
            } 
        }

        public void EnterArgs(ExprParser.ArgsContext context)
        {
            
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