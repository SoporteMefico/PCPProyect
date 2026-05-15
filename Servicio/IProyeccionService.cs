using Microsoft.EntityFrameworkCore;
using PCPProyect.Datos;
using PCPProyect.Models;
using PCPProyect.ViewModel;
using System.Globalization;

namespace PCPProyect.Servicio

{
    public interface IProyeccionService
    {
        //Task<List<ProyeccionGridVM>> ObtenerGrid(ProyeccionFiltroVM filtro);
        Task<DataTableResponseVM<ProyeccionGridVM>> ObtenerGrid(ProyeccionFiltroDTVM request);

        Task GuardarCelda(ProyeccionUpdateDto dto);
        Task GuardarLote(List<ProyeccionUpdateDto> lista);
    }

    public class ProyeccionService : IProyeccionService
    {
        private readonly ApplicationDbContext _context;

        public ProyeccionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataTableResponseVM<ProyeccionGridVM>> ObtenerGrid(ProyeccionFiltroDTVM request)
        {
            var desde = DateTime.Parse(request.FechaDesde).Date;

            var hasta = DateTime.Parse(request.FechaHasta).Date;

            // =========================
            // 1. PROYECCIONES (SQL → MEMORIA)
            // =========================
            var dataProy = await _context.MovHis00
                .Where(x => x.FecIniPro.HasValue &&
                            x.FecIniPro.Value.Date >= desde &&
                            x.FecIniPro.Value.Date <= hasta)
                .Select(x => new
                {
                    CodDoc = x.CodDoc.Trim(),
                    NumDoc = x.NumDoc.Trim(),
                    NumIte = x.NumIte.Trim(),
                    Fecha = x.FecIniPro.Value,
                    Cantidad = x.CantidadProyectada ?? 0
                })
                .ToListAsync();

            // Agrupar por semana (en memoria)
            var proyecciones = dataProy
                .Select(x => new
                {
                    x.CodDoc,
                    x.NumDoc,
                    x.NumIte,
                    Anio = ISOWeek.GetYear(x.Fecha),
                    Semana = ISOWeek.GetWeekOfYear(x.Fecha),
                    x.Cantidad
                })
                .GroupBy(x => new { x.CodDoc, x.NumDoc, x.NumIte, x.Anio, x.Semana })
                .Select(g => new
                {
                    g.Key.CodDoc,
                    g.Key.NumDoc,
                    g.Key.NumIte,
                    KeySemana = $"{g.Key.Anio}-W{g.Key.Semana}",
                    Cantidad = g.Sum(x => x.Cantidad)
                })
                .ToList();

            // =========================
            // 2. LISTA DE SEMANAS
            // =========================
            var semanas = proyecciones
                .Select(x => x.KeySemana)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            // =========================
            // 3. CABECERA
            // =========================
            var cabeceras = await _context.Movcabe1ot01
                .Where(x => x.XEstDoc != "A")
                .Select(x => new
                {
                    CodDoc = x.CodDoc.Trim(),
                    NumDoc = x.NumDoc.Trim(),
                    Cliente = x.NomAne
                })
                .ToListAsync();

            // =========================
            // 4. DETALLE
            // =========================
            //var detalles = await _context.Movdete1ot01
            //    .Select(x => new
            //    {
            //        CodDoc = x.CodDoc.Trim(),
            //        NumDoc = x.NumDoc.Trim(),
            //        NumIte = x.NumIte.Trim(),

            //        x.CodArt,
            //        x.DesArt,
            //        x.CanTot
            //    }).Where(x => x.NumIte == "01")
            //    .ToListAsync();
            var detalles = await (
    from a in _context.Movcabe1ot01

    join b in _context.Movdete1ot01
        on new { a.CodDoc, a.NumDoc }
        equals new { b.CodDoc, b.NumDoc }

    join c in _context.Articulo
        on new
        {
            CodSubAlm = b.CodSubAlm.Trim(),
            CodArt = b.CodArt.Trim()
        }
        equals new
        {
            CodSubAlm = c.CodSubAlm.Trim(),
            CodArt = c.CodArt.Trim()
        }
        into artJoin

    from c in artJoin.DefaultIfEmpty()

    where a.XEstDoc != "A"
          && b.NumIte == "01"

    select new
    {
        CodDoc = b.CodDoc.Trim(),
        NumDoc = b.NumDoc.Trim(),
        NumIte = b.NumIte.Trim(),

        CodArt = b.CodArt,
        DesArt = b.DesArt,

        CanTot = b.CanTot,

        PesoUnitario = c != null
            ? c.PesArt
            : 0
    }
).ToListAsync();

            // =========================
            // 5. JOIN EN MEMORIA
            // =========================
            var datos = (
                from det in detalles
                join cab in cabeceras
                    on new { det.CodDoc, det.NumDoc }
                    equals new { cab.CodDoc, cab.NumDoc }

                select new
                {
                    det.CodDoc,
                    det.NumDoc,
                    det.NumIte,

                    cab.Cliente,

                    det.CodArt,
                    det.DesArt,
                    det.CanTot,
                    det.PesoUnitario
                }
            ).ToList();

            // =========================
            // 6. ARMAR GRID FINAL
            // =========================
            var resultado = datos.Select(d =>
            {
                var item = new ProyeccionGridVM
                {
                    CodDoc = d.CodDoc,
                    NumDoc = d.NumDoc,
                    NumIte = d.NumIte,
                    Cliente = d.Cliente,
                    CodArt = d.CodArt,
                    DesArt = d.DesArt,
                    Cantidad = d.CanTot,
                    PesoUnitario = d.PesoUnitario
                };

                // =========================
                // Inicializar semanas
                // =========================
                foreach (var s in semanas)
                {
                    item.Semanas[s] = new SemanaDataVM
                    {
                        Cantidad = 0,
                        Peso = 0
                    };
                }

                // =========================
                // Proyecciones del item
                // =========================
                var proyItem = proyecciones
                    .Where(p =>
                        p.CodDoc == d.CodDoc &&
                        p.NumDoc == d.NumDoc &&
                        p.NumIte == d.NumIte);

                foreach (var p in proyItem)
                {
                    item.Semanas[p.KeySemana] = new SemanaDataVM
                    {
                        Cantidad = p.Cantidad,
                        Peso = p.Cantidad * d.PesoUnitario
                    };
                }

                return item;

            }).AsQueryable();


            // =========================
            // BUSCADOR DATATABLES
            // =========================
            if (!string.IsNullOrWhiteSpace(request.Search?.Value))
            {
                string texto = request.Search.Value.ToLower();

                resultado = resultado.Where(x =>

                    (x.Cliente != null &&
                     x.Cliente.ToLower().Contains(texto))

                    ||

                    (x.NumDoc != null &&
                     x.NumDoc.ToLower().Contains(texto))

                    ||

                    (x.CodArt != null &&
                     x.CodArt.ToLower().Contains(texto))

                    ||

                    (x.DesArt != null &&
                     x.DesArt.ToLower().Contains(texto))
                );
            }


            // =========================
            // TOTAL FILTRADO
            // =========================
            int totalFiltrado = resultado.Count();


            // =========================
            // PAGINACIÓN
            // =========================
            var pagina = resultado
                .Skip(request.Start)
                .Take(request.Length)
                .ToList();


            // =========================
            // RESPONSE DATATABLE
            // =========================
            return new DataTableResponseVM<ProyeccionGridVM>
            {
                Draw = request.Draw,

                RecordsTotal = datos.Count,

                RecordsFiltered = totalFiltrado,

                Data = pagina
            };
        }

