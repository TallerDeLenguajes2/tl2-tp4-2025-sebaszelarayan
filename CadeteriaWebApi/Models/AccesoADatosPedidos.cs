namespace CadeteriaWebApi.Models;

using System.Text.Json;
public class AccesoADatosPedidos
{

    private string rutaArchivo = Path.Combine(AppContext.BaseDirectory, "Pedidos.json");
    public List<Pedido> Obtener() //obtiene los pedios del JSON
    {
        if (!File.Exists(rutaArchivo))
        {
            return new List<Pedido>();
        }

        string json = File.ReadAllText(rutaArchivo);
        var pedidos = JsonSerializer.Deserialize<List<Pedido>>(json);

        return pedidos ?? new List<Pedido>();
    }
    public void Guardar(List<Pedido> Pedidos) //guarda los datos de pedido lo utilizamos para actualizar medainte modificaciones el JSON
    {
        var opciones = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(Pedidos, opciones);
        File.WriteAllText(rutaArchivo, json);

    }
    
}