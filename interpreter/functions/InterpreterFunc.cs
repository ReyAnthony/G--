namespace GMinusMinus.interpreter.functions
{
    public abstract class InterpreterFunc
    {

        protected readonly ExprContext Context;
        public abstract Value Execute();

        protected InterpreterFunc(ExprContext context)
        {
            Context = context;
        }
    }
}