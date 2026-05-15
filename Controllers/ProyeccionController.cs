using Microsoft.AspNetCore.Mvc;
using PCPProyect.Models;
using PCPProyect.Servicio;

namespace PCPProyect.Controllers
{
    public class ProyeccionController : Controller
    {
        private readonly IProyeccionService _service;

        public ProyeccionController(IProyeccionService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerGrid([FromBody] ProyeccionFiltroVM filtro)
        {
            var data = await _service.ObtenerGrid(filtro);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarCelda([FromBody] ProyeccionUpdateDto dto)
        {
            await _service.GuardarCelda(dto);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GuardarLote([FromBody] List<ProyeccionUpdateDto> lista)
        {
            await _service.GuardarLote(lista);
            return Ok();
        }
    }
}
