using ManejoPresupuestos.Models;
using ManejoPresupuestos.Servicios;
using ManejoPresupuestos.Servicios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuestos.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly IServicioUsuario servicioUsuario;
        private readonly IRepositorioTransacciones repositorioTransacciones;
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IRepositorioCuentas repositorioCuentas;

        public TransaccionesController(IServicioUsuario servicioUsuario, IRepositorioTransacciones repositorioTransacciones,
                                       IRepositorioCategorias repositorioCategorias,IRepositorioCuentas repositorioCuentas)
        {
            this.servicioUsuario = servicioUsuario;
            this.repositorioTransacciones = repositorioTransacciones;
            this.repositorioCategorias = repositorioCategorias;
            this.repositorioCuentas = repositorioCuentas;
        }

        public async Task<IActionResult> Crear()
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var modelo = new TransaccionCreacionViewModel();
            modelo.Cuentas = await ObtenerCuentas(usuarioId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Transaccion transaccion)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            //FALTA implementar
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId)
        {
            var cuentas =  await repositorioCuentas.Buscar(usuarioId);
            return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerCategorias(int usuarioId, TipoOperacion tipoOperacion)
        {
            var categorias = await repositorioCategorias.Obtener(usuarioId, tipoOperacion);
            return categorias.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerCategorias([FromBody] TipoOperacion tipoOperacion)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var categorias = await ObtenerCategorias(usuarioId, tipoOperacion);
            return Ok(categorias);
        }

    }
}
