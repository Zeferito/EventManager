// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.Configuration;
using EventManager.Core.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Core.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<Agregable> Agregables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Usuario>()
                .HasMany(usuario => usuario.Eventos)
                .WithOne(evento => evento.Usuario)
                .HasForeignKey(evento => evento.UsuarioId);

            modelBuilder.Entity<Usuario>().HasIndex(u => u.Nombre).IsUnique();

            modelBuilder
                .Entity<Evento>()
                .HasMany(evento => evento.Clientes)
                .WithOne(cliente => cliente.Evento)
                .HasForeignKey(cliente => cliente.EventoId);

            modelBuilder
                .Entity<Evento>()
                .HasMany(evento => evento.Salas)
                .WithOne(sala => sala.Evento)
                .HasForeignKey(sala => sala.EventoId);

            modelBuilder.Entity<EventoEmpleado>().HasKey(ee => new { ee.EventoId, ee.EmpleadoId });

            modelBuilder
                .Entity<EventoEmpleado>()
                .HasOne(ee => ee.Evento)
                .WithMany(evento => evento.EventoEmpleados)
                .HasForeignKey(ee => ee.EventoId);

            modelBuilder
                .Entity<EventoEmpleado>()
                .HasOne(ee => ee.Empleado)
                .WithMany(empleado => empleado.EventoEmpleados)
                .HasForeignKey(ee => ee.EmpleadoId);

            modelBuilder
                .Entity<EventoAgregable>()
                .HasKey(ea => new { ea.EventoId, ea.AgregableId });

            modelBuilder
                .Entity<EventoAgregable>()
                .HasOne(ea => ea.Evento)
                .WithMany(evento => evento.EventoAgregables)
                .HasForeignKey(ea => ea.EventoId);

            modelBuilder
                .Entity<EventoAgregable>()
                .HasOne(ea => ea.Agregable)
                .WithMany(agregable => agregable.EventoAgregables)
                .HasForeignKey(ea => ea.AgregableId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            string connectionString = ConfigurationManager.ConnectionStrings[
                "DatabaseContext"
            ].ConnectionString;

            optionsBuilder.UseMySQL(connectionString);
        }
    }
}
