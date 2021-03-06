using Fuse.CodeAnalysis.Symbols;

namespace Fuse.CodeAnalysis.Binding
{
    internal sealed class BoundScope
    {
        private Dictionary<string, VariableSymbol> _variables = new();
        private Dictionary<string, FunctionSymbol> _functions = new();

        public BoundScope(BoundScope parent)
        {
            Parent = parent;
        }

        public BoundScope Parent { get; }

        public bool TryDeclareVariable(VariableSymbol variable)
        {
            if (_variables == null)
                _variables = new();

            if (_variables.ContainsKey(variable.Name))
                return false;

            _variables.Add(variable.Name, variable);
            return true;
        }

        public bool TryLookupVariable(string name, out VariableSymbol variable)
        {
            variable = null;
            if (_variables != null && _variables.TryGetValue(name, out variable))
                return true;

            if (Parent == null)
                return false;

            return Parent.TryLookupVariable(name, out variable);
        }

        public bool TryDeclareFunction(FunctionSymbol function)
        {
            if (_functions == null)
                _functions = new();

            if (_functions.ContainsKey(function.Name))
                return false;

            _functions.Add(function.Name, function);
            return true;
        }

        public bool TryLookupFunction(string name, out FunctionSymbol function)
        {
            function = null;
            if (_functions != null && _functions.TryGetValue(name, out function))
                return true;

            if (Parent == null)
                return false;

            return Parent.TryLookupFunction(name, out function);
        }

        public ImmutableArray<VariableSymbol> GetDeclaredVariables()
        {
            if (_variables == null)
                return ImmutableArray<VariableSymbol>.Empty;

            return _variables.Values.ToImmutableArray();
        }

        public ImmutableArray<FunctionSymbol> GetDeclaredFunctions()
        {
            if (_functions == null)
                return ImmutableArray<FunctionSymbol>.Empty;

            return _functions.Values.ToImmutableArray();
        }
    }
}
