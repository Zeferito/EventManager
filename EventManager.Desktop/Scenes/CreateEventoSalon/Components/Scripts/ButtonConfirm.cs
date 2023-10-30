using EventManager.Database.Models.Entities;
using Godot;
using System;
using System.Text.Json;

public partial class ButtonConfirm : Button
{
    [Export]
    private LineEdit _lineEditTitleEvent;

    [Export]
    private TextEdit _textEditDescriptionEvent;

    [Export]
    private LineEdit _lineEditStartDateEvent;

    [Export]
    private LineEdit _lineEditEndDateEvent;

    [Export]
    private VBoxContainer _clientesListaContainer;

    [Export]
    private VBoxContainer _salasListaContainer;

    [Export]
    private VBoxContainer _empleadosListaContainer;

    [Export]
    private VBoxContainer _agregablesListaContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Global global = GetNode<Global>("/root/Global");

        Pressed += () =>
        {
            Evento evento = new Evento();

            if (_lineEditTitleEvent.Text == "")
            {
                GD.Print("No se puede crear un evento sin nombre");
                return;
            }

            if (_textEditDescriptionEvent.Text == "")
            {
                GD.Print("No se puede crear un evento sin descripcion");
                return;
            }

            if (_lineEditStartDateEvent.Text == "")
            {
                GD.Print("No se puede crear un evento sin fecha de inicio");
                return;
            }

            if (_lineEditEndDateEvent.Text == "")
            {
                GD.Print("No se puede crear un evento infinito wtf");
                return;
            }

            evento.Nombre = _lineEditTitleEvent.Text;
            evento.Descripcion = _textEditDescriptionEvent.Text;

            DateTime startDateValue;
            if (!DateTime.TryParse(_lineEditStartDateEvent.Text, out startDateValue))
            {
                GD.Print("No se puede crear un evento con una fecha de inicio invalida");
                return;
            }

            evento.FechaInicio = startDateValue;

            DateTime endDateValue;
            if (!DateTime.TryParse(_lineEditEndDateEvent.Text, out endDateValue))
            {
                GD.Print("No se puede crear un evento con una fecha de inicio invalida");
                return;
            }

            evento.FechaTermino = endDateValue;

            foreach (ClienteItemContainer node in _clientesListaContainer.GetChildren())
            {
                evento.Clientes.Add(node.Cliente);
            }

            foreach (EmpleadoItemComponent node in _empleadosListaContainer.GetChildren())
            {
                evento.Empleados.Add(node.Empleado);
            }

            foreach (ItemSalaComponent node in _salasListaContainer.GetChildren())
            {
                evento.Salas.Add(node.Sala);
            }

            foreach (AgregableMaterialItemComponent node in _agregablesListaContainer.GetChildren())
            {
                EventoAgregable eventoAgregable = new EventoAgregable
                {
                    Agregable = node.Agregable,
                    CantidadReservada = node.Cantidad
                };

                evento.EventoAgregables.Add(eventoAgregable);
            }

            UserManager userManager = GetNode<UserManager>("/root/UserManager");

            evento.UsuarioId = userManager.Usuario.Id;

            string jsonString = JsonSerializer.Serialize(evento);

            GD.Print(jsonString);

            global.EventoToSend = evento;

            GetTree()
                .ChangeSceneToFile(
                    "res://Scenes/ConfirmacionEvento/confirmacion_evento_scene.tscn"
                );
        };
    }
}