        public async Task GuardarCelda(ProyeccionUpdateDto dto)
        {
            // =========================
            //  1. CONVERTIR SEMANA A FECHA
            // =========================
            var fechaInicioSemana = ISOWeek.ToDateTime(dto.Anio, dto.Semana, DayOfWeek.Monday);

            // =========================
            //  2. NORMALIZAR
            // =========================
            var codDoc = dto.CodDoc.Trim();
            var numDoc = dto.NumDoc.Trim();
            var numIte = dto.NumIte.Trim();

            // =========================
            //  3. BUSCAR EXISTENTE
            // =========================
            var existente = await _context.MovHis00
                .Where(x =>
                    x.CodDoc.Trim() == codDoc &&
                    x.NumDoc.Trim() == numDoc &&
                    x.NumIte.Trim() == numIte &&
                    x.FecIniPro.HasValue &&
                    x.FecIniPro.Value.Date == fechaInicioSemana.Date)
                .FirstOrDefaultAsync();

            if (existente != null)
            {
                // =========================
                //  UPDATE
                // =========================
                existente.CantidadProyectada = dto.Cantidad;
                existente.Mod3 = "OT01PCP001";
            }
            else
            {
                // =========================
                //  INSERT
                // =========================
                var nuevo = new MovHis00
                {
                    CodEmp = "E1",
                    CodDoc = codDoc,
                    NumDoc = numDoc,
                    NumIte = numIte,

                    FechaHis = DateTime.Now, // evento
                    FecIniPro = fechaInicioSemana, // fechaProgramacion

                    CantidadProyectada = dto.Cantidad,

                    TipEve = "OT01PCP001",
                    Mod0 = "ProyectPCP",
                    Mod3 = "OT01PCP001",
                    NomPc = System.Environment.MachineName
                };

                _context.MovHis00.Add(nuevo);
            }

            await _context.SaveChangesAsync();
        }


        public async Task GuardarLote(List<ProyeccionUpdateDto> lista)
        {
            foreach (var item in lista)
            {
                var fecha = ISOWeek.ToDateTime(item.Anio, item.Semana, DayOfWeek.Monday);

                var existente = await _context.MovHis00.FirstOrDefaultAsync(x =>
                    x.CodDoc == item.CodDoc &&
                    x.NumDoc == item.NumDoc &&
                    x.NumIte == item.NumIte &&
                    x.FecIniPro >= fecha.Date &&
                    x.FecIniPro < fecha.Date.AddDays(1)
                );

                if (existente != null)
                {
                    existente.CantidadProyectada = item.Cantidad;
                }
                else
                {
                    _context.MovHis00.Add(new MovHis00
                    {
                        CodEmp = "E1",
                        CodDoc = item.CodDoc,
                        NumDoc = item.NumDoc,
                        NumIte = item.NumIte,
                        NumIte1 = "",
                        FecIniPro = fecha,
                        CantidadProyectada = item.Cantidad,
                        FechaHis = DateTime.Now,
                        DesHis = "PLANIFICACION del doc. OT01 - " + item.NumDoc,
                        TipEve = "OT01PCP001",
                        NumCor = 46,
                        ActInc = true,
                        DesEve = "PLANIFICACION2.0",
                        EstEve = "PLANIFICACION",
                        Mod0 = "ProyectPCP",
                        Mod1 = "",
                        Mod3 = "OT01PCP001",
                        NomPc = System.Environment.MachineName
                    });
                }
            }

            await _context.SaveChangesAsync();


        }
    }
}
