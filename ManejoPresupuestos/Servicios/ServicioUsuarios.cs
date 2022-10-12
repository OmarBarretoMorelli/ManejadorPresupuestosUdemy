using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManejoPresupuestos.Servicios.Interfaces;

namespace ManejoPresupuestos.Servicios
{
    public class ServicioUsuarios : IServicioUsuario
    {
        public int ObtenerUsuarioId()
        { 
            return 1;
        }
    }
}
