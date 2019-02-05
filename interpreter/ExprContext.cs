using System;
using System.Collections.Generic;
using GMinusMinus.interpreter.functions;

namespace GMinusMinus.interpreter
{
    public class ExprContext
    {
        public string FunctionName { get; private set; }
        public List<LazyValue> Arguments { get; private set; }
        
        private Dictionary<string, Func<ExprContext, InterpreterFunc>> LocalFunctions { get; set; }
        private Dictionary<string, Value> LocalVariables { get; set; }
        private Dictionary<string, Value> GlobalVariables { get; set; }
        private ExprContext Parent { get; set; }
        private Stack<Dictionary<string, Value>> LocalVariablesStashHack { get; set; }

        public ExprContext(string functionName, ExprContext parent)
        {
            this.FunctionName = functionName;
            Parent = parent;
            LocalFunctions = new Dictionary<string, Func<ExprContext, InterpreterFunc>>();
            LocalVariables = new Dictionary<string, Value>();
            GlobalVariables = new Dictionary<string, Value>();
            Arguments = new List<LazyValue>();
            LocalVariablesStashHack = new Stack<Dictionary<string, Value>>();
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
                return Parent == null ? GlobalVariables[name] : Parent.RetrieveValueFromContext(name);
            }   
        }
        
        public void AddValueToLocalContext(string name, Value val)
        {
            //Hack not to break when recursion...
            //but SHOULD NOT BE THIS
            //anyway, will only cause pbs in recursion because you can't 
            //actually call it twice in the same context
            if (LocalVariables.ContainsKey(name))
                LocalVariables.Remove(name);
            LocalVariables.Add(name, val);
        }
        
        public void AddValueToGlobalContext(string name, Value val)
        {
            if (Parent == null)
            {
                if (GlobalVariables.ContainsKey(name))
                    GlobalVariables.Remove(name);
                GlobalVariables.Add(name, val);
            }
            else
            {
                Parent.AddValueToGlobalContext(name, val); 
            }   
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

        public void StashLocalContext(ExprContext funcDeclContext)
        {
            funcDeclContext.LocalVariablesStashHack.Push(new Dictionary<string, Value>());
            foreach (var keyValuePair in funcDeclContext.LocalVariables)
            {
                funcDeclContext.LocalVariablesStashHack.Peek()[keyValuePair.Key] = keyValuePair.Value;
            }
        }

        public void RevertLocalContext(ExprContext funcDeclContext)
        {
            funcDeclContext.LocalVariables.Clear();   
            foreach (var keyValuePair in funcDeclContext.LocalVariablesStashHack.Peek())
            {
                funcDeclContext.LocalVariables[keyValuePair.Key] = keyValuePair.Value;
            }
            funcDeclContext.LocalVariablesStashHack.Pop();
        }
    }
}