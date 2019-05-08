namespace GMinusMinus.interpreter.functions
{
    public abstract class InterpreterFunc
    {
        protected readonly ExprContext Context;
        public abstract int MinArgs { get; }
        public abstract int MaxArgs { get; }

        public Value Execute()
        {
            if (Context.Arguments.Count < MinArgs || Context.Arguments.Count > MaxArgs)
                throw new WrongArgumentCount(Context.FunctionName, MinArgs, MaxArgs);
            
            return ExecuteImpl();
        }

        protected abstract Value ExecuteImpl();

        protected InterpreterFunc(ExprContext context)
        {
            Context = context;
        }
    }
}