
namespace CadeteriaWebApi.Models;
public class Cadete
{
    private int id;
    private string? nombre;
    private string? direccion;
    private long telefono;

    public int Id { get => id; set => id = value; }
    public string? Nombre { get => nombre; set => nombre = value; }

    public Cadete(int id, string? nombre, string? direccion, long telefono)
    {
        this.Id = id;
        this.Nombre = nombre;
        this.direccion = direccion;
        this.telefono = telefono;
    }
}


