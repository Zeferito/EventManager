using System.Text.Json;
using System.Text.Json.Serialization;
using EventManager.Desktop.Api.Entities;
using EventManager.Desktop.Scenes.Autoload.Scripts;
using Godot;
using Godot.Collections;
using Material = EventManager.Desktop.Api.Entities.Material;

namespace EventManager.Desktop.Scenes.CreateEventoSalon.Components.Scripts;

public partial class ButtonAgregarMaterial : TextureButton
{
    [Export]
    private VBoxContainer _listaMaterialesContainer;

    [Export]
    private OptionButton _optionButtonSeleccionarMaterial;

    [Export]
    private LineEdit _lineEditCantidadMaterial;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ApiConnection apiConnection = GetNode<ApiConnection>("/root/ApiConnection");

        Pressed += () =>
        {
            int id = (int)_optionButtonSeleccionarMaterial.GetSelectedMetadata();

            int cantidad;

            if (!int.TryParse(_lineEditCantidadMaterial.Text, out cantidad))
            {
                return;
            }

            foreach (AgregableMaterialItemComponent node in _listaMaterialesContainer.GetChildren())
            {
                if (node.EventToMaterial.Material.Id == id && node.EventToMaterial.AmountReserved == cantidad)
                {
                    return;
                }

                if (node.EventToMaterial.Material.Id == id)
                {
                    _listaMaterialesContainer.RemoveChild(node);
                    node.QueueFree();
                }
            }

            HttpRequest httpRequest = new HttpRequest();
            httpRequest.UseThreads = true;
            AddChild(httpRequest);
            httpRequest.RequestCompleted += HttpRequestCompleted;
            httpRequest.RequestCompleted += (result, code, strings, body) =>
            {
                RemoveChild(httpRequest);
                httpRequest.QueueFree();
            };

            string contentType = "application/json";
            string authToken = apiConnection.AuthDetails.AuthToken;
            string[] headers =
            {
                $"Content-Type: {contentType}",
                $"Authorization: Bearer {authToken}"
            };

            Error error = httpRequest.Request($"{apiConnection.Url}/materials/{id}", headers, HttpClient.Method.Get);

            if (error != Error.Ok)
            {
                GD.PushError("An error occurred in the HTTP request.");
            }
        };
    }

    private void HttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        Json json = new Json();
        json.Parse(body.GetStringFromUtf8());

        Array responseArray = json.Data.AsGodotArray();
        Dictionary responseDictionary = json.Data.AsGodotDictionary();

        switch (responseCode)
        {
            case 200:
                GD.Print(responseDictionary);

                string dictionaryJson = Json.Stringify(responseDictionary);

                int cantidad;

                if (!int.TryParse(_lineEditCantidadMaterial.Text, out cantidad))
                {
                    return;
                }

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    },
                };

                Material material = JsonSerializer.Deserialize<Material>(dictionaryJson, options);

                PackedScene agregableItemComponent
                    = ResourceLoader.Load<PackedScene>(
                        "res://Scenes/CreateEventoSalon/Components/agregable_material_item_component.tscn");

                AgregableMaterialItemComponent agregableMaterialItemComponent
                    = (AgregableMaterialItemComponent)agregableItemComponent.Instantiate();

                _listaMaterialesContainer.AddChild(agregableMaterialItemComponent);

                agregableMaterialItemComponent.EventToMaterial = new EventToMaterial
                {
                    Material = material,
                    AmountReserved = cantidad
                };
                break;
            default:
                GD.PrintErr(responseDictionary);
                break;
        }
    }
}