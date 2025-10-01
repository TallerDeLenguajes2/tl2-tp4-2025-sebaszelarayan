using Microsoft.AspNetCore.Mvc;
using CadeteriaWebApi.Models;

namespace CadeteriaWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CadeteriaController : ControllerBase
    {
        private Cadeteria cadeteria; //Cadeteria ya no es clase estática
        private AccesoADatosCadeteria ADCadeteria;
        private AccesoADatosCadetes ADCadetes;
        private AccesoADatosPedidos ADPedidos;
        public CadeteriaController()
        {
            ADCadeteria = new AccesoADatosCadeteria();
            ADCadetes = new AccesoADatosCadetes();
            ADPedidos = new AccesoADatosPedidos();
            cadeteria = ADCadeteria.Obtener();
            cadeteria.AgregarListaCadetes(ADCadetes.Obtener());
            cadeteria.AgregarListaPedidos(ADPedidos.Obtener());

        }
        [HttpGet("pedidos")]
        public ActionResult<List<Pedido>> GetPedidos()
        {
            return Ok(cadeteria.GetPedidos());
        }

        [HttpGet("cadetes")]
        public ActionResult<List<Cadete>> GetCadetes()
        {
            return Ok(cadeteria.GetCadetes());
        }

        [HttpGet("informe")]
        public ActionResult<object> GetInforme()
        {
            var informe = cadeteria.MostrarInforme();
            return Ok(informe);
        }

        [HttpPost("agregarPedido")]
        public ActionResult<string> AgregarPedido([FromBody] Pedido pedido)
        {
            cadeteria.AltaPedido(pedido);
            ADPedidos.Guardar(cadeteria.GetPedidos());
            return Created("Pedido dado de alta exitosamente",pedido);
        }

        [HttpPut("asignarPedido/{idPedido}/{idCadete}")]
        public ActionResult AsignarPedido(int idPedido, int idCadete)
        {
            cadeteria.AsignarCadeteAPedido(idCadete, idPedido);
            ADPedidos.Guardar(cadeteria.GetPedidos());
            return Ok("Pedido asignado correctamente");
        }

        [HttpPut("cambiarEstado/{idPedido}/{nuevoEstado}")]
        public ActionResult CambiarEstadoPedido(int idPedido, [FromBody] EstadoPedido nuevoEstado)
        {
            cadeteria.CambiarEstado(nuevoEstado, idPedido);
            ADPedidos.Guardar(cadeteria.GetPedidos());
            return Ok("Estado cambiado correctamente");
        }

        [HttpPut("cambiarCadete/{idPedido}/{idNuevoCadete}")]
        public ActionResult CambiarCadetePedido(int idPedido, int idNuevoCadete)
        {
            cadeteria.AsignarCadeteAPedido(idNuevoCadete, idPedido);
            ADPedidos.Guardar(cadeteria.GetPedidos());
            return Ok("Pedido cambiado correctamente");
        }
    }
}
