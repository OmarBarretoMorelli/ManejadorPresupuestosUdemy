using AutoMapper;
using ManejoPresupuestos.Models;
using ManejoPresupuestos.Servicios;
using ManejoPresupuestos.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuestos.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IRepositorioTipoCuenta repositorioTipoCuenta;
        private readonly IServicioUsuario servicioUsuario;
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IMapper mapper;

        public CuentasController(IRepositorioTipoCuenta repositorioTipoCuenta, IServicioUsuario servicioUsuario, IRepositorioCuentas repositorioCuentas, IMapper mapper)
        {
            this.repositorioTipoCuenta = repositorioTipoCuenta;
            this.servicioUsuario = servicioUsuario;
            this.repositorioCuentas = repositorioCuentas;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var cuentasConTipoCuenta = await repositorioCuentas.Buscar(usuarioId);

            var modelo = cuentasConTipoCuenta
                .GroupBy(x => x.TipoCuenta)
                .Select(grupo => new IndiceCuentasViewModel
                {
                    TipoCuenta = grupo.Key,
                    Cuentas = grupo.AsEnumerable()
                }).ToList();
            return View(modelo);
        }



        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var modelo = new CuentaCreacionViewModel();
            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuenta)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTipoCuenta.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

            if (tipoCuenta == null) //Si el usuario poste un tipo cuenta que no es suyo
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid) //Si el usuario poste informacion erronea le vuelvo a enviar toda la info
            {
                cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(cuenta);
            }

            await repositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.obtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<CuentaCreacionViewModel>(cuenta); //mapper mapea cuenta a CuentaCreacionViewModel, devolviendo un nuevo objeto
            

            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CuentaCreacionViewModel cuentaEditar)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.obtenerPorId(cuentaEditar.Id,usuarioId);

            if (cuenta is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var tipoCuenta = await repositorioTipoCuenta.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCuentas.Actualizar(cuentaEditar);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.obtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.obtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCuentas.Borrar(id);
            return RedirectToAction("Index");
        }



        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId)
        { 
            var tipoCuentas = await repositorioTipoCuenta.Obtener(usuarioId);
            return tipoCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }


    }
}
