namespace CadeteriaWebApi.Models;
using System.Text.Json;

    public class AccesoADatosJSON : IAccesoADatos
    {
        public Cadeteria cargarDatos(string pathCadeteria, string pathCadetes)
        {
            // Leer cadetería desde JSON
            string jsonCadeteria = File.ReadAllText(pathCadeteria);
            Cadeteria? cadeteria = JsonSerializer.Deserialize<Cadeteria>(jsonCadeteria);

            if (cadeteria == null)
                throw new Exception("Error al deserializar cadetería");
            // Leer cadetes desde JSON
            string jsonCadetes = File.ReadAllText(pathCadetes);
            List<Cadete>? cadetes = JsonSerializer.Deserialize<List<Cadete>>(jsonCadetes);

            if (cadetes != null)
            {
                foreach (var cad in cadetes)
                {
                    cadeteria.agregarCadete(cad);
                }
            }

            return cadeteria;
        }
    }
