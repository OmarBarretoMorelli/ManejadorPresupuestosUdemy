using ManejoPresupuestos.Models;
using ManejoPresupuestos.Servicios;
using ManejoPresupuestos.Servicios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuestos.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IServicioUsuario servicioUsuario;

        public CategoriasController(IRepositorioCategorias repositorioCategorias, IServicioUsuario servicioUsuario)
        {

            this.repositorioCategorias = repositorioCategorias;
            this.servicioUsuario = servicioUsuario;
        }

        public async Task<ActionResult> Index()
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var categorias = await repositorioCategorias.Obtener(usuarioId);
            return View(categorias);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                return View("NoEncontrado", "Home");
            }
            categoria.UsuarioId = usuarioId;
            await repositorioCategorias.Crear(categoria);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var categoria = await repositorioCategorias.ObteneroPorId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NoEncontrad", "Home");
            }

            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var categoriaObtenida = await repositorioCategorias.ObteneroPorId(categoria.Id, usuarioId);

            if (categoriaObtenida is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            categoria.UsuarioId = usuarioId;

            await repositorioCategorias.Editar(categoria);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Borrar(int id) 
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var categoria = await repositorioCategorias.ObteneroPorId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Borrar(Categoria categoria)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var categoriaObtenida = await repositorioCategorias.ObteneroPorId(categoria.Id, usuarioId);

            if (categoriaObtenida is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCategorias.Borrar(categoria.Id);
            return RedirectToAction("Index");
        }
    }
}
