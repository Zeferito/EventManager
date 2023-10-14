// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Database.Models
{
    [Table("evento")]
    public class Evento
    {
        [Key][Column("id")] public int Id { get; set; }
        [Required][Column("fecha")] public DateTime Fecha { get; set; }
        [Required][Column("usuario_id")] public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public List<Cliente> Clientes { get; set; }
        public List<Sala> Salas { get; set; }
        public List<EventoEmpleado> EventoEmpleados { get; set; }
        public List<EventoAgregable> EventoAgregables { get; set; }
    }
}