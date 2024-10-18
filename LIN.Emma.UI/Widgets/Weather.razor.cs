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
        return Model == null
            ? ""
            : Model.Condition switch
            {
                Condition.ThunderStorm => "_content/LIN.Emma.UI/EmmaAssets/weather/storm.png",
                Condition.Clear => "_content/LIN.Emma.UI/EmmaAssets/weather/clear.png",
                Condition.ScatteredClouds => "_content/LIN.Emma.UI/EmmaAssets/weather/cloud.png",
                Condition.FewClouds => "_content/LIN.Emma.UI/EmmaAssets/weather/fewClouds.png",
                Condition.Haze => "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png",
                Condition.Mist => "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png",
                Condition.Smoke => "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png",
                Condition.Dust => "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png",
                Condition.Fog => "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png",
                Condition.Sand => "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png",
                Condition.Ash => "_content/LIN.Emma.UI/EmmaAssets/weather/haze.png",
                Condition.OvercastClouds => "_content/LIN.Emma.UI/EmmaAssets/weather/Overcast.png",
                Condition.Rain => "_content/LIN.Emma.UI/EmmaAssets/weather/rain.png",
                Condition.Snow => "_content/LIN.Emma.UI/EmmaAssets/weather/snow.png",
                Condition.BrokenClouds => "_content/LIN.Emma.UI/EmmaAssets/weather/brokenClouds.png",
                Condition.Tornado => "_content/LIN.Emma.UI/EmmaAssets/weather/tornado.png",
                _ => "",
            };
    }


    /// <summary>
    /// Obtener texto.
    /// </summary>
    public string Text()
    {
        return Model == null
            ? ""
            : Model.Condition switch
            {
                Condition.ThunderStorm => "Tormenta",
                Condition.Clear => "Despejado",
                Condition.ScatteredClouds => "Nubes dispersas",
                Condition.FewClouds => "Pocas nubes",
                Condition.Haze => "Niebla",
                Condition.Mist => "Niebla",
                Condition.Smoke => "Humo",
                Condition.Dust => "Polvo",
                Condition.Fog => "Niebla",
                Condition.Sand => "Arena",
                Condition.Ash => "Ceniza",
                Condition.OvercastClouds => "Nubes cubiertas",
                Condition.Rain => "Lluvia",
                Condition.Snow => "Nevando",
                Condition.BrokenClouds => "Lloviendo",
                Condition.Tornado => "Tornado",
                _ => "Desconocido",
            };
    }

}