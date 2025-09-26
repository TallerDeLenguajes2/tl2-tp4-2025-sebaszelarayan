using Microsoft.AspNetCore.Mvc;
using CadeteriaWebApi.Models;

namespace CadeteriaWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CadeteriaController : ControllerBase
    {
        // Para este TP, usamos una instancia estática simulando persistencia en memoria
        private static Cadeteria cadeteria = new Cadeteria("Cadetería Central", 381123456);

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
            var informe = cadeteria.mostrarInforme();
            return Ok(informe);
        }

        [HttpPost("agregarPedido")]
        public ActionResult AgregarPedido([FromBody] Pedido pedido)
        {
            cadeteria.altaPediodo(pedido);
            return CreatedAtAction(nameof(GetPedidos), pedido);
        }

        [HttpPut("asignarPedido/{idPedido}/{idCadete}")]
        public ActionResult AsignarPedido(int idPedido, int idCadete)
        {
            cadeteria.asignarCadeteAPedido(idCadete, idPedido);
            return Ok();
        }

        [HttpPut("cambiarEstado/{idPedido}/{nuevoEstado}")]
        public ActionResult CambiarEstadoPedido(int idPedido,[FromBody] EstadoPedido nuevoEstado)
        {
            cadeteria.cambiarEstado(nuevoEstado, idPedido);
            return Ok();
        }

        [HttpPut("cambiarCadete/{idPedido}/{idNuevoCadete}")]
        public ActionResult CambiarCadetePedido(int idPedido, int idNuevoCadete)
        {
            cadeteria.asignarCadeteAPedido(idNuevoCadete, idPedido);
            return Ok();
        }
    }
}
