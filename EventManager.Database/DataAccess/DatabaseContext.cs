// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.Configuration;
using EventManager.Database.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Database.DataAccess
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

        private readonly string _connectionString;

        public DatabaseContext() { }

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

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
                    .HasMany(evento => evento.Empleados)
                    .WithMany(empleado => empleado.Eventos)
                    .UsingEntity(
                        "evento_empleado",
                        l =>
                            l.HasOne(typeof(Empleado))
                                .WithMany()
                                .HasForeignKey("empleado_id")
                                .HasPrincipalKey(nameof(Empleado.Id)),
                        r =>
                            r.HasOne(typeof(Evento))
                                .WithMany()
                                .HasForeignKey("evento_id")
                                .HasPrincipalKey(nameof(Evento.Id)),
                        j => j.HasKey("evento_id", "empleado_id")
                    );

                evento
                    .HasMany(evento => evento.Salas)
                    .WithMany(sala => sala.Eventos)
                    .UsingEntity(
                        "evento_sala",
                        l =>
                            l.HasOne(typeof(Sala))
                                .WithMany()
                                .HasForeignKey("sala_id")
                                .HasPrincipalKey(nameof(Sala.Id)),
                        r =>
                            r.HasOne(typeof(Evento))
                                .WithMany()
                                .HasForeignKey("evento_id")
                                .HasPrincipalKey(nameof(Evento.Id)),
                        j => j.HasKey("evento_id", "sala_id")
                    );
            });

            modelBuilder.Entity<Empleado>(empleado =>
            {
                empleado
                    .HasMany(empleado => empleado.Eventos)
                    .WithMany(evento => evento.Empleados)
                    .UsingEntity(
                        "evento_empleado",
                        l =>
                            l.HasOne(typeof(Empleado))
                                .WithMany()
                                .HasForeignKey("empleado_id")
                                .HasPrincipalKey(nameof(Empleado.Id)),
                        r =>
                            r.HasOne(typeof(Evento))
                                .WithMany()
                                .HasForeignKey("evento_id")
                                .HasPrincipalKey(nameof(Evento.Id)),
                        j => j.HasKey("evento_id", "empleado_id")
                    );
            });

            modelBuilder.Entity<Sala>(sala =>
            {
                sala.HasMany(sala => sala.Eventos)
                    .WithMany(evento => evento.Salas)
                    .UsingEntity(
                        "evento_sala",
                        l =>
                            l.HasOne(typeof(Sala))
                                .WithMany()
                                .HasForeignKey("sala_id")
                                .HasPrincipalKey(nameof(Sala.Id)),
                        r =>
                            r.HasOne(typeof(Evento))
                                .WithMany()
                                .HasForeignKey("evento_id")
                                .HasPrincipalKey(nameof(Evento.Id)),
                        j => j.HasKey("evento_id", "sala_id")
                    );
            });

            modelBuilder.Entity<Usuario>(usuario =>
            {
                usuario
                    .HasMany(usuario => usuario.Eventos)
                    .WithOne(evento => evento.Usuario)
                    .HasForeignKey(evento => evento.UsuarioId);

                usuario.HasIndex(u => u.Nombre).IsUnique();
            });

            modelBuilder.Entity<EventoAgregable>(eventoAgregable =>
            {
                eventoAgregable.HasKey(ea => new { ea.EventoId, ea.AgregableId });

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

            if (_connectionString != null)
            {
                optionsBuilder.UseMySQL(_connectionString);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings[
                "DatabaseContext"
            ].ConnectionString;

            optionsBuilder.UseMySQL(connectionString);
        }
    }
}
