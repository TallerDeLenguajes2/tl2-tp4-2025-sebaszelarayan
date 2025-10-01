namespace CadeteriaWebApi.Models;
using System.Text.Json;
public class AccesoADatosCadetes
{
    public List<Cadete>? Obtener()
    {
        string pathCadetes = Path.Combine(AppContext.BaseDirectory, "Cadetes.json");

        // Leer cadetes desde JSON
        string jsonCadetes = File.ReadAllText(pathCadetes);
        List<Cadete>? cadetes = JsonSerializer.Deserialize<List<Cadete>>(jsonCadetes);

        return cadetes;
    }
}