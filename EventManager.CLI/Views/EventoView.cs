// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.
using EventManager.Core.Database;
using EventManager.Core.Database.Models;
using EventManager.Core.Database.Services;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Tls;
using Org.BouncyCastle.Utilities;

namespace EventManager.CLI.Views
{
    public class EventoView
    {
        public class AgregableCantidad
        {
            public Agregable agregable;
            public int cantidad;

            public AgregableCantidad(Agregable agregable, int cantidad)
            {
                this.agregable = agregable;
                this.cantidad = cantidad;
            }
        }

        public void EliminarEvento()
        {
            Evento evento = ConsultarEvento();
            if (evento != null)
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    EventoService eventoService = new EventoService(context);
                    eventoService.DeleteEventoAsync(evento.Id).Wait();
                }
            }
        }

        public Evento ConsultarEvento()
        {
            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Buscar por Id");
                //Console.WriteLine("2. Buscar por fecha");
                Console.WriteLine("2. Cancelar");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        return BuscarEventoPorId();
                    /**case "2":
                        clientes.Add(RegistrarCliente());
                        break;**/
                    case "2":
                        Console.WriteLine("Volviendo al menu anterior");
                        return null;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private Evento BuscarEventoPorId()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                //Obtener a raiz de una fecha para luego consultar
                EventoService eventoService = new EventoService(context);

                int eventoId;
                while (true)
                {
                    Console.WriteLine("Ingrese Id del evento");
                    Console.WriteLine("Ingrese 0 para volver al menu anterior");
                    if (int.TryParse(Console.ReadLine(), out eventoId))
                    {
                        if (eventoId == 0)
                        {
                            Console.WriteLine("Volviendo al menu anterior");
                            return null;
                        }
                        if (!eventoService.EventoExistsAsync(eventoId).Result)
                        {
                            Console.WriteLine("Evento no existe");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("Id invalida");
                }

                var evento = eventoService.GetEventoByIdEagerAsync(eventoId).Result;
                if (evento != null)
                {
                    return evento;
                }
                else
                {
                    Console.WriteLine("Evento no encontrado");
                    return null;
                }
            }
        }

        public void AgregarEvento()
        {
            Evento evento = new Evento();
            //Con el login establecer el id del usuario para los eventos que se creen
            evento.UsuarioId = 1;

            List<AgregableCantidad> agregableCantidades = new List<AgregableCantidad>();
            List<Cliente> clientes = new List<Cliente>();
            List<Empleado> empleados = new List<Empleado>();
            List<Sala> salas = new List<Sala>();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Asignar Periodo de Evento");
                Console.WriteLine("2. Agregar agregables de Evento");
                Console.WriteLine("3. Titulo del Evento");
                Console.WriteLine("4. Descripción del Evento");
                Console.WriteLine("5. Agregar Cliente a Evento");
                Console.WriteLine("6. Agregar Empleados para Evento");
                Console.WriteLine("7. Agregar sala de Evento");
                Console.WriteLine("8. Confirmar Evento");
                Console.WriteLine("9. Cancelar el Evento");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        DateTime Inicio = AsignarFechaInicio();
                        DateTime Final = AsignarFechaFinal();
                        if (Inicio != null && Final != null)
                        {
                            if (Inicio > Final)
                            {
                                Console.WriteLine(
                                    "La fecha de inicio no puede ser mayor a la fecha de finalización"
                                );
                                break;
                            }
                        }
                        evento.FechaInicio = Inicio;
                        evento.FechaTermino = Final;
                        break;
                    case "2":
                        agregableCantidades = AgregarAgregableAEvento();
                        break;
                    case "3":
                        evento.Nombre = TituloEvento();
                        break;
                    case "4":
                        evento.Descripcion = DescripcionEvento();
                        break;
                    case "5":
                        clientes = AsignarClienteAEvento();
                        break;
                    case "6":
                        empleados = AgregarEmpleadoAEvento();
                        break;
                    case "7":
                        salas = AgregarSalasAEvento();
                        break;
                    case "8":
                        Confirmar(evento, agregableCantidades, empleados, clientes, salas);
                        return;
                    case "9":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private static string TituloEvento()
        {
            Console.WriteLine("Ingrese el titulo del evento");
            return Console.ReadLine();
        }

        private static string DescripcionEvento()
        {
            Console.WriteLine("Ingrese descripción del evento");
            return Console.ReadLine();
        }

        private static DateTime AsignarFechaInicio()
        {
            //Establecer fecha del evento
            DateTime fecha;
            while (true)
            {
                Console.WriteLine("Ingrese la fecha y hora de inicio del evento");
                if (DateTime.TryParse(Console.ReadLine(), out fecha))
                {
                    break;
                }
                Console.WriteLine("Fecha invalida");
            }

            return fecha;
        }

        private static DateTime AsignarFechaFinal()
        {
            //Establecer fecha del evento
            DateTime fecha;
            while (true)
            {
                Console.WriteLine("Ingrese la fecha y hora de termino del evento");
                if (DateTime.TryParse(Console.ReadLine(), out fecha))
                {
                    break;
                }
                Console.WriteLine("Fecha invalida");
            }

            return fecha;
        }

        private List<AgregableCantidad> AgregarAgregableAEvento()
        {
            List<AgregableCantidad> agregableCantidades = new List<AgregableCantidad>();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Agregar Material para el evento");
                Console.WriteLine("2. Descartar Cantidad Material");
                Console.WriteLine("3. Confirmar");
                Console.WriteLine("4. Cancelar");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        agregableCantidades.Add(BuscarAgregable());
                        break;
                    case "2":
                        SelecionarAgregable(agregableCantidades);
                        break;
                    case "3":
                        return agregableCantidades;
                    case "4":
                        Console.WriteLine("Volviendo al menu anterior");
                        return null;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private void SelecionarAgregable(List<AgregableCantidad> agregableCantidades)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                AgregableService agregableService = new AgregableService(context);
                int agregableId;
                while (true)
                {
                    Console.WriteLine("Ingrese Id del agregable");
                    if (int.TryParse(Console.ReadLine(), out agregableId))
                    {
                        if(agregableId < 0 || agregableId >= agregableCantidades.Count)
                        {
                            Console.WriteLine("Id invalida");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("Id invalida");
                }

                Console.WriteLine("Cantidad a remover");
                int cantidadARemover;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out cantidadARemover))
                    {
                        if (cantidadARemover >= agregableCantidades[agregableId].cantidad)
                        {
                            agregableCantidades.Remove(agregableCantidades[agregableId]);
                            return;
                        }
                        agregableCantidades[agregableId].cantidad -= cantidadARemover;
                        return;
                    }
                }
            }
        }

        private AgregableCantidad BuscarAgregable()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                AgregableService agregableService = new AgregableService(context);
                int agregableId;
                while (true)
                {
                    Console.WriteLine("Ingrese Id del agregable");
                    if (int.TryParse(Console.ReadLine(), out agregableId))
                    {
                        if (!agregableService.AgregableExistsAsync(agregableId).Result)
                        {
                            Console.WriteLine("Agregable no existe");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("Id invalida");
                }

                Agregable agregable = agregableService.GetAgregableByIdAsync(agregableId).Result;

                int cantidad;
                while (true)
                {
                    Console.WriteLine("Ingrese la cantidad del agregable");
                    if (int.TryParse(Console.ReadLine(), out cantidad))
                    {
                        if (cantidad > agregable.Total || cantidad <= 0)
                        {
                            Console.WriteLine("Cantidad invalida");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("Cantidad invalida");
                }
                return new AgregableCantidad(agregable, cantidad);
            }
        }

        public List<Cliente> AsignarClienteAEvento()
        {
            List<Cliente> clientes = new List<Cliente>();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Buscar Cliente");
                Console.WriteLine("2. Registrar Cliente");
                Console.WriteLine("3. Descartar Cliente");
                Console.WriteLine("4. Confirmar");
                Console.WriteLine("5. Cancelar");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        clientes.Add(BuscarCliente());
                        break;
                    case "2":
                        clientes.Add(RegistrarCliente());
                        break;
                    case "3":
                        clientes.Remove(BuscarCliente());
                        break;
                    case "4":
                        return clientes;
                    case "5":
                        Console.WriteLine("Volviendo al menu anterior");
                        return null;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private Cliente BuscarCliente()
        {
            //Buscar cliente
            using (DatabaseContext context = new DatabaseContext())
            {
                ClienteService clienteService = new ClienteService(context);

                int clienteId;
                while (true)
                {
                    Console.WriteLine("Ingrese id del cliente");
                    if (int.TryParse(Console.ReadLine(), out clienteId))
                    {
                        if (!clienteService.ClienteExistsAsync(clienteId).Result)
                        {
                            Console.WriteLine("Cliente no existe");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("id invalido");
                }

                return clienteService.GetClienteByIdAsync(clienteId).Result;
            }
        }

        private Cliente RegistrarCliente()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                ClienteService clienteService = new ClienteService(context);

                Console.WriteLine("Ingrese nombre del cliente");
                string nombre = Console.ReadLine();

                Console.WriteLine("Ingrese telefono del cliente");
                string telefono = Console.ReadLine();

                return clienteService
                    .CreateClienteAsync(new Cliente { Nombre = nombre, Telefono = telefono })
                    .Result;
            }
        }

        public List<Empleado> AgregarEmpleadoAEvento()
        {
            List<Empleado> empleados = new List<Empleado>();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Agregar empleado");
                Console.WriteLine("2. Descartar empleado");
                Console.WriteLine("3. Confirmar");
                Console.WriteLine("4. Cancelar");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        empleados.Add(ObtenerEmpleado());
                        break;
                    case "2":
                        empleados.Remove(ObtenerEmpleado());
                        break;
                    case "3":
                        return empleados;
                    case "4":
                        Console.WriteLine("Volviendo al menu anterior");
                        return null;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private Empleado ObtenerEmpleado()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                EmpleadoService agregableService = new EmpleadoService(context);

                int empleadoId;
                while (true)
                {
                    Console.WriteLine("Ingrese Id del empleado");
                    if (int.TryParse(Console.ReadLine(), out empleadoId))
                    {
                        break;
                    }
                    Console.WriteLine("Id invalida");
                }

                return agregableService.GetEmpleadoByIdAsync(empleadoId).Result;
            }
        }

        public List<Sala> AgregarSalasAEvento()
        {
            List<Sala> salas = new List<Sala>();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Buscar Sala");
                Console.WriteLine("2. Descartar Sala");
                Console.WriteLine("3. Confirmar");
                Console.WriteLine("4. Cancelar");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        salas.Add(ObtenerSala());
                        break;
                    case "2":
                        salas.Remove(ObtenerSala());
                        break;
                    case "3":
                        return salas;
                    case "4":
                        Console.WriteLine("Volviendo al menu anterior");
                        return null;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private Sala ObtenerSala()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                SalaService salaService = new SalaService(context);

                int salaId;
                while (true)
                {
                    Console.WriteLine("Ingrese id de la sala");
                    if (int.TryParse(Console.ReadLine(), out salaId))
                    {
                        if (!salaService.SalaExistsAsync(salaId).Result)
                        {
                            Console.WriteLine("Sala no existe");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("id invalido");
                }

                return salaService.GetSalaByIdAsync(salaId).Result;
            }
        }

        private void Confirmar(
            Evento evento,
            List<AgregableCantidad> agregableCantidades,
            List<Empleado> empleados,
            List<Cliente> clientes,
            List<Sala> salas
        )
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                EventoService eventoService = new EventoService(context);

                Evento eventoCreado = eventoService.CreateEventoAsync(evento).Result;

                for (int i = 0; i < agregableCantidades.Count; i++)
                {
                    eventoService
                        .EventoAddAgregableAsync(
                            eventoCreado.Id,
                            agregableCantidades[i].agregable.Id,
                            agregableCantidades[i].cantidad
                        )
                        .Wait();
                }

                foreach (Empleado empleado in empleados)
                {
                    eventoService.EventoAddEmpleadoAsync(eventoCreado.Id, empleado.Id).Wait();
                }

                foreach (Cliente cliente in clientes)
                {
                    eventoService.EventoAddClienteAsync(eventoCreado.Id, cliente.Id).Wait();
                }

                foreach (Sala sala in salas)
                {
                    eventoService.EventoAddSalaAsync(eventoCreado.Id, sala.Id).Wait();
                }
            }
        }
    }
}
