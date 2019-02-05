using System;

namespace GMinusMinus.interpreter
{
    public class LazyValue
    {
        private readonly Func<Value> _lazy;

        public LazyValue(Func<Value> val)
        {
            _lazy = val;
        }

        public Value Execute()
        {
            return _lazy();
        }
    }
}