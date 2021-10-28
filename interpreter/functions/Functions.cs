using System;
using System.Globalization;
using System.Linq;

namespace GMinusMinus.interpreter.functions
{
    internal class Add : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return Int32.MaxValue; }
        }
        
        public Add(ExprContext context) : base(context)
        {}

        protected override Value ExecuteImpl()
        {
            float added = 0;
            var returnType = Types.Int;
            Context.Arguments.ForEach(lazy =>
            {
                var a = lazy.Execute();
                if (a.Type == Types.FloatingPoint)
                {
                    added += float.Parse(a.Val, CultureInfo.InvariantCulture);
                    returnType = Types.FloatingPoint;
                }
                else
                {
                    added += int.Parse(a.Val);
                }
            });

            return new Value(added.ToString(CultureInfo.InvariantCulture), returnType);
        }
    }

    internal class Sub : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return Int32.MaxValue; }
        }
        public Sub(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            float subbed = 0;
            var returnType = Types.Int;
            for (var i = 0; i < Context.Arguments.Count; i++)
            {
                var a = Context.Arguments[i].Execute();
                if (a.Type == Types.FloatingPoint)
                {
                    var val = float.Parse(a.Val, CultureInfo.InvariantCulture);
                    if (i == 0)
                        subbed = val;
                    else
                        subbed -= val;
                    returnType = Types.FloatingPoint;
                }
                else
                {
                    var val = int.Parse(a.Val);
                    if (i == 0)
                        subbed = val;
                    else
                        subbed -= val;
                }
            }

            return new Value(subbed.ToString(CultureInfo.InvariantCulture), returnType);
        }
    }

    internal class Div : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return Int32.MaxValue; }
        }
        
        public Div(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            float divd = 0;
            const Types returnType = Types.FloatingPoint;
            for (var i = 0; i < Context.Arguments.Count; i++)
            {
                var a = Context.Arguments[i].Execute();
                if (a.Type == Types.FloatingPoint)
                {
                    var val = float.Parse(a.Val, CultureInfo.InvariantCulture);
                    if (i == 0)
                        divd = val;
                    else
                        divd /= val;
                }
                else
                {
                    var val = int.Parse(a.Val);
                    if (i == 0)
                        divd = val;
                    else
                        divd /= val;
                }
            }

            return new Value(divd.ToString(CultureInfo.InvariantCulture), returnType);
        }
    }
    
    internal class Mod : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return Int32.MaxValue; }
        }
        
        public Mod(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            float divd = 0;
            const Types returnType = Types.FloatingPoint;
            for (var i = 0; i < Context.Arguments.Count; i++)
            {
                var a = Context.Arguments[i].Execute();
                if (a.Type == Types.FloatingPoint)
                {
                    var val = float.Parse(a.Val, CultureInfo.InvariantCulture);
                    if (i == 0)
                        divd = val;
                    else
                        divd %= val;
                }
                else
                {
                    var val = int.Parse(a.Val);
                    if (i == 0)
                        divd = val;
                    else
                        divd %= val;
                }
            }

            return new Value(divd.ToString(CultureInfo.InvariantCulture), returnType);
        }
    }

    internal class Mult : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return Int32.MaxValue; }
        }
        
        public Mult(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            float mult = 0;
            const Types returnType = Types.FloatingPoint;
            for (var i = 0; i < Context.Arguments.Count; i++)
            {
                var a = Context.Arguments[i].Execute();
                if (a.Type == Types.FloatingPoint)
                {
                    var val = float.Parse(a.Val, CultureInfo.InvariantCulture);
                    if (i == 0)
                        mult = val;
                    else
                        mult *= val;
                }
                else
                {
                    var val = int.Parse(a.Val);
                    if (i == 0)
                        mult = val;
                    else
                        mult *= val;
                }
            }

            return new Value(mult.ToString(CultureInfo.InvariantCulture), returnType);
        }
    }

    internal class StatementsGroup : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return Int32.MaxValue; }
        }
        
        public StatementsGroup(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            Context.Arguments.GetRange(0, Context.Arguments.Count - 1).ForEach(a => a.Execute());
            return Context.Arguments.Last().Execute();
        }
    }

    internal class Print : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return Int32.MaxValue; }
        }
        
        public Print(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            var fullStr = "";
            Value last = null;
            foreach (var lazy in Context.Arguments)
            {
                
                var argument = lazy.Execute();
                if (argument.Type == Types.Falsy)
                {
                    continue;
                }
                
                if (argument.Type != Types.String && 
                    argument.Type != Types.Name && 
                    argument.Type != Types.Int && 
                    argument.Type != Types.FloatingPoint)
                {
                    throw new WrongType(Context.FunctionName, "", Types.String, Types.Int, Types.FloatingPoint);
                }
                fullStr += argument.Val;
                last = argument;
            }

            if (last != null)
            {
                Console.WriteLine(fullStr.Replace("\"", string.Empty));
            }
            return last ?? Value.No();
        }
    }

    internal class Read : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 0; }
        }

        public override int MaxArgs
        {
            get { return 0; }
        }
        
        public Read(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            var read = Console.ReadLine();
            // HACK pseudo flush for repl
            if (read == string.Empty)
            {
                read = Console.ReadLine();
            }
            return new Value(read, Types.String);
        }
    }

    internal class TypeOf : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return 1; }
        }
        
        public TypeOf(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        { 
            return new Value(Context.Arguments.First().Execute().Type.ToString(), Types.String);
        }
    }

    internal class Eq : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 2; }
        }

        public override int MaxArgs
        {
            get { return 2; }
        }
        
        public Eq(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            return Context.Arguments.First().Execute().Val
                          .Equals(Context.Arguments.Last().Execute().Val) ? Value.Yes() : Value.No();
        }
    }
    
    internal class Less : InterpreterFunc
     {
         
         public override int MinArgs
         {
             get { return 2; }
         }

         public override int MaxArgs
         {
             get { return 2; }
         }
         
         public Less(ExprContext context) : base(context)
         {
         }
 
         protected override Value ExecuteImpl()
         {
             var arg1 = Context.Arguments.First().Execute();
             var arg2 = Context.Arguments.Last().Execute();
             
             if (arg1.Type != Types.Int && 
                 arg1.Type != Types.FloatingPoint && 
                 arg2.Type != Types.FloatingPoint &&
                 arg2.Type != Types.Int)
             {
                 throw new WrongType(Context.FunctionName, "", Types.Int, Types.FloatingPoint);
             }
             
             var a = float.Parse(arg1.Val);
             var b = float.Parse(arg2.Val);
           
             return a < b ? Value.Yes() : Value.No();
         }   
     }
    
    internal class More : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 2; }
        }

        public override int MaxArgs
        {
            get { return 2; }
        }
        
        public More(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            var arg1 = Context.Arguments.First().Execute();
            var arg2 = Context.Arguments.Last().Execute();
                 
            if (arg1.Type != Types.Int && 
                arg1.Type != Types.FloatingPoint && 
                arg2.Type != Types.FloatingPoint &&
                arg2.Type != Types.Int)
            {
                throw new WrongType(Context.FunctionName, "", Types.Int, Types.FloatingPoint);
            }

            var a = float.Parse(arg1.Val);
            var b = float.Parse(arg2.Val);
          
            return a > b ? Value.Yes() : Value.No();
        }
    }


    internal class Not : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return 1; }
        }
        
        public Not(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            return Context.Arguments.First().Execute().Type == Types.Falsy ? Value.Yes() : Value.No();
        }
    }
    
    internal class And : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 2; }
        }

        public override int MaxArgs
        {
            get { return 2; }
        }
        
        public And(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            var a = Context.Arguments.First().Execute().Type != Types.Falsy;
            var b = Context.Arguments.Last().Execute().Type != Types.Falsy;
            
            if (a && b)
            {
                return Value.Yes();
            }
            return Value.No();
        }
    }
    
    internal class Or : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 2; }
        }

        public override int MaxArgs
        {
            get { return 2; }
        }
        
        public Or(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            var a = Context.Arguments.First().Execute().Type != Types.Falsy;
            var b = Context.Arguments.Last().Execute().Type != Types.Falsy;
            
            if (a || b)
            {
                return Value.Yes();
            }
            return Value.No();
        }
    }

    internal class When : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 2; }
        }

        public override int MaxArgs
        {
            get { return 2; }
        }
        
        public When(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            return Context.Arguments.First().Execute().Type != Types.Falsy 
                ? Context.Arguments.Last().Execute() 
                : Value.No();
        }
    }
    
    internal class If : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 3; }
        }

        public override int MaxArgs
        {
            get { return 3; }
        }
        
        public If(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            return Context.Arguments.First().Execute().Type != Types.Falsy 
                ? Context.Arguments[1].Execute() 
                : Context.Arguments.Last().Execute();
        }
    }

    internal class Let : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 3; }
        }

        public override int MaxArgs
        {
            get { return 3; }
        }
        
        public Let(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            if (Context.Arguments.First().Execute().Type != Types.Name)
                throw new WrongType(Context.FunctionName, "first argument should be a Name", Types.Name);

            Context
                .AddValueToLocalContext(
                    Context.Arguments.First().Execute().Val, 
                    Context.Arguments[1].Execute());
            
            return Context.Arguments.Last().Execute();
        }
    }
    
    internal class Set : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 2; }
        }

        public override int MaxArgs
        {
            get { return 2; }
        }
        
        public Set(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            if (Context.Arguments.First().Execute().Type != Types.Name)
                throw new WrongType(Context.FunctionName, "first argument should be a Name", Types.Name);

            var value = Context.Arguments[1].Execute();
            Context
                .AddValueToGlobalContext(
                    Context.Arguments.First().Execute().Val, value);
            
            return value;
        }
    }
    

    internal class Retrieve : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return 1; }
        }
        
        public Retrieve(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            var value = Context.Arguments.First().Execute();
            if (value.Type != Types.Name)
                throw new WrongType(Context.FunctionName, " argument should be a Name", Types.Name);

            return Context.RetrieveValueFromContext(value.Val);
        }
    }

    internal class DefineFunction : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 2; }
        }

        public override int MaxArgs
        {
            get { return Int32.MaxValue; }
        }
        
        public DefineFunction(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            if (Context.Arguments.Count < 2)
                throw new WrongArgumentCount(Context.FunctionName, 2);

            var funcName = Context.Arguments.First().Execute();
            if (funcName.Type != Types.Name)
                throw new WrongType(Context.FunctionName, "first argument should be a Name", Types.Name);
            
            //let's remove name + body from the context
            var codeBody = Context.Arguments.Last();
            Context.Arguments.RemoveAt(0);
            Context.Arguments.Remove(codeBody);

            var codeBodyContext = Context;
            Context.AddFunctionToLocalContext(
                funcName.Val, (context) => new CustomFunc(context, funcName.Val, codeBody, codeBodyContext));
            
            return new Value(funcName.Val, Types.Name);
        }
    }

    internal class CustomFunc : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 0; }
        }

        public override int MaxArgs
        {
            get { return Int32.MaxValue; }
        }
        
        private readonly LazyValue _codeBody;
        private readonly string _functionName;
        public  ExprContext FuncDeclContext { get; private set; }

        public CustomFunc(ExprContext context, string funcName, LazyValue codeBody, ExprContext funcDeclContext) : base(context)
        {
            _codeBody = codeBody;
            FuncDeclContext = funcDeclContext;
            _functionName = funcName;  
        }
        
        protected override Value ExecuteImpl()
        {
            var funcArgs = FuncDeclContext.Arguments;
            if (funcArgs.Count != Context.Arguments.Count)
                throw new WrongArgumentCount(_functionName, Context.Arguments.Count, Context.Arguments.Count);
  
            for (var i = 0; i < Context.Arguments.Count; i++)
            {
                var name = FuncDeclContext.Arguments[i].Execute().Val;
                var val = Context.Arguments[i].Execute();
                FuncDeclContext.AddValueToLocalContext(name, val);
            }

            return _codeBody.Execute();
        }
    }
    
    internal class Random : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 2; }
        }

        public override int MaxArgs
        {
            get { return 2; }
        }
        
        public Random(ExprContext context) : base(context)
        {
        }
        
        protected override Value ExecuteImpl()
        {
            var arg1 = Context.Arguments.First().Execute();
            var arg2 = Context.Arguments.Last().Execute();
                 
            if (arg1.Type != Types.Int && 
                arg2.Type != Types.Int)
            {
                throw new WrongType(Context.FunctionName, "", Types.Int);
            }

            var random = new System.Random();
            var number = random.Next(int.Parse(arg1.Val), int.Parse(arg2.Val));

            return new Value(number.ToString(), Types.Int);
        }
    }
    
    internal class Apply : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return Int32.MaxValue; }
        }
        
        public Apply(ExprContext context) : base(context)
        {
        }
        
        protected override Value ExecuteImpl()
        {
            if (Context.Arguments.Count < 1)
                throw new WrongArgumentCount(Context.FunctionName, 1);
            
            var functionName = Context.Arguments.First().Execute().Val;
            var func = Context.RetrieveFunctionFromLocalContext(functionName);
            
            var newContext = new ExprContext(functionName, Context);
            newContext.Arguments.AddRange(Context.Arguments);
            newContext.Arguments.RemoveAt(0);
            return func(newContext).Execute();
        }
    }
    
    
    internal class DefineLambdaFunction : InterpreterFunc
    {
        public override int MinArgs
        {
            get { return 1; }
        }

        public override int MaxArgs
        {
            get { return Int32.MaxValue; }
        }
        
        public DefineLambdaFunction(ExprContext context) : base(context)
        {
        }

        protected override Value ExecuteImpl()
        {
            if (Context.Arguments.Count < 1)
                throw new WrongArgumentCount(Context.FunctionName, 1);
            
            var codeBody = Context.Arguments.Last();
            Context.Arguments.Remove(codeBody);

            var codeBodyContext = Context;
            var funcName = Guid.NewGuid().ToString();
            Context.AddFunctionToLocalContext(funcName, (context) => new CustomFunc(context, funcName + " (lambda)", codeBody, codeBodyContext));
            
            return new Value(funcName, Types.Name);
        }
    }
}