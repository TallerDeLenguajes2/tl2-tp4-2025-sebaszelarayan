
namespace CadeteriaWebApi.Models;


public enum EstadoPedido { Pendiente, EnCamino, Entregado }
public class Pedido
{
    private int nro;
    private string? obs;
    private Cliente? cliente;
    private EstadoPedido estado;
    private int idCadete;

    public int Nro { get => nro; set => nro = value; }
    public EstadoPedido Estado { get => estado; set => estado = value; }
    public int IdCadete { get => idCadete; set => idCadete = value; }
    public string? Obs { get => obs; set => obs = value; }
    public Cliente? Cliente { get => cliente; set => cliente = value; }


//Costructor por defecto para lectura del JSON
    public Pedido()
    {
    }
    public Pedido(int nro, string? obs, Cliente? cliente)
    {
        this.nro = nro;
        this.obs = obs;
        this.cliente = cliente;
        Estado = EstadoPedido.Pendiente;
        idCadete = -1;
    }

    private string verDireccionCliente()
    {
        return cliente?.Direccion ?? "sin direccion"; //Si cliente es null o cliente.Direccion es null, devuelve "sin direccion".
    }
    public string verDatosClientes()
    {
        return "Nombre: " + Cliente?.Nombre + "\nTelefono: " + Cliente?.Telefono + "\nDireccion: " + verDireccionCliente() + "\nReferencia: " + Cliente?.DatosReferenciaDireccion;
    }
    public string mostarPedido()
    {
        return "Nro: " + nro + "\nObservaciones: " + Obs + "\nEstado: " + Estado;
    }
}