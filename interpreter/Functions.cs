using System;
using System.Globalization;
using System.Linq;

namespace Interpreter1
{
    internal class Add : InterpreterFunc
    {
        public Add(ExprContext exprContext) : base(exprContext)
        {}
        
        public override Value Execute()
        {
            float added = 0;
            Types returnType = Types.Int;
            ExprContext.Arguments.ForEach(lazy =>
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
        public Sub(ExprContext exprContext) : base(exprContext)
        {
        }
        
        public override Value Execute()
        {
            float subbed = 0;
            Types returnType = Types.Int;
            for (var i = 0; i < ExprContext.Arguments.Count; i++)
            {
                var a = ExprContext.Arguments[i].Execute();
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
        public Div(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            float divd = 0;
            Types returnType = Types.FloatingPoint;
            for (var i = 0; i < ExprContext.Arguments.Count; i++)
            {
                var a = ExprContext.Arguments[i].Execute();
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

    internal class Mult : InterpreterFunc
    {
        public Mult(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            float mult = 0;
            Types returnType = Types.FloatingPoint;
            for (var i = 0; i < ExprContext.Arguments.Count; i++)
            {
                var a = ExprContext.Arguments[i].Execute();
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
        public StatementsGroup(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            if (ExprContext.Arguments.Count == 0)
                throw new WrongArgumentCount("$$", 1);

            ExprContext.Arguments.GetRange(0, ExprContext.Arguments.Count - 1).ForEach((a) => a.Execute());
            return ExprContext.Arguments.Last().Execute();
        }
    }

    internal class Print : InterpreterFunc
    {
        public Print(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            if (ExprContext.Arguments.Count < 1)
                throw new WrongArgumentCount("print", 1);

            var fullStr = "";
            Value last = null;
            foreach (var lazy in ExprContext.Arguments)
            {
                var argument = lazy.Execute();
                if (argument.Type != Types.String && argument.Type != Types.Int && argument.Type != Types.FloatingPoint)
                {
                    throw new WrongType("print", "", Types.String, Types.Int, Types.FloatingPoint);
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
        public Read(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            if (ExprContext.Arguments.Count != 0)
                throw new WrongArgumentCount("read", 0, 0);

            Console.ReadLine();
            var read = Console.ReadLine();
            return new Value(read, Types.String);
        }
    }

    internal class TypeOf : InterpreterFunc
    {
        public TypeOf(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            if (ExprContext.Arguments.Count != 1)
                throw new WrongArgumentCount("typeof", 1, 1);

            return new Value(ExprContext.Arguments.First().Execute().Type.ToString(), Types.String);
        }
    }

    internal class Eq : InterpreterFunc
    {
        public Eq(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            if (ExprContext.Arguments.Count != 2)
                throw new WrongArgumentCount("eq", 2, 2);

            if (ExprContext.Arguments.First().Execute().Val.Equals(ExprContext.Arguments.Last().Execute().Val))
            {
                return new Value("t", Types.Name);
            }
            return new Value("()", Types.EmptyList);
        }
    }

    internal class Not : InterpreterFunc
    {
        public Not(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            if (ExprContext.Arguments.Count != 1)
                throw new WrongArgumentCount("not", 1, 1);

            if (ExprContext.Arguments.First().Execute().Type == Types.EmptyList)
            {
                return new Value("t", Types.Name);
            }
            return new Value("()", Types.EmptyList);
        }
    }

    internal class When : InterpreterFunc
    {
        public When(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            if (ExprContext.Arguments.Count != 2)
                throw new WrongArgumentCount("when", 2, 2);

            if (ExprContext.Arguments.First().Execute().Type != Types.EmptyList)
            {
                return ExprContext.Arguments.Last().Execute();
            }
            else
            {
                return new Value("()", Types.EmptyList);
            }
        }
    }
    
    internal class If : InterpreterFunc
    {
        public If(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            if (ExprContext.Arguments.Count != 3)
                throw new WrongArgumentCount("if", 3, 3);

            if (ExprContext.Arguments.First().Execute().Type != Types.EmptyList)
            {
                return ExprContext.Arguments[1].Execute();
            }
            return ExprContext.Arguments.Last().Execute();
        }
    }

    internal class Def : InterpreterFunc
    {
        public Def(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            if (ExprContext.Arguments.Count != 3)
                throw new WrongArgumentCount("def", 3, 3);

            if (ExprContext.Arguments.First().Execute().Type != Types.Name)
                throw new WrongType("def", "first argument should be a Name", Types.Name);

            ExprContext
                .AddValueToLocalContext(
                    ExprContext.Arguments.First().Execute().Val, 
                    ExprContext.Arguments[1].Execute());
            
            return ExprContext.Arguments.Last().Execute();
        }
    }

    internal class Retrieve : InterpreterFunc
    {
        public Retrieve(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            if (ExprContext.Arguments.Count != 1)
                throw new WrongArgumentCount("ret", 1, 1);

            if (ExprContext.Arguments.First().Execute().Type != Types.Name)
                throw new WrongType("ret", "first argument should be a Name", Types.Name);

            return ExprContext.RetrieveValueFromContext(ExprContext.Arguments.First().Execute().Val);
        }
    }

    //Function with no argument
    internal class DefineFunction : InterpreterFunc
    {
        public DefineFunction(ExprContext exprContext) : base(exprContext)
        {
        }

        public override Value Execute()
        {
            if (ExprContext.Arguments.Count != 2)
                throw new WrongArgumentCount("functon", 2);

            var funcName = ExprContext.Arguments.First().Execute();
            if (funcName.Type != Types.Name)
                throw new WrongType("function", "first argument should be a Name", Types.Name);
            
            ExprContext.AddFunctionToLocalContext(
                funcName.Val, 
                () => new CustomFunc(new ExprContext(funcName.Val, ExprContext), ExprContext.Arguments.Last(), funcName.Val));
            
            return new Value("t", Types.Name);
        }
    }

    internal class CustomFunc : InterpreterFunc
    {
        private readonly LazyValue _codeBody;
        private readonly string[] _names;
        private readonly string _functionName;

        public CustomFunc(ExprContext exprContext, LazyValue codeBody, string functionName, params string[] names) 
            : base(exprContext)
        {
            _codeBody = codeBody;
            _names = names;
            _functionName = functionName;
        }
        
        public override Value Execute()
        {
            if (ExprContext.Arguments.Count != _names.Length)
                throw new WrongArgumentCount(_functionName, _names.Length);
            
            for (var i = 0; i < ExprContext.Arguments.Count; i++)
            {
                var lazy = ExprContext.Arguments[i];
                var val = lazy.Execute();
                ExprContext.AddValueToLocalContext(_names[i], val);
            }

            return _codeBody.Execute();
        }
    }
}