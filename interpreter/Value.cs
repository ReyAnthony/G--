namespace GMinusMinus.interpreter
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
        
        public string Val { get; private set; }
        public Types Type { get; private set; }

        public Value(string value, Types type)
        {
            Val = value;
            Type = type;
        }
    }
}