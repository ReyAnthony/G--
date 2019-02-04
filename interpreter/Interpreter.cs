 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Net.Configuration;
 using Antlr4.Runtime;
 using Antlr4.Runtime.Tree;

namespace Interpreter1
{
    public class Value
    {
        public static Value Yes()
        {
            return new Value("yes", Types.Name);
        }
        
        public static Value No()
        {
            return new Value("no", Types.Falsy);
        }
        
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
        Falsy
    }

    public class ExprContext
    {
        public string FunctionName { get; }
        public List<LazyValue> Arguments { get; }
        private Dictionary<string, Func<ExprContext, InterpreterFunc>> LocalFunctions { get; }
        private Dictionary<string, Value> LocalVariables { get; }
        private ExprContext Parent { get; }

        public ExprContext(string functionName, ExprContext parent)
        {
            this.FunctionName = functionName;
            Parent = parent;
            LocalFunctions = new Dictionary<string, Func<ExprContext, InterpreterFunc>>();
            LocalVariables = new Dictionary<string, Value>();
            Arguments = new List<LazyValue>();
        }
        
        public void AddArgument(LazyValue arg)
        {
            Arguments.Add(arg);
        }

        public Value RetrieveValueFromContext(string name)
        {  
            try
            {
                return LocalVariables[name];
            }
            catch (KeyNotFoundException ex)
            {
                if (Parent == null)
                {
                    throw new UndefinedVariable(name);
                }
                return Parent.RetrieveValueFromContext(name);
            }          
        }
        
        public void AddValueToLocalContext(string name, Value val)
        {
            LocalVariables.Add(name, val);
        }
        
        public Func<ExprContext, InterpreterFunc> RetrieveFunctionFromLocalContext(string name)
        {  
            try
            {
                return LocalFunctions[name];
            }
            catch (KeyNotFoundException ex)
            {
                if (Parent == null)
                {
                    throw new UndefinedFunction(name);
                }
                return Parent.RetrieveFunctionFromLocalContext(name);
            }          
        }
        
        public void AddFunctionToLocalContext(string name, Func<ExprContext, InterpreterFunc> val)
        {
            if (Parent == null)
            {
                throw new Exception("TODO should not define functions at toplevel");
            }
            Parent.LocalFunctions.Add(name, val);
        }
    }

    public abstract class InterpreterFunc
    {

        protected readonly ExprContext Context;
        public abstract Value Execute();

        public InterpreterFunc(ExprContext context)
        {
            Context = context;
        }

        protected InterpreterFunc()
        {
            throw new NotImplementedException();
        }
    }
    
    public class Listener : IExprListener
    {

        public Dictionary<string, Func<ExprContext,InterpreterFunc>> Functions { get; }
        private Stack<ExprContext> _callStack = new Stack<ExprContext>();

        public Listener()
        {
            Functions = 
                new Dictionary<string, Func<ExprContext, InterpreterFunc>>
                {
                    {"%%", (e) => new StatementsGroup(e)},
                    {"+", (e) => new Add(e)},
                    {"-", (e) => new Sub(e)},
                    {"/", (e) => new Div(e)},
                    {"%", (e) => new Mod(e)},
                    {"*", (e) => new Mult(e)},
                    {"<", (e) => new Less(e)},
                    {">", (e) => new More(e)},
                    {"==", (e) => new Eq(e)},
                    {"typeof", (e) => new TypeOf(e)},
                    {"not", (e) => new Not(e)},
                    {"when", (e) => new When(e)},
                    {"if", (e) => new If(e)},
                    {"def", (e) => new Def(e)},
                    {"ret", (e) => new Retrieve(e)},
                    {"print", (e) => new Print(e)},
                    {"read", (e) => new Read(e)},
                    {"and", (e) => new And(e)},
                    {"or", (e) => new Or(e)},
                    {"random", (e) => new Random(e)},  
                    {"function", (e) => new DefineFunction(e)}
                };
        }
        
        public void EnterExpr(ExprParser.ExprContext context)
        {
            var funcName = context.NAME().GetText();
            var exprContext = new ExprContext(funcName, parent: _callStack.Count > 0 ? _callStack.Peek() : null);
            _callStack.Push(exprContext);
            
        }

        public void ExitExpr(ExprParser.ExprContext context)
        {
            var currentExpr = _callStack.Pop();
           
            if (_callStack.Count > 0)
            {
                var parentExpr = _callStack.Peek();
           
                parentExpr.AddArgument(new LazyValue(() =>
                {
                    //TODO the local should be first to allow for 
                    //variable override
                    try
                    {
                        var func = Functions[currentExpr.FunctionName](currentExpr);
                        return func.Execute();
                    }
                    catch (KeyNotFoundException e)
                    {
                        var func = currentExpr.RetrieveFunctionFromLocalContext(currentExpr.FunctionName)(currentExpr);
                        return func.Execute();
                    }
                }));
            }
            else
            {
                //top level can never be a local function
                var func = Functions[currentExpr.FunctionName](currentExpr);
                Console.WriteLine(func.Execute().Val);
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