using SILF.Script.Interfaces;

namespace LIN.Emma.UI;

public class Functions
{
    /// <summary>
    /// Funciones.
    /// </summary>
    public static List<IFunction> Actions { get; set; } = new();

    /// <summary>
    /// Funciones.
    /// </summary>
    public static List<Delegate> Delegates { get; set; } = new();



    public static void LoadActions(IFunction function)
    {
        Actions.Add(function);
    }

    public static void LoadActions(IEnumerable<IFunction> functions)
    {
        Actions.AddRange(functions);
    }

    public static void LoadActions(IEnumerable<Delegate> functions)
    {
        Delegates.AddRange(functions);
    }

}