namespace CadeteriaWebApi.Models;

using System.Text.Json;
public class AccesoADatosPedidos
{

    private string rutaArchivo = Path.Combine(AppContext.BaseDirectory, "Pedidos.json");
    public List<Pedido> Obtener()
    {
        if (!File.Exists(rutaArchivo))
        {
            return new List<Pedido>();
        }

        string json = File.ReadAllText(rutaArchivo);
        var pedidos = JsonSerializer.Deserialize<List<Pedido>>(json);

        return pedidos ?? new List<Pedido>();
    }
    public void Guardar(List<Pedido> Pedidos)
    {
        var opciones = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(Pedidos, opciones);
        File.WriteAllText(rutaArchivo, json);

    }
    
}