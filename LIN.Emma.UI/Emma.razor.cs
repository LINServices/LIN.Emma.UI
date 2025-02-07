using LIN.Emma.UI.Classes;
using LIN.Types.Exp.Search.Models;
using LIN.Types.Responses;
using Microsoft.JSInterop;
using SILF.Script.Elements.Functions;

namespace LIN.Emma.UI;


public partial class Emma
{


    /// <summary>
    /// Recibe un intento (Admin).
    /// </summary>
    public event EventHandler<string>? OnPromptRequire;

    private string Raw = "";

    private ReadAllResponse<SearchResult> SearchModels { get; set; } = new();


    public ReadOneResponse<Weather> Modelo { get; set; }




    /// <summary>
    /// Actual estado.
    /// </summary>
    private State ActualState { get; set; } = State.Witting;

    private HeaderState HeaderActualState { get; set; } = HeaderState.Titles;

    /// <summary>
    /// Lista de estados.
    /// </summary>
    private enum State
    {
        Witting,
        Responding
    }


    /// <summary>
    /// Lista de estados.
    /// </summary>
    private enum HeaderState
    {
        Titles,
        Weather,
        Search
    }



    /// <summary>
    /// Entrada del usuario.
    /// </summary>
    private string Prompt { get; set; } = string.Empty;



    /// <summary>
    /// Respuesta de Emma.
    /// </summary>
    private ReadOneResponse<Types.Cloud.OpenAssistant.Models.EmmaSchemaResponse>? EmmaResponse { get; set; } = null;


    private Movie Movie { get; set; }




    [Parameter]
    public Task<ReadOneResponse<Types.Cloud.OpenAssistant.Models.EmmaSchemaResponse>> ResponseIA { get; set; }



    /// <summary>
    /// Enviar a Emma.
    /// </summary>
    private async void ToEmma()
    {

        // Cambia el estado.
        ActualState = State.Responding;
        HeaderActualState = HeaderState.Titles;
        StateHasChanged();

        // Respuesta.
        OnPromptRequire?.Invoke(this, Prompt);

        if (ResponseIA == null)
            return;

        var response = await ResponseIA;


        switch (response.Response)
        {
            case Responses.Success:
                break;
            case Responses.RateLimitExceeded:
                response = new()
                {
                    Response = Responses.RateLimitExceeded,
                    Model = new()
                    {
                        UserText = "Has excedido el limite de solicitudes permitidas"
                    }
                };
                StateHasChanged();
                break;
            default:
                response = new()
                {
                    Response = Responses.RateLimitExceeded,
                    Model = new()
                    {
                        UserText = "Hubo un error"
                    }
                };
                StateHasChanged();
                break;
        }

        // Cambia el estado.
        ActualState = State.Witting;

        // Ejecutar comandos.
        foreach (var e in response.Model.Commands ?? [])
        {
            var app = new SILF.Script.App(e);
            app.AddDefaultFunctions(Functions.Actions);
            app.AddDefaultFunctions(Load());
            app.AddBridget([.. Functions.Delegates]);
            app.Run();
        }

        HeaderActualState = HeaderState.Titles;
        EmmaResponse = response;

        StateHasChanged();

    }



    /// <summary>
    /// Invocable.
    /// </summary>
    [JSInvokable("OnEmma")]
    public void OnEmma(string value)
    {
        Prompt = value;
        StateHasChanged();
        ToEmma();
    }

    private IEnumerable<SILFFunction> Load()
    {


        // Acción.
        SILFFunction weather =
        new(async (param) =>
        {

            Modelo = new()
            {
                Response = Responses.IsLoading
            };

            StateHasChanged();

            // Propiedades.
            var content = param.Where(T => T.Name == "contenido").FirstOrDefault();

            var city = await LIN.Access.Search.Controllers.Weather.Get(content.Objeto.Value.ToString());

            Modelo = city;

            HeaderActualState = HeaderState.Weather;

            StateHasChanged();


        })
        {
            Name = "weather",
            Parameters =
            [
                new Parameter("contenido", new("string"))
            ]
        };


        // Acción.
        SILFFunction search =
        new(async (param) =>
        {

            // Propiedades.
            var content = param.Where(T => T.Name == "contenido").FirstOrDefault();

            var city = LIN.Access.Search.Controllers.Search.Get(content.Objeto.Value.ToString());

            var movie = LIN.Access.Search.Controllers.Search.Movie(content.Objeto.Value.ToString());

            await city;
            await movie;

            SearchModels = city.Result;
            Movie = movie.Result.Model;

            HeaderActualState = HeaderState.Search;

            StateHasChanged();


        })
        {
            Name = "search",
            Parameters =
            [
                new Parameter("contenido", new("string"))
            ]
        };

        return [weather, search];



    }



}