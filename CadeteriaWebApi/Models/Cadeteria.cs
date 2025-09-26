namespace CadeteriaWebApi.Models;

public class Cadeteria
{
    private string? nombre;
    private long telefono;
    private List<Cadete>? listadoCadetes;

    private List<Pedido>? listadoPedidos;

    int contadorPedidos = 1;

    public Cadeteria(string? nombre, long telefono)
    {
        this.nombre = nombre;
        this.telefono = telefono;
        listadoCadetes = new List<Cadete>();
        listadoPedidos = new List<Pedido>();
    }

    public Pedido crearPedido(string nombre, string direccion, string datosReferenciaDireccioncion, long telefono, string observacion)
    {

        // Crear cliente y pedido
        Cliente cliente = new Cliente(nombre, direccion, telefono, datosReferenciaDireccioncion);
        Pedido pedido = new Pedido(contadorPedidos++, observacion, cliente);
        return pedido;
    }
    public void altaPediodo(Pedido pedido)
    {
        // Guardar en la lista de pedidos
        if (listadoPedidos != null)
        {
            listadoPedidos.Add(pedido);
        }
    }
    public void agregarCadete(Cadete cadete)
    {
        if (listadoCadetes != null)
        {
            listadoCadetes.Add(cadete);
        }
    }
    public int CantidadPedidosEntregados(int idCadete)
    {
        int pedidosEntregado = 0;
        if (listadoPedidos != null)
        {
            foreach (var pedido in listadoPedidos)
            {
                if (pedido.IdCadete == idCadete)
                {
                    if (pedido.Estado == EstadoPedido.Entregado)
                    {
                        pedidosEntregado++;
                    }
                }
            }
        }
        return pedidosEntregado;
    }
    private Pedido? buscarPedido(int idPedido)
    {
        if (listadoPedidos != null)
        {
            foreach (var pedido in listadoPedidos)
            {
                if (pedido.Nro == idPedido)
                {
                    return pedido;
                }
            }
        }
        return null;
    }
    private Cadete? buscarCadete(int idCadete)
    {
        if (listadoCadetes != null)
        {
            foreach (var cadete in listadoCadetes)
            {
                if (cadete.Id == idCadete)
                {
                    return cadete;
                }
            }

        }
        return null;
    }
    public void asignarCadeteAPedido(int idCadete, int idPedido)
    {
        if (buscarCadete(idCadete) != null && buscarPedido(idPedido) != null)

        {
            buscarPedido(idPedido).IdCadete = idCadete;
        }
    }
    public void reasiganarPedido(int idPedido, int idCadeteNuevo)
    {
        asignarCadeteAPedido(idCadeteNuevo, idPedido);
    }
    public void cambiarEstado(EstadoPedido estado, int idPedido)
    {
        if (buscarPedido(idPedido) != null)
        {
            buscarPedido(idPedido).Estado = estado;

        }
    }
    public float JornalACobrar(int idCadete)
    {
        return 500 * CantidadPedidosEntregados(idCadete);

    }

    public void mostarListaPedidiosCadete(int idCadete)
    {
        if (listadoPedidos != null)
        {
            foreach (var pedido in listadoPedidos)
            {
                if (pedido.IdCadete == idCadete)
                {
                    pedido.mostarPedido();
                }
            }
        }
    }
    // Devuelve la lista de pedidos
    public List<Pedido> GetPedidos()
    {
        return listadoPedidos ?? new List<Pedido>();
    }

    // Devuelve la lista de cadetes
    public List<Cadete> GetCadetes()
    {
        return listadoCadetes ?? new List<Cadete>();
    }

    public string mostrarInforme()
    {
        string informe = "No se encontro lista";
        if (listadoCadetes != null)
        {
            informe = "\n=== Informe Final ===\n";
            int totalEnvios = 0;
            float totalGanancia = 0;

            foreach (var cadete in listadoCadetes)
            {
                int entregados = CantidadPedidosEntregados(cadete.Id);
                float ganancia = JornalACobrar(cadete.Id);
                totalEnvios += entregados;
                totalGanancia += ganancia;

                informe = informe + "Nombre: " + cadete.Nombre + " → Entrengados:" + entregados + "envíos → Ganancias: " + ganancia + "\n";
            }
            double promedio = listadoCadetes.Count > 0 ? (double)totalEnvios / listadoCadetes.Count : 0;

            informe = informe + $"\nTotal de envíos: {totalEnvios}\n" + $"Ganancia total: ${totalGanancia}\n" + $"Promedio de envíos por cadete: {promedio:F2}\n";
        }
        return informe;
    }
}