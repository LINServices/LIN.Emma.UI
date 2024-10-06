using LIN.Types.Exp.Search.Enums;

namespace LIN.Emma.UI.Widgets;

public partial class Weather
{

    [Parameter]
    public LIN.Types.Exp.Search.Models.Weather? Model { get; set; }


    /// <summary>
    /// Obtener img.
    /// </summary>
    public string Img()
    {
        if (Model == null)
            return ("");


        switch (Model.Condition)
        {
            case Condition.ThunderStorm:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/storm.png";
            case Condition.Clear:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/clear.png";
            case Condition.ScatteredClouds:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/cloud.png";
            case Condition.FewClouds:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/fewClouds.png";
            case Condition.Haze:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png";
            case Condition.Mist:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png";
            case Condition.Smoke:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png";
            case Condition.Dust:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png";
            case Condition.Fog:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png";
            case Condition.Sand:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png";
            case Condition.Ash:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png";
            case Condition.OvercastClouds:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/Overcast.png";
            case Condition.Rain:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/rain.png";
            case Condition.Snow:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/snow.png";
            case Condition.BrokenClouds:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/brokenClouds.png";
            case Condition.Tornado:
                return "_content/LIN.Emma.UI/EmmaAssets/weather/tornado.png";


        }

        return "";

    }


    /// <summary>
    /// Obtener texto.
    /// </summary>
    public string Text()
    {
        if (Model == null)
            return ("");


        switch (Model.Condition)
        {
            case Condition.ThunderStorm:
                return "Tormenta";
            case Condition.Clear:
                return "Despejado";
            case Condition.ScatteredClouds:
                return "Nubes dispersas";
            case Condition.FewClouds:
                return "Pocas nubes";
            case Condition.Haze:
                return "Niebla";
            case Condition.Mist:
                return "Niebla";
            case Condition.Smoke:
                return "Humo";
            case Condition.Dust:
                return "Polvo";
            case Condition.Fog:
                return "Niebla";
            case Condition.Sand:
                return "Arena";
            case Condition.Ash:
                return "Ceniza";
            case Condition.OvercastClouds:
                return "Nubes cubiertas";
            case Condition.Rain:
                return "Lluvia";
            case Condition.Snow:
                return "Nevando";
            case Condition.BrokenClouds:
                return "Lloviendo";
            case Condition.Tornado:
                return "Tornado";
        }

        return "Desconocido";

    }

}