// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lombok.NET;

namespace EventManager.Core.Models
{
    [Table("evento_empleados")]
    public partial class EventoEmpleado
    {
        [Required]
        [Column("empleado_id")]
        public int EmpleadoId { get; set; }

        public Empleado Empleado { get; set; }

        [Required]
        [Column("evento_id")]
        public int EventoId { get; set; }

        public Evento Evento { get; set; }
    }
}