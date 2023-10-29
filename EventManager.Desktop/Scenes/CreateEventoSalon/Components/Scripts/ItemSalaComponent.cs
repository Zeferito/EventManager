using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class ItemSalaComponent : HBoxContainer
{
    private VBoxContainer _parentContainer;

    private Label _labelSala;

    private Button _textureButtonEliminar;

    private Sala _sala;

    public Sala Sala
    {
        get => _sala;
        set => SetSala(value);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { 
		_parentContainer = GetParent<VBoxContainer>();
		_labelSala = GetNode<Label>("LabelSala");
		_textureButtonEliminar = GetNode<Button>("ButtonRemoveSala");

		_textureButtonEliminar.Pressed += () =>
		{
			_parentContainer.RemoveChild(this);
			QueueFree();
		};
	}

    private void SetSala(Sala value)
    {
        _sala = value;
        _labelSala.Text = _sala.Nombre;
    }
}
