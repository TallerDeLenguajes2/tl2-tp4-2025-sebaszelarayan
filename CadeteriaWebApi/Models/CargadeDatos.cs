namespace CadeteriaWebApi.Models;

public interface IAccesoADatos
{
    Cadeteria cargarDatos(string pathCadeteria, string pathCadetes);
}
