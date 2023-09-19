using MeninoDev.Entidades;
using System.Collections.Generic;

namespace MeninoDev.Services
{
    public interface ICategoriaService
    {
        IEnumerable<Categoria> BuscarTodas();
    }
}
