using Microsoft.AspNetCore.Mvc;
using PCPProyect.Servicio;
using PCPProyect.ViewModel;

namespace PCPProyect.Controllers
{
    public class PCPControlController : Controller
    {
        private readonly IPCPControlService _service;

        public PCPControlController(IPCPControlService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult>
            ObtenerGrid(
                [FromBody] PCPControlFiltroVM filtro)
        {
            var result =
                await _service.ObtenerGrid(filtro);

            return Json(new
            {
                total = result.Total,
                data = result.Data
            });
        }
    }
}
