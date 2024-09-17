using SILF.Script;
using SILF.Script.Elements;
using SILF.Script.Elements.Functions;
using SILF.Script.Interfaces;
using SILF.Script.Runtime;

namespace LIN.Emma.UI.Classes;

public class SILFFunction : IFunction
{

    public Tipo? Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Parameter> Parameters { get; set; } = new();

    Context IFunction.Context { get; set; }
    Action<List<SILF.Script.Elements.ParameterValue>> Action;

    public SILFFunction(Action<List<SILF.Script.Elements.ParameterValue>> action)
    {
        Action = action;
    }

    public FuncContext Run(Instance instance, List<SILF.Script.Elements.ParameterValue> values, ObjectContext @object)
    {
        Action.Invoke(values);
        return new();
    }

}
