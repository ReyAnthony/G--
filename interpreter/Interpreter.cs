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
        
        public Listener()
        {
            
            Functions = 
                new Dictionary<string, Func<ExprContext, InterpreterFunc>>
                {
                    {"%%", e => new StatementsGroup(e)},
                    {"+", e => new Add(e)},
                    {"-", e => new Sub(e)},
                    {"/", e => new Div(e)},
                    {"%", e => new Mod(e)},
                    {"*", e => new Mult(e)},
                    {"<", e => new Less(e)},
                    {">", e => new More(e)},
                    {"==", e => new Eq(e)},
                    {"typeof", e => new TypeOf(e)},
                    {"not", e => new Not(e)},
                    {"when", e => new When(e)},
                    {"if", e => new If(e)},
                    {"let", e => new Let(e)},
                    {"set", e => new Set(e)},
                    {"ret", e => new Retrieve(e)},
                    {"print", e => new Print(e)},
                    {"read", e => new Read(e)},
                    {"and", e => new And(e)},
                    {"or", e => new Or(e)},
                    {"random", e => new Random(e)},  
                    {"function", e => new DefineFunction(e)},
                    {"apply", e => new Apply(e)}
                };
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
                            var func =
                                currentExpr.RetrieveFunctionFromLocalContext(currentExpr.FunctionName)(currentExpr);
                            Func<ExprContext, Value> a = exprContext =>
                            {
                                // FORGIVE ME FOR MY CODE SINS 
                                // HACK for the localContext in recursions
                                var custom = func as CustomFunc;
                                exprContext.StashLocalContext(custom.FuncDeclContext);
                                var funcReturn = custom.Execute();
                                exprContext.RevertLocalContext(custom.FuncDeclContext);
                                return funcReturn;
                            };
                            return a(currentExpr);
                        }
                        catch (UndefinedFunction e)
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
            catch (KeyNotFoundException ex)
            {
                throw new UndefinedFunction(currentExpr.FunctionName);
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
            else if (context.FALSY() != null)
            {
                func.AddArgument(new LazyValue(() => new Value(context.FALSY().GetText(), Types.Falsy)));
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