// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Database.Models.Entities
{
    [Table("evento")]
    public partial class Evento
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required]
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Column("fecha_inicio")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Column("fecha_termino")]
        public DateTime FechaTermino { get; set; }

        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public List<Cliente> Clientes { get; private set; } = new();

        public List<Sala> Salas { get; private set; } = new();

        public List<Empleado> Empleados { get; private set; } = new();

        public List<EventoAgregable> EventoAgregables { get; private set; } = new();
    }
}