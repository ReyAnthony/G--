using System;
using System.Globalization;
using System.Linq;

namespace Interpreter1
{
    internal class Add : InterpreterFunc
    {
        public Add(ExprContext context) : base(context)
        {}
        
        public override Value Execute()
        {
            float added = 0;
            Types returnType = Types.Int;
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
        public Sub(ExprContext context) : base(context)
        {
        }
        
        public override Value Execute()
        {
            float subbed = 0;
            Types returnType = Types.Int;
            for (var i = 0; i < Context.Arguments.Count; i++)
            {
                var a = Context.Arguments[i].Execute();
                if (a.Type == Types.FloatingPoint)
                {
                    var val = float.Parse(a.Val.ToString(), CultureInfo.InvariantCulture);
                    if (i == 0)
                        subbed = val;
                    else
                        subbed -= val;
                    returnType = Types.FloatingPoint;
                }
                else
                {
                    var val = int.Parse(a.Val.ToString());
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
        public Div(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            float divd = 0;
            Types returnType = Types.FloatingPoint;
            for (var i = 0; i < Context.Arguments.Count; i++)
            {
                var a = Context.Arguments[i].Execute();
                if (a.Type == Types.FloatingPoint)
                {
                    var val = float.Parse(a.Val.ToString(), CultureInfo.InvariantCulture);
                    if (i == 0)
                        divd = val;
                    else
                        divd /= val;
                }
                else
                {
                    var val = int.Parse(a.Val.ToString());
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
        public Mod(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            float divd = 0;
            Types returnType = Types.FloatingPoint;
            for (var i = 0; i < Context.Arguments.Count; i++)
            {
                var a = Context.Arguments[i].Execute();
                if (a.Type == Types.FloatingPoint)
                {
                    var val = float.Parse(a.Val.ToString(), CultureInfo.InvariantCulture);
                    if (i == 0)
                        divd = val;
                    else
                        divd %= val;
                }
                else
                {
                    var val = int.Parse(a.Val.ToString());
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
        public Mult(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            float mult = 0;
            Types returnType = Types.FloatingPoint;
            for (var i = 0; i < Context.Arguments.Count; i++)
            {
                var a = Context.Arguments[i].Execute();
                if (a.Type == Types.FloatingPoint)
                {
                    var val = float.Parse(a.Val.ToString(), CultureInfo.InvariantCulture);
                    if (i == 0)
                        mult = val;
                    else
                        mult *= val;
                }
                else
                {
                    var val = int.Parse(a.Val.ToString());
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
        public StatementsGroup(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count == 0)
                throw new WrongArgumentCount(Context.FunctionName, 1);

            Context.Arguments.GetRange(0, Context.Arguments.Count - 1).ForEach((a) => a.Execute());
            return Context.Arguments.Last().Execute();
        }
    }

    internal class Print : InterpreterFunc
    {
        public Print(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count < 1)
                throw new WrongArgumentCount("ExprContext.FunctionName", 1);

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
                    argument.Type != Types.Int && 
                    argument.Type != Types.FloatingPoint)
                {
                    throw new WrongType(Context.FunctionName, "", Types.String, Types.Int, Types.FloatingPoint);
                }
                fullStr += argument.Val;
                last = argument;
            }

            Console.WriteLine(fullStr.Replace("\"", string.Empty));
            return last;
        }
    }

    internal class Read : InterpreterFunc
    {
        public Read(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 0)
                throw new WrongArgumentCount(Context.FunctionName, 0, 0);

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
        public TypeOf(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 1)
                throw new WrongArgumentCount(Context.FunctionName, 1, 1);

            return new Value(Context.Arguments.First().Execute().Type.ToString(), Types.String);
        }
    }

    internal class Eq : InterpreterFunc
    {
        public Eq(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 2)
                throw new WrongArgumentCount(Context.FunctionName, 2, 2);

            if (Context.Arguments.First().Execute().Val.Equals(Context.Arguments.Last().Execute().Val))
            {
                return Value.Yes();
            }
            return Value.No();
        }
    }
    
    internal class Less : InterpreterFunc
     {
             public Less(ExprContext context) : base(context)
             {
             }
     
             public override Value Execute()
             {
                 if (Context.Arguments.Count != 2)
                     throw new WrongArgumentCount(Context.FunctionName, 2, 2);
     
                 var arg1 = Context.Arguments.First().Execute();
                 var arg2 = Context.Arguments.Last().Execute();
                 
                 if (arg1.Type != Types.Int && 
                     arg1.Type != Types.FloatingPoint && 
                     arg2.Type != Types.FloatingPoint &&
                     arg2.Type != Types.Int)
                 {
                     throw new WrongType(Context.FunctionName, "", Types.Int, Types.FloatingPoint);
                 }
     
                 float a = 0;
                 float b = 0;
                 a = float.Parse(arg1.Val);
                 b = float.Parse(arg2.Val);
               
                 if (a < b)
                 {
                     return Value.Yes();
                 }
                 return Value.No();
             }
             
     }
    
    internal class More : InterpreterFunc
    {
        public More(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 2)
                throw new WrongArgumentCount(Context.FunctionName, 2, 2);
            
            var arg1 = Context.Arguments.First().Execute();
            var arg2 = Context.Arguments.Last().Execute();
                 
            if (arg1.Type != Types.Int && 
                arg1.Type != Types.FloatingPoint && 
                arg2.Type != Types.FloatingPoint &&
                arg2.Type != Types.Int)
            {
                throw new WrongType(Context.FunctionName, "", Types.Int, Types.FloatingPoint);
            }

            float a = 0;
            float b = 0;
            a = float.Parse(arg1.Val);
            b = float.Parse(arg2.Val);
          
            if (a > b)
            {
                return Value.Yes();
            }
            return Value.No();
        }
    }


    internal class Not : InterpreterFunc
    {
        public Not(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 1)
                throw new WrongArgumentCount(Context.FunctionName, 1, 1);

            if (Context.Arguments.First().Execute().Type == Types.Falsy)
            {
                return Value.Yes();
            }
            return Value.No();
        }
    }
    
    internal class And : InterpreterFunc
    {
        public And(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 2)
                throw new WrongArgumentCount(Context.FunctionName, 2, 2);

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
        public Or(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 2)
                throw new WrongArgumentCount(Context.FunctionName, 2, 2);

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
        public When(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 2)
                throw new WrongArgumentCount(Context.FunctionName, 2, 2);

            if (Context.Arguments.First().Execute().Type != Types.Falsy)
            {
                return Context.Arguments.Last().Execute();
            }
            else
            {
                return Value.No();
            }
        }
    }
    
    internal class If : InterpreterFunc
    {
        public If(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 3)
                throw new WrongArgumentCount("if", 3, 3);

            if (Context.Arguments.First().Execute().Type != Types.Falsy)
            {
                return Context.Arguments[1].Execute();
            }
            return Context.Arguments.Last().Execute();
        }
    }

    internal class Let : InterpreterFunc
    {
        public Let(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 3)
                throw new WrongArgumentCount(Context.FunctionName, 3, 3);

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
        public Set(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 2)
                throw new WrongArgumentCount(Context.FunctionName, 2, 2);

            if (Context.Arguments.First().Execute().Type != Types.Name)
                throw new WrongType(Context.FunctionName, "first argument should be a Name", Types.Name);

            Context
                .AddValueToGlobalContext(
                    Context.Arguments.First().Execute().Val, 
                    Context.Arguments[1].Execute());
            
            return Value.Yes();
        }
    }
    

    internal class Retrieve : InterpreterFunc
    {
        public Retrieve(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
        {
            if (Context.Arguments.Count != 1)
                throw new WrongArgumentCount(Context.FunctionName, 1, 1);

            var value = Context.Arguments.First().Execute();
            if (value.Type != Types.Name)
                throw new WrongType(Context.FunctionName, " argument should be a Name", Types.Name);

            return Context.RetrieveValueFromContext(value.Val);
        }
    }

    internal class DefineFunction : InterpreterFunc
    {
        public DefineFunction(ExprContext context) : base(context)
        {
        }

        public override Value Execute()
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
                funcName.Val, 
                (context) =>
                {
                    return new CustomFunc(context, funcName.Val, codeBody, codeBodyContext);
                });
            
            return Value.Yes();
        }
    }

    internal class CustomFunc : InterpreterFunc
    {
        private readonly LazyValue _codeBody;
        private readonly string _functionName;
        public  ExprContext FuncDeclContext { get; }

        public CustomFunc(ExprContext context, string funcName, LazyValue codeBody, ExprContext funcDeclContext) : base(context)
        {
            _codeBody = codeBody;
            FuncDeclContext = funcDeclContext;
            _functionName = funcName;  
        }
        
        public override Value Execute()
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
        public Random(ExprContext context) : base(context)
        {
        }
        
        public override Value Execute()
        {
            if (Context.Arguments.Count != 2)
                throw new WrongArgumentCount(Context.FunctionName, 2, 2);
            
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
        public Apply(ExprContext context) : base(context)
        {
        }
        
        public override Value Execute()
        {
            if (Context.Arguments.Count < 1)
                throw new WrongArgumentCount(Context.FunctionName, 1);
            
            var functionName = Context.Arguments.First().Execute().Val;
            var func = Context.RetrieveFunctionFromLocalContext(functionName);
            return func(new ExprContext(functionName, Context)).Execute();
        }
    }

}