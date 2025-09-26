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

    public Pedido(int nro, string? obs, Cliente? cliente)
    {
        this.Nro = nro;
        this.obs = obs;
        this.cliente = cliente;
        Estado = EstadoPedido.Pendiente;
        idCadete = 0;
    }

    public string verDireccionCliente()
    {
        return cliente?.Direccion;
    }
    public string verDatosClientes()
    {
        return "Nombre: " + cliente?.Nombre + "\nTelefono: " + cliente?.Telefono + "\nDireccion: " + verDireccionCliente() + "\nReferencia: " + cliente?.DatosReferenciaDireccion;
    }
    public string mostarPedido()
    {
        return "Nro: " + nro + "\nObservaciones: " + obs + "\nEstado: " + Estado;
    }
}