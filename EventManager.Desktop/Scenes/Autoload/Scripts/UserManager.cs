using EventManager.Database.Models.Entities;
using Godot;
using System;

public partial class UserManager : Node
{
    public Usuario Usuario { get; set; }

    public override void _Ready()
    {
        Usuario = new Usuario { Id = 1 };
    }
}
