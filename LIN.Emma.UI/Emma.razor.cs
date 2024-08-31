using LIN.Emma.UI.Classes;
using LIN.Types.Exp.Search.Models;
using LIN.Types.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SILF.Script.Elements.Functions;
using SILF.Script.Enums;
using SILF.Script.Interfaces;

namespace LIN.Emma.UI;


public partial class Emma
{


    /// <summary>
    /// Recibe un intento (Admin).
    /// </summary>
    public event EventHandler<string>? OnPromptRequire;





    string Raw = "";

    ReadAllResponse<SearchResult> SearchModels { get; set; } = new();


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
    private ReadOneResponse<Types.Emma.Models.ResponseIAModel>? EmmaResponse { get; set; } = null;


    private Movie Movie { get; set; }




    [Parameter]
    public Task<ReadOneResponse<LIN.Types.Emma.Models.ResponseIAModel>> ResponseIA { get; set; }



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

        // Cambia el estado.
        ActualState = State.Witting;

        Raw = response.Model.Content;

        // Es un comando.
        if (response.Model.Content.StartsWith("#"))
        {
            var app = new SILF.Script.App(response.Model.Content.Remove(0, 1), new A());
            app.AddDefaultFunctions(Functions.Actions);
            app.AddDefaultFunctions(Load());

            EmmaResponse = new()
            {
                Response = Responses.Success,
                Model = new()
                {
                    Content = "Perfecto"
                }
            };

            app.Run();
            StateHasChanged();
            return;
        }

        // Establece la respuesta de Emma.
        EmmaResponse = response;
        HeaderActualState = HeaderState.Titles;
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


    IEnumerable<SILFFunction> Load()
    {

        // Acción.
        SILFFunction actionMessage =
        new((param) =>
        {

            // Propiedades.
            var content = param.Where(T => T.Name == "contenido").FirstOrDefault();

            EmmaResponse = new()
            {
                Message = content?.Objeto.Value.ToString(),
                Response = Responses.Success,
                Model = new()
                {
                    Content = content?.Objeto.Value.ToString(),
                    IsSuccess = true
                }
            };

            HeaderActualState = HeaderState.Titles;

            StateHasChanged();


        })
        {
            Name = "say",
            Parameters =
            [
                new Parameter("contenido", new("string"))
            ]
        };


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

        return [actionMessage, weather, search];



    }



}

class A : IConsole
{
    public void InsertLine(string error, string result, LogLevel logLevel)
    {
        var s = "";
    }
}