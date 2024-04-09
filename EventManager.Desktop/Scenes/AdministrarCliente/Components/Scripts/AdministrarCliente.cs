using EventManager.Desktop.Scenes.AdministrarCliente.Components.Scripts;
using Godot;
using System;

namespace EventManager.Desktop.Scenes.AdministrarCliente.Components.Scripts;

public partial class AdministrarCliente : Control
{
	[Export]
	private ListaConsultarClientesContainer _listaConsultarClientesContainer;

	[Export]
	private OptionButtonBuscarCliente _optionButtonBuscarCliente;

	[Export]
	private ListaClientesContainer _listaClientesContainer;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	public void RefreshContainers()
	{
		_listaConsultarClientesContainer.Refresh();
		_optionButtonBuscarCliente.Refresh();
		_listaClientesContainer.Refresh();
	}
}
