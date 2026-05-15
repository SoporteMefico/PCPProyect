using Microsoft.AspNetCore.Mvc;
using PCPProyect.Models;
using PCPProyect.Servicio;
using PCPProyect.ViewModel;

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
        [HttpPost]
        public async Task<IActionResult> ObtenerGrid([FromBody] ProyeccionFiltroDTVM request)
        {
            var resultado = await _service.ObtenerGrid(request);

            return Json(resultado);
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
