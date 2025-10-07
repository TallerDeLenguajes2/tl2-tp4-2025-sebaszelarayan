using Microsoft.AspNetCore.Mvc;
using CadeteriaWebApi.Models;
using System.Linq;

namespace CadeteriaWebApi.Controllers
{
    [ApiController] // Indica que este controlador es una API (no MVC con vistas)
    [Route("api/[controller]")] // La ruta base será /api/Cadeteria
    public class CadeteriaController : ControllerBase
    {
        // ==== Atributos privados ====
        private Cadeteria cadeteria; // Instancia principal que maneja la lógica
        private AccesoADatosCadeteria ADCadeteria; // Acceso a datos de la cadetería
        private AccesoADatosCadetes ADCadetes; // Acceso a datos de cadetes
        private AccesoADatosPedidos ADPedidos; // Acceso a datos de pedidos

        // ==== Constructor ====
        public CadeteriaController()
        {
            // Inicializa los objetos de acceso a datos
            ADCadeteria = new AccesoADatosCadeteria();
            ADCadetes = new AccesoADatosCadetes();
            ADPedidos = new AccesoADatosPedidos();

            // Carga la cadetería desde los datos, o crea una nueva si no existe
            cadeteria = ADCadeteria.Obtener() ?? new Cadeteria();

            // Carga las listas de cadetes y pedidos (si no hay, usa listas vacías)
            cadeteria.AgregarListaCadetes(ADCadetes.Obtener() ?? new List<Cadete>());
            cadeteria.AgregarListaPedidos(ADPedidos.Obtener() ?? new List<Pedido>());
        }

        // ==== MÉTODOS HTTP ====

        // --- GET: /api/cadeteria/pedidos
        [HttpGet("pedidos")]
        public ActionResult<List<Pedido>> GetPedidos()
        {
            var pedidos = cadeteria.GetPedidos();

            // Si no hay pedidos cargados, devolver 404
            if (pedidos == null || !pedidos.Any())
                return NotFound("No hay pedidos registrados.");

            // Caso contrario, devolver 200 OK con la lista
            return Ok(pedidos);
        }

        // --- GET: /api/cadeteria/cadetes
        [HttpGet("cadetes")]
        public ActionResult<List<Cadete>> GetCadetes()
        {
            var cadetes = cadeteria.GetCadetes();

            // Si no hay cadetes, devolver 404
            if (cadetes == null || !cadetes.Any())
                return NotFound("No hay cadetes cargados.");

            // Devuelve la lista si existen
            return Ok(cadetes);
        }

        // --- GET: /api/cadeteria/informe
        [HttpGet("informe")]
        public ActionResult<object> GetInforme()
        {
            var informe = cadeteria.MostrarInforme();

            // Devuelve un objeto con mensaje + los datos del informe
            return Ok(new
            {
                mensaje = "Informe generado correctamente",
                data = informe
            });
        }

        // --- POST: /api/cadeteria/agregarPedido
        [HttpPost("agregarPedido")]
        public ActionResult<Pedido> AgregarPedido(string observacionPedido, string nombreCliente, string direccionCliente, long telefonoCliente, string datosReferenciaDireccion)
        {
            // Validación: nombre obligatorio
            if (string.IsNullOrWhiteSpace(nombreCliente))
                return BadRequest("Debe ingresar el nombre del cliente.");

            // Validación: número de teléfono válido
            if (telefonoCliente <= 0)
                return BadRequest("El número de teléfono debe ser válido.");

            // Crea un nuevo cliente con los datos recibidos
            Cliente cliente = new Cliente(nombreCliente, direccionCliente, telefonoCliente, datosReferenciaDireccion);

            // Determina el número de pedido (incremental)
            int nro = cadeteria.GetPedidos().Any() ? cadeteria.GetPedidos().Max(p => p.Nro) + 1 : 1;

            // Crea el pedido y lo agrega a la cadetería
            Pedido pedido = new Pedido(nro, observacionPedido, cliente);
            cadeteria.AltaPedido(pedido);

            // Guarda todos los pedidos actualizados en archivo/datos
            ADPedidos.Guardar(cadeteria.GetPedidos());

            // Devuelve 201 Created con mensaje y el pedido creado
            return CreatedAtAction(nameof(GetPedidos), new { id = pedido.Nro }, new
            {
                mensaje = "Pedido creado exitosamente.",
                pedido
            });
        }

