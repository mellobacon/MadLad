using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace MadLad.Compiler.CodeAnalysis.Syntax.Symbols
{
    public class BuiltinFunctions
    {
        public static readonly FunctionSymbol PrintString = new ("print",
            ImmutableArray.Create(new ParameterSymbol("text", TypeSymbol.String)), TypeSymbol.Void);

        public static readonly FunctionSymbol PrintInt = new("printnumber",
            ImmutableArray.Create(new ParameterSymbol("int", TypeSymbol.Int)), TypeSymbol.Void);
        
        internal static IEnumerable<FunctionSymbol> GetAll() => 
            typeof(BuiltinFunctions).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(FunctionSymbol)).Select(f => (FunctionSymbol)f.GetValue(null));
    }
}