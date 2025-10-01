namespace CadeteriaWebApi.Models;

public class Cadeteria
{
    public string? Nombre { get; set; }
    public long Telefono { get; set; }
    public List<Cadete> Cadetes { get; set; } = new List<Cadete>();
    public List<Pedido> Pedidos { get; set; } = new List<Pedido>();

    private int contadorPedidos = 1;

    // Constructor con parámetros (opcional, por si querés instanciar manualmente)
    public Cadeteria(string? nombre, long telefono)
    {
        Nombre = nombre;
        Telefono = telefono;
    }

    // Constructor vacío requerido por el serializer
    public Cadeteria() { }

    public Pedido CrearPedido(string nombre, string direccion, string datosReferenciaDireccioncion, long telefono, string observacion)
    {
        Cliente cliente = new Cliente(nombre, direccion, telefono, datosReferenciaDireccioncion);
        Pedido pedido = new Pedido();
        return pedido;
    }

    public void AgregarListaCadetes(List<Cadete> cadetes)
    {
        Cadetes = cadetes;
    }

    public void AgregarListaPedidos(List<Pedido> pedidos)
    {
        Pedidos = pedidos;
    }

    public void AltaPedido(Pedido pedido)
    {
        Pedidos.Add(pedido);
    }

    public void AgregarCadete(Cadete cadete)
    {
        Cadetes.Add(cadete);
    }

    public int CantidadPedidosEntregados(int idCadete)
    {
        return Pedidos.Count(p => p.IdCadete == idCadete && p.Estado == EstadoPedido.Entregado);
    }

    private Pedido? BuscarPedido(int idPedido)
    {
        return Pedidos.FirstOrDefault(p => p.Nro == idPedido);
    }

    private Cadete? BuscarCadete(int idCadete)
    {
        return Cadetes.FirstOrDefault(c => c.Id == idCadete);
    }

    public void AsignarCadeteAPedido(int idCadete, int idPedido)
    {
        var pedido = BuscarPedido(idPedido);
        var cadete = BuscarCadete(idCadete);

        if (pedido != null && cadete != null)
        {
            pedido.IdCadete = idCadete;
        }
    }

    public void ReasignarPedido(int idPedido, int idCadeteNuevo)
    {
        AsignarCadeteAPedido(idCadeteNuevo, idPedido);
    }

    public void CambiarEstado(EstadoPedido estado, int idPedido)
    {
        var pedido = BuscarPedido(idPedido);
        if (pedido != null)
        {
            pedido.Estado = estado;
        }
    }

    public float JornalACobrar(int idCadete)
    {
        return 500 * CantidadPedidosEntregados(idCadete);
    }

    public string MostrarListaPedidosCadete(int idCadete)
    {
        var pedidosCadete = Pedidos.Where(p => p.IdCadete == idCadete).ToList();
        return string.Join("\n", pedidosCadete.Select(p => p.mostarPedido()));
    }

    public List<Pedido> GetPedidos()
    {
        return Pedidos;
    }

    public List<Cadete> GetCadetes()
    {
        return Cadetes;
    }

    public string MostrarInforme()
    {
        if (Cadetes.Count == 0)
            return "No se encontro lista";

        string informe = "\n=== Informe Final ===\n";
        int totalEnvios = 0;
        float totalGanancia = 0;

        foreach (var cadete in Cadetes)
        {
            int entregados = CantidadPedidosEntregados(cadete.Id);
            float ganancia = JornalACobrar(cadete.Id);
            totalEnvios += entregados;
            totalGanancia += ganancia;

            informe += $"Nombre: {cadete.Nombre} → Entregados: {entregados} envíos → Ganancias: {ganancia}\n";
        }

        double promedio = Cadetes.Count > 0 ? (double)totalEnvios / Cadetes.Count : 0;
        informe += $"\nTotal de envíos: {totalEnvios}\nGanancia total: ${totalGanancia}\nPromedio de envíos por cadete: {promedio:F2}\n";

        return informe;
    }
}