        // --- PUT: /api/cadeteria/asignarPedido/{idPedido}/{idCadete}
        [HttpPut("asignarPedido/{idPedido}/{idCadete}")]
        public ActionResult AsignarPedido(int idPedido, int idCadete)
        {
            // Busca el pedido y el cadete correspondientes
            var pedido = cadeteria.GetPedidos().FirstOrDefault(p => p.Nro == idPedido);
            var cadete = cadeteria.GetCadetes().FirstOrDefault(c => c.Id == idCadete);

            // Validaciones: si no existen, devuelve 404
            if (pedido == null)
                return NotFound($"No se encontró el pedido con ID {idPedido}.");
            if (cadete == null)
                return NotFound($"No se encontró el cadete con ID {idCadete}.");

            // Asigna el cadete al pedido
            cadeteria.AsignarCadeteAPedido(idCadete, idPedido);

            // Guarda los cambios en la base de pedidos
            ADPedidos.Guardar(cadeteria.GetPedidos());

            // Devuelve 200 OK con mensaje descriptivo
            return Ok(new
            {
                mensaje = $"El pedido #{idPedido} fue asignado al cadete {cadete.Nombre}."
            });
        }

        // --- PUT: /api/cadeteria/cambiarEstado/{idPedido}/{nuevoEstado}
        [HttpPut("cambiarEstado/{idPedido}/{nuevoEstado}")]
        public ActionResult CambiarEstadoPedido(int idPedido, EstadoPedido nuevoEstado)
        {
            // Busca el pedido
            var pedido = cadeteria.GetPedidos().FirstOrDefault(p => p.Nro == idPedido);

            // Si no existe, devuelve 404
            if (pedido == null)
                return NotFound($"No se encontró el pedido con ID {idPedido}.");

            // Cambia el estado en la lógica de negocio
            cadeteria.CambiarEstado(nuevoEstado, idPedido);

            // Guarda los cambios persistidos
            ADPedidos.Guardar(cadeteria.GetPedidos());

            // Devuelve mensaje de éxito
            return Ok(new
            {
                mensaje = $"Estado del pedido #{idPedido} actualizado a {nuevoEstado}."
            });
        }

        // --- PUT: /api/cadeteria/cambiarCadete/{idPedido}/{idNuevoCadete}
        [HttpPut("cambiarCadete/{idPedido}/{idNuevoCadete}")]
        public ActionResult CambiarCadetePedido(int idPedido, int idNuevoCadete)
        {
            // Busca el pedido y el nuevo cadete
            var pedido = cadeteria.GetPedidos().FirstOrDefault(p => p.Nro == idPedido);
            var cadeteNuevo = cadeteria.GetCadetes().FirstOrDefault(c => c.Id == idNuevoCadete);

            // Validaciones de existencia
            if (pedido == null)
                return NotFound($"No se encontró el pedido con ID {idPedido}.");
            if (cadeteNuevo == null)
                return NotFound($"No se encontró el nuevo cadete con ID {idNuevoCadete}.");

            // Asigna el nuevo cadete
            cadeteria.AsignarCadeteAPedido(idNuevoCadete, idPedido);

            // Guarda el cambio en el archivo/lista
            ADPedidos.Guardar(cadeteria.GetPedidos());

            // Devuelve mensaje de confirmación
            return Ok(new
            {
                mensaje = $"El pedido #{idPedido} fue reasignado al cadete {cadeteNuevo.Nombre}."
            });
        }
    }
}
