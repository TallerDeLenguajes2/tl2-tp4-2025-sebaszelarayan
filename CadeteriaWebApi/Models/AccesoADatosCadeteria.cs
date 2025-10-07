namespace CadeteriaWebApi.Models;

using System.Text.Json;
public class AccesoADatosCadeteria
{
    public Cadeteria? Obtener() // obtiene los datos de cadeteria mediante el JSON
    {
        string pathCadeteria = Path.Combine(AppContext.BaseDirectory, "Cadeteria.json");
        string jsonCadeteria = File.ReadAllText(pathCadeteria);
        Cadeteria? cadeteria = JsonSerializer.Deserialize<Cadeteria>(jsonCadeteria) ?? new Cadeteria();
        // Cargar cadetes
        var datosCadetes = new AccesoADatosCadetes();
        var cadetes = datosCadetes.Obtener() ?? new List<Cadete>(); 
        cadeteria.AgregarListaCadetes(cadetes);

        // Cargar pedidos
        var datosPedidos = new AccesoADatosPedidos();
        var pedidos = datosPedidos.Obtener();
        cadeteria.AgregarListaPedidos(pedidos);

        return cadeteria;

    }

}