namespace CadeteriaWebApi.Models;

using System.Text.Json;
public class AccesoADatosCadeteria
{
    public Cadeteria? Obtener()
    {
        string pathCadeteria = Path.Combine(AppContext.BaseDirectory, "Cadeteria.json");
            string jsonCadeteria = File.ReadAllText(pathCadeteria);
            Cadeteria? cadeteria = JsonSerializer.Deserialize<Cadeteria>(jsonCadeteria);
            // Cargar cadetes
            var datosCadetes = new AccesoADatosCadetes();
            var cadetes = datosCadetes.Obtener();
            cadeteria.AgregarListaCadetes(cadetes);

            // Cargar pedidos
            var datosPedidos = new AccesoADatosPedidos();
            var pedidos = datosPedidos.Obtener();
            cadeteria.AgregarListaPedidos(pedidos);

            return cadeteria;
        
    }

}