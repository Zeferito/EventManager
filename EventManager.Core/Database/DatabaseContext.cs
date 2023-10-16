// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.Configuration;
using EventManager.Core.Models;
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
        public DbSet<EventoAgregable> EventoAgregables { get; set; }
        public DbSet<EventoEmpleado> EventoEmpleados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evento>(evento =>
            {
                evento
                    .HasMany(evento => evento.Clientes)
                    .WithOne(cliente => cliente.Evento)
                    .HasForeignKey(cliente => cliente.EventoId)
                    .OnDelete(DeleteBehavior.SetNull);

                evento
                    .HasMany(evento => evento.Salas)
                    .WithOne(sala => sala.Evento)
                    .HasForeignKey(sala => sala.EventoId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Usuario>(usuario =>
            {
                usuario
                    .HasMany(usuario => usuario.Eventos)
                    .WithOne(evento => evento.Usuario)
                    .HasForeignKey(evento => evento.UsuarioId);

                usuario
                    .HasIndex(u => u.Nombre)
                    .IsUnique();
            });

            modelBuilder.Entity<EventoEmpleado>(eventoEmpleado =>
            {
                eventoEmpleado
                    .HasKey(ee => new { ee.EventoId, ee.EmpleadoId });

                eventoEmpleado
                    .HasOne(ee => ee.Evento)
                    .WithMany(evento => evento.EventoEmpleados)
                    .HasForeignKey(ee => ee.EventoId)
                    .OnDelete(DeleteBehavior.Cascade);

                eventoEmpleado
                    .HasOne(ee => ee.Empleado)
                    .WithMany(empleado => empleado.EventoEmpleados)
                    .HasForeignKey(ee => ee.EmpleadoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EventoAgregable>(eventoAgregable =>
            {
                eventoAgregable
                    .HasKey(ea => new { ea.EventoId, ea.AgregableId });

                eventoAgregable
                    .HasOne(ea => ea.Evento)
                    .WithMany(evento => evento.EventoAgregables)
                    .HasForeignKey(ea => ea.EventoId)
                    .OnDelete(DeleteBehavior.Cascade);

                eventoAgregable
                    .HasOne(ea => ea.Agregable)
                    .WithMany(agregable => agregable.EventoAgregables)
                    .HasForeignKey(ea => ea.AgregableId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseContext"].ConnectionString;

            optionsBuilder.UseMySQL(connectionString);
        }

        public static void RemoveAgregableFromEvento(int eventoId, int agregableId)
        {
            using DatabaseContext context = new DatabaseContext();

            EventoAgregable? eventoAgregable = context.EventoAgregables
                .FirstOrDefault(ab => ab.EventoId == eventoId && ab.AgregableId == agregableId);

            if (eventoAgregable == null)
            {
                return;
            }

            context.EventoAgregables.Remove(eventoAgregable);
        }
    }
}