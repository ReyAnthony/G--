namespace Interpreter1
{
    internal class Add : InterpreterFunc
    {
        public override Value Execute()
        {
            int added = 0;
            Arguments.ForEach(a =>
            {
                added += int.Parse(a.Val.ToString());
            });

            return new Value(added.ToString(), Types.Int);
        }
    }

    internal class Sub : InterpreterFunc
    {
        public override Value Execute()
        {
            int subbed = 0;
            for (var i = 0; i < Arguments.Count; i++)
            {
                var arg = int.Parse(Arguments[i].Val);
                if (i == 0)
                {
                    subbed = arg;
                }
                else
                {
                    subbed -= arg;
                }
            }

            return new Value(subbed.ToString(), Types.Int);
        }
    }

    internal class Div : InterpreterFunc
    {
        public override Value Execute()
        {
            int divd = 0;
            for (var i = 0; i < Arguments.Count; i++)
            {
                var arg = int.Parse(Arguments[i].Val);
                if (i == 0)
                {
                    divd = arg;
                }
                else
                {
                    divd /= arg;
                }
            }

            return new Value(divd.ToString(), Types.Int);
        }
    }

    internal class Mult : InterpreterFunc
    {
        public override Value Execute()
        {
            int mult = 0;
            for (var i = 0; i < Arguments.Count; i++)
            {
                var arg = int.Parse(Arguments[i].Val);
                if (i == 0)
                {
                    mult = arg;
                }
                else
                {
                    mult *= arg;
                }
            }

            return new Value(mult.ToString(), Types.Int);
        }
    }
}