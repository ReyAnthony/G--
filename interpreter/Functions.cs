using System;
using System.Globalization;
using System.Linq;

namespace Interpreter1
{
    internal class Add : InterpreterFunc
    {
        public override Value Execute()
        {
            float added = 0;
            Types returnType = Types.Int;
            Arguments.ForEach(lazy =>
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
        public override Value Execute()
        {
            float subbed = 0;
            Types returnType = Types.Int;
            for (var i = 0; i < Arguments.Count; i++)
            {
                var a = Arguments[i].Execute();
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
        public override Value Execute()
        {
            float divd = 0;
            Types returnType = Types.FloatingPoint;
            for (var i = 0; i < Arguments.Count; i++)
            {
                var a = Arguments[i].Execute();
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
        public override Value Execute()
        {
            float mult = 0;
            Types returnType = Types.FloatingPoint;
            for (var i = 0; i < Arguments.Count; i++)
            {
                var a = Arguments[i].Execute();
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
        public override Value Execute()
        {
            if (Arguments.Count == 0)
                throw new WrongArgumentCount("$$", 1);

            Arguments.GetRange(0, Arguments.Count - 1).ForEach((a) => a.Execute());
            return Arguments.Last().Execute();
        }
    }

    internal class Print : InterpreterFunc
    {
        public override Value Execute()
        {
            if (Arguments.Count != 1)
                throw new WrongArgumentCount("print", 1, 1);

            foreach (var lazy in Arguments)
            {
                var argument = lazy.Execute();
                if (argument.Type != Types.String && argument.Type != Types.Int && argument.Type != Types.FloatingPoint)
                {
                    throw new WrongType("print", "", Types.String, Types.Int, Types.FloatingPoint);
                }
            }

            var arg1 = Arguments.First().Execute();

            Console.WriteLine(arg1.Val.Replace("\"", string.Empty));
            return arg1;
        }
    }

    internal class Read : InterpreterFunc
    {
        public override Value Execute()
        {
            if (Arguments.Count != 0)
                throw new WrongArgumentCount("read", 0, 0);

            Console.ReadLine();
            var read = Console.ReadLine();
            return new Value(read, Types.String);
        }
    }

    internal class TypeOf : InterpreterFunc
    {
        public override Value Execute()
        {
            if (Arguments.Count != 1)
                throw new WrongArgumentCount("typeof", 1, 1);

            return new Value(Arguments.First().Execute().Type.ToString(), Types.String);
        }
    }

    internal class Eq : InterpreterFunc
    {
        public override Value Execute()
        {
            if (Arguments.Count != 2)
                throw new WrongArgumentCount("eq", 2, 2);

            if (Arguments.First().Execute().Val.Equals(Arguments.Last().Execute().Val))
            {
                return new Value("t", Types.Name);
            }
            return new Value("()", Types.EmptyList);
        }
    }

    internal class Not : InterpreterFunc
    {
        public override Value Execute()
        {
            if (Arguments.Count != 1)
                throw new WrongArgumentCount("not", 1, 1);

            if (Arguments.First().Execute().Type == Types.EmptyList)
            {
                return new Value("t", Types.Name);
            }
            return new Value("()", Types.EmptyList);
        }
    }

    internal class When : InterpreterFunc
    {
        public override Value Execute()
        {
            if (Arguments.Count != 2)
                throw new WrongArgumentCount("when", 2, 2);

            if (Arguments.First().Execute().Type != Types.EmptyList)
            {
                return Arguments.Last().Execute();
            }
            else
            {
                return new Value("()", Types.EmptyList);
            }
        }
    }
    
    internal class If : InterpreterFunc
    {
        public override Value Execute()
        {
            if (Arguments.Count != 3)
                throw new WrongArgumentCount("if", 3, 3);

            if (Arguments.First().Execute().Type != Types.EmptyList)
            {
                return Arguments[1].Execute();
            }
            return Arguments.Last().Execute();
        }
    }

    internal class Def : InterpreterFunc
    {
        public override Value Execute()
        {
            if (Arguments.Count != 3)
                throw new WrongArgumentCount("def", 3, 3);

            if (Arguments.First().Execute().Type != Types.Name)
                throw new WrongType("def", "first argument should be a Name", Types.Name);

            AddValueToLocalContext(Arguments.First().Execute().Val, Arguments[1].Execute());
            return Arguments.Last().Execute();
        }
    }

    internal class Retrieve : InterpreterFunc
    {
        public override Value Execute()
        {
            if (Arguments.Count != 1)
                throw new WrongArgumentCount("ret", 1, 1);

            if (Arguments.First().Execute().Type != Types.Name)
                throw new WrongType("ret", "first argument should be a Name", Types.Name);

            return RetrieveValueFromContext(Arguments.First().Execute().Val);
        }
    }
}