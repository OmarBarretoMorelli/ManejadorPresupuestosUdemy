using Dapper;
using ManejoPresupuestos.Models;
using ManejoPresupuestos.Servicios;
using ManejoPresupuestos.Servicios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuestos.Controllers
{
    public class TipoCuentasController : Controller
    {
        private readonly IRepositorioTipoCuenta repositorioTipoCuenta;
        private readonly IServicioUsuario servicioUsuario;

        public TipoCuentasController(IRepositorioTipoCuenta repositorioTipoCuenta, IServicioUsuario servicioUsuario)
        {
            this.repositorioTipoCuenta = repositorioTipoCuenta;
            this.servicioUsuario = servicioUsuario;
        }

        // GET: TipoCuentasController
        public async Task<IActionResult> Index()
        {
            int usuarioId = servicioUsuario.ObtenerUsuarioId();
            var tipoCuentas = await repositorioTipoCuenta.Obtener(usuarioId);
            return View(tipoCuentas);
        }

        // GET: TipoCuentasController
        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            var yaExiste = await repositorioTipoCuenta.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (yaExiste)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe");
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = servicioUsuario.ObtenerUsuarioId();
            await repositorioTipoCuenta.Crear(tipoCuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            int usuarioId = servicioUsuario.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTipoCuenta.ObtenerPorId(id, usuarioId);

            if (tipoCuenta == null)
            { 
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta) 
        {
            int usuarioId = servicioUsuario.ObtenerUsuarioId();

            var existe = await repositorioTipoCuenta.ObtenerPorId(tipoCuenta.Id, usuarioId);

            if (existe == null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTipoCuenta.Actualizar(tipoCuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExistenciaTipoCuenta(String nombre)
        {
            int usuarioId = servicioUsuario.ObtenerUsuarioId();
            var yaExiste = await repositorioTipoCuenta.Existe(nombre, usuarioId);

            if (yaExiste) 
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);

        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTipoCuenta.ObtenerPorId(id, usuarioId);

            if (tipoCuenta == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var existe = await repositorioTipoCuenta.ObtenerPorId(id, usuarioId);

            if (existe == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTipoCuenta.BorrarTipoCuenta(id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            //Esto tiene un poco de magia negra echa por el profe con LinkQ que habre de leer en el futuro
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var tiposCuentas = await repositorioTipoCuenta.Obtener(usuarioId); //Obtengo todos los tipoCuentas del usuario para chekear que los ids que recibo coinciden con estos.
            var idsTiposCuentas = tiposCuentas.Select(x => x.Id); //Para cada tipo cuentas selecciono el id y lo guardo en idsTiposCuentas (eso es lo que hace la funcion lamda)

            var idsTiposCuentasNoPertenecenAlUsuario = ids.Except(idsTiposCuentas).ToList(); //Aqui creo una lista con todos los id que el usuario me paso y no le pertenecen.

            if (idsTiposCuentasNoPertenecenAlUsuario.Count() > 0)
            {
                return Forbid();
            }

            var tiposCuentasOrdenados = ids.Select((valor, indice) => new TipoCuenta() { Id = valor, Orden = indice + 1 }).AsEnumerable(); //Esto es magia negra de LinkQ

            await repositorioTipoCuenta.Ordenar(tiposCuentasOrdenados); //Paso los tipos de cuenta ordenados parasobreescribir el orden en la BD

            return Ok();
        }
    }
}
