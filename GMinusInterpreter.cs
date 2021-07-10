using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using GMinusMinus.interpreter;
using GMinusMinus.interpreter.functions;
using Random = GMinusMinus.interpreter.functions.Random;

namespace GMinusMinus
{
    public class GMinusInterpreter
    {
        private readonly Dictionary<string, Func<ExprContext, InterpreterFunc>> _functions;
        private IAntlrErrorListener<IToken> _customErrorListener;

        public GMinusInterpreter(Dictionary<string, Func<ExprContext, InterpreterFunc>> functions)
        {
            _functions = new Dictionary<string, Func<ExprContext, InterpreterFunc>>
            {
                {"%%", e => new StatementsGroup(e)},
                {"+", e => new Add(e)},
                {"-", e => new Sub(e)},
                {"/", e => new Div(e)},
                {"%", e => new Mod(e)},
                {"*", e => new Mult(e)},
                {"<", e => new Less(e)},
                {">", e => new More(e)},
                {"==", e => new Eq(e)},
                {"and", e => new And(e)},
                {"or", e => new Or(e)},
                {"typeof", e => new TypeOf(e)},
                {"not", e => new Not(e)},
                {"when", e => new When(e)},
                {"if", e => new If(e)},
                {"let", e => new Let(e)},
                {"set", e => new Set(e)},
                {"ret", e => new Retrieve(e)},
                {"print", e => new Print(e)},
                {"read", e => new Read(e)},
                {"random", e => new Random(e)},  
                {"function", e => new DefineFunction(e)},
                {"apply", e => new Apply(e)}
            };

            if (functions != null)
            {
                var merged = _functions.Concat(functions)
                                       .ToLookup(x => x.Key, x => x.Value)
                                       .ToDictionary(x => x.Key, g => g.First());
                _functions = merged;
            }
        }

        public GMinusInterpreter() : this(null) {}

        public void SetCustomErrorListener(IAntlrErrorListener<IToken> listener)
        {
            _customErrorListener = listener;
        }

        public void Eval(string expr)
        {
            var stream = CharStreams.fromstring(expr);
            var lexer = new ExprLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ExprParser(tokens) {BuildParseTree = true};
            
            if (_customErrorListener != null)
            {
                parser.RemoveErrorListeners();
                parser.AddErrorListener(_customErrorListener);
            }
            
            IParseTree tree = parser.prog();
            ParseTreeWalker.Default.Walk(new Listener(_functions), tree);  
        }
    }
}