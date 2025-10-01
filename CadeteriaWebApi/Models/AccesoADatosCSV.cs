namespace CadeteriaWebApi.Models;

    public class AccesoADatosCSV : IAccesoADatos
    {
        public Cadeteria cargarDatos(string pathCadeteria, string pathCadetes)
        {
            // Leer datos de la cadetería
            string[] datosCadeteria = File.ReadAllText(pathCadeteria).Split(',');
            string nombre = datosCadeteria[0];
            long telefono = long.Parse(datosCadeteria[1]);

            // Crear cadetería
            Cadeteria cadeteria = new Cadeteria(nombre, telefono);

            // Leer cadetes
            foreach (var linea in File.ReadAllLines(pathCadetes))
            {
                string[] partes = linea.Split(',');
                int id = int.Parse(partes[0]);
                string nombreCad = partes[1];
                string direccionCad = partes[2];
                long telCad = long.Parse(partes[3]);

                Cadete cad = new Cadete(id, nombreCad, direccionCad, telCad);
                cadeteria.AgregarCadete(cad);
            }

            return cadeteria;
        }
    }
