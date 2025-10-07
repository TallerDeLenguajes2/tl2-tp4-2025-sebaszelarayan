namespace CadeteriaWebApi.Models
{
    public class Cadeteria
    {
        // ============================
        // Propiedades principales
        // ============================

        // Nombre de la cadetería (nullable por si no se carga desde JSON)
        public string? Nombre { get; set; }

        // Teléfono de contacto de la cadetería
        public long Telefono { get; set; }

        // Lista de cadetes asociados a la cadetería
        // Se inicializa vacía para evitar null reference
        public List<Cadete> Cadetes { get; set; } = new List<Cadete>();

        // Lista de pedidos administrados por la cadetería
        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();


        // ============================
        // Constructores
        // ============================

        // Constructor con parámetros (útil si se instancia manualmente)
        public Cadeteria(string? nombre, long telefono)
        {
            Nombre = nombre;
            Telefono = telefono;
        }

        // Constructor vacío necesario para la deserialización JSON
        public Cadeteria() { }


        // ============================
        // Creación y carga de datos
        // ============================

        // Crea un nuevo pedido a partir de los datos del cliente y la observación
        public Pedido CrearPedido(string nombre, string direccion, string datosReferenciaDireccioncion, long telefono, string observacion)
        {
            // Se crea un cliente con los datos recibidos
            Cliente cliente = new Cliente(nombre, direccion, telefono, datosReferenciaDireccioncion);

            // Se crea un pedido asociado al cliente
            Pedido pedido = new Pedido();

            return pedido;
        }

        // Carga una lista de cadetes (por ejemplo, leída desde JSON)
        public void AgregarListaCadetes(List<Cadete> cadetes)
        {
            Cadetes = cadetes;
        }

        // Carga una lista de pedidos (por ejemplo, desde un archivo)
        public void AgregarListaPedidos(List<Pedido> pedidos)
        {
            Pedidos = pedidos;
        }


        // ============================
        //  Operaciones básicas
        // ============================

        // Agrega un nuevo pedido a la lista
        public void AltaPedido(Pedido pedido)
        {
            Pedidos.Add(pedido);
        }

        // Agrega un nuevo cadete
        public void AgregarCadete(Cadete cadete)
        {
            Cadetes.Add(cadete);
        }


        // ============================
        // Consultas sobre pedidos
        // ============================

        // Devuelve la cantidad de pedidos entregados por un cadete específico
        public int CantidadPedidosEntregados(int idCadete)
        {
            return Pedidos.Count(p => p.IdCadete == idCadete && p.Estado == EstadoPedido.Entregado);
        }

        // Busca un pedido por número (devuelve null si no lo encuentra)
        private Pedido? BuscarPedido(int idPedido)
        {
            return Pedidos.FirstOrDefault(p => p.Nro == idPedido);
        }

        // Busca un cadete por ID
        private Cadete? BuscarCadete(int idCadete)
        {
            return Cadetes.FirstOrDefault(c => c.Id == idCadete);
        }


        // ============================
        //  Asignación y estado
        // ============================

        // Asigna un cadete a un pedido específico
        public void AsignarCadeteAPedido(int idCadete, int idPedido)
        {
            var pedido = BuscarPedido(idPedido);
            var cadete = BuscarCadete(idCadete);

            // Solo se asigna si ambos existen
            if (pedido != null && cadete != null)
            {
                pedido.IdCadete = idCadete;
            }
        }

        // Permite cambiar la asignación de un pedido a otro cadete
        public void ReasignarPedido(int idPedido, int idCadeteNuevo)
        {
            AsignarCadeteAPedido(idCadeteNuevo, idPedido);
        }

        // Cambia el estado de un pedido (Pendiente, EnCamino, Entregado)
        public void CambiarEstado(EstadoPedido estado, int idPedido)
        {
            var pedido = BuscarPedido(idPedido);
            if (pedido != null)
            {
                pedido.Estado = estado;
            }
        }


        // ============================
        //  Cálculos e informes
        // ============================

        // Calcula el jornal de un cadete ($500 por cada pedido entregado)
        public float JornalACobrar(int idCadete)
        {
            return 500 * CantidadPedidosEntregados(idCadete);
        }

        // Muestra los pedidos de un cadete en formato texto
        public string MostrarListaPedidosCadete(int idCadete)
        {
            var pedidosCadete = Pedidos.Where(p => p.IdCadete == idCadete).ToList();
            return string.Join("\n", pedidosCadete.Select(p => p.mostarPedido()));
        }

        // Devuelve la lista completa de pedidos
        public List<Pedido> GetPedidos()
        {
            return Pedidos;
        }

        // Devuelve la lista completa de cadetes
        public List<Cadete> GetCadetes()
        {
            return Cadetes;
        }


        // ============================
        // Informe general
        // ============================

        // Muestra un resumen de toda la cadetería:
        // - Pedidos entregados por cadete
        // - Ganancia total
        // - Promedio de envíos
        public string MostrarInforme()
        {
            if (Cadetes.Count == 0)
                return "No se encontro lista";

            string informe = "\n=== Informe Final ===\n";
            int totalEnvios = 0;
            float totalGanancia = 0;

            // Recorre cada cadete para calcular su rendimiento
            foreach (var cadete in Cadetes)
            {
                int entregados = CantidadPedidosEntregados(cadete.Id);
                float ganancia = JornalACobrar(cadete.Id);
                totalEnvios += entregados;
                totalGanancia += ganancia;

                informe += $"Nombre: {cadete.Nombre} → Entregados: {entregados} envíos → Ganancias: {ganancia}\n";
            }

            // Calcula el promedio de envíos por cadete
            double promedio = Cadetes.Count > 0 ? (double)totalEnvios / Cadetes.Count : 0;

            informe += $"\nTotal de envíos: {totalEnvios}\nGanancia total: ${totalGanancia}\nPromedio de envíos por cadete: {promedio:F2}\n";

            return informe;
        }
    }
}
