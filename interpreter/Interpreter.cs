using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using GMinusMinus.interpreter.functions;
using Random = GMinusMinus.interpreter.functions.Random;

namespace GMinusMinus.interpreter
{
    public enum Types
    {
        Int,
        FloatingPoint,
        Name,
        String,
        Falsy
    }
    
    public class Listener : IExprListener
    {
        private Dictionary<string, Func<ExprContext,InterpreterFunc>> Functions { get; set; }
        private readonly Stack<ExprContext> _callStack = new Stack<ExprContext>();
        
        public Listener(Dictionary<string, Func<ExprContext, InterpreterFunc>> functions)
        {
            Functions = functions;
        }
        
        public void EnterExpr(ExprParser.ExprContext context)
        {
            var funcName = context.NAME().GetText();
            var exprContext = new ExprContext(funcName, _callStack.Count > 0 ? _callStack.Peek() : null);      
            _callStack.Push(exprContext);  
        }

        public void ExitExpr(ExprParser.ExprContext context)
        {
            var currentExpr = _callStack.Pop();
            try
            {
                if (_callStack.Count > 0)
                {
                    var parentExpr = _callStack.Peek();
                    parentExpr.AddArgument(new LazyValue(() =>
                    {
                        try
                        {
                            var func = currentExpr.RetrieveFunctionFromLocalContext(currentExpr.FunctionName)(currentExpr);
                            Func<ExprContext, Value> a = exprContext =>
                            {
                                // HACK for the localContext in recursions
                                var custom = (CustomFunc) func;
                                exprContext.StashLocalContext(custom.FuncDeclContext);
                                var funcReturn = custom.Execute();
                                exprContext.RevertLocalContext(custom.FuncDeclContext);
                                return funcReturn;
                            };
                            return a(currentExpr);
                        }
                        catch (UndefinedFunction)
                        {
                            var func = Functions[currentExpr.FunctionName](currentExpr);
                            return func.Execute();
                        }
                    }));
                }
                else
                {
                    //top level can never be a local function, ne need to check
                    var func = Functions[currentExpr.FunctionName](currentExpr);
                    Console.WriteLine(func.Execute().Val);
                } 
            } 
            catch (KeyNotFoundException)
            {
                throw new UndefinedFunction(currentExpr.FunctionName);
            }   
        }

        public void EnterArgs(ExprParser.ArgsContext context)
        {
            var currentExpr = _callStack.Peek();
            
            if (context.INT() != null)
            {
                currentExpr.AddArgument(new LazyValue(() => new Value(context.INT().GetText(), Types.Int)));
            }
            else if (context.FLOATING() != null)
            {
                currentExpr.AddArgument(new LazyValue(() => new Value(context.FLOATING().GetText(), Types.FloatingPoint)));
            }
            else if (context.STRING() != null)
            {
                currentExpr.AddArgument(new LazyValue(() => new Value(context.STRING().GetText(), Types.String)));
            }
            else if (context.NAME() != null)
            {
                currentExpr.AddArgument(new LazyValue(() => new Value(context.NAME().GetText(), Types.Name)));
            }
            else if (context.FALSY() != null)
            {
                currentExpr.AddArgument(new LazyValue(() => new Value(context.FALSY().GetText(), Types.Falsy)));
            }
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