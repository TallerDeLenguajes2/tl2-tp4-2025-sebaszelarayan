namespace CadeteriaWebApi.Models;


public class Cliente
{

    private string? nombre;
    private string? direccion;
    private long telefono;
    private string? datosReferenciaDireccion;

    public Cliente(string? nombre, string? direccion, long telefono, string? datosReferenciaDireccion)
    {
        this.nombre = nombre;
        this.direccion = direccion;
        this.telefono = telefono;
        this.datosReferenciaDireccion = datosReferenciaDireccion;
    }

    public string? Nombre { get => nombre; set => nombre = value; }
    public string? Direccion { get => direccion; set => direccion = value; }
    public long Telefono { get => telefono; set => telefono = value; }
    public string? DatosReferenciaDireccion { get => datosReferenciaDireccion; set => datosReferenciaDireccion = value; }


}