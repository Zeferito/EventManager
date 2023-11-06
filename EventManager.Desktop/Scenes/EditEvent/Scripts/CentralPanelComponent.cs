using Godot;
using System;

public partial class CentralPanelComponent : Control
{
	[Export]
	private LineEdit _lineEditNuevoNombreEvento;
	[Export]
	private LineEdit _lineEditNuevaDescripcionEvento;
	[Export]
	private LineEdit _lineEditFechaInicio;
	[Export]
	private LineEdit _lineEditFechaTermino;
	[Export]
	private VBoxContainer _listaClientesContainer;
	[Export]
	private VBoxContainer _listaMaterialesContainer;
	[Export]
	private VBoxContainer _listaEmpleadosContainer;
	[Export]
	private VBoxContainer _listaSalaContainer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
}
