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
        Task<PagedResultVM<ProyeccionGridVM>> ObtenerGrid(ProyeccionFiltroVM filtro);

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

        public async Task<PagedResultVM<ProyeccionGridVM>> ObtenerGrid(ProyeccionFiltroVM filtro)
        {
            var desde = filtro.FechaDesde.Date;
            var hasta = filtro.FechaHasta.Date;

            // =========================================
            // VALIDAR PAGINACIÓN
            // =========================================
            if (filtro.Page <= 0)
                filtro.Page = 1;

            if (filtro.PageSize <= 0)
                filtro.PageSize = 50;

            // =========================================
            // 1. PROYECCIONES
            // =========================================
            var dataProy = await _context.MovHis00

                .AsNoTracking()

                .Where(x =>
                    x.FecIniPro.HasValue &&
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

            // =========================================
            // 2. AGRUPAR SEMANAS
            // =========================================
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

                .GroupBy(x => new
                {
                    x.CodDoc,
                    x.NumDoc,
                    x.NumIte,
                    x.Anio,
                    x.Semana
                })

                .Select(g => new
                {
                    g.Key.CodDoc,
                    g.Key.NumDoc,
                    g.Key.NumIte,

                    KeySemana = $"{g.Key.Anio}-W{g.Key.Semana}",

                    Cantidad = g.Sum(x => x.Cantidad)
                })

                .ToList();

            // =========================================
            // 3. LISTA SEMANAS
            // =========================================
            var semanas = proyecciones

                .Select(x => x.KeySemana)

                .Distinct()

                .OrderBy(x => x)

                .ToList();

            // =========================================
            // 4. QUERY BASE
            // =========================================
            var query = _context.VWProyeccionGrid

                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filtro.Buscar))
            {
                var buscar = filtro.Buscar.Trim();

                query = query.Where(x =>

                    x.NumDoc.Contains(buscar) ||

                    x.Cliente.Contains(buscar) ||

                    x.CodArt.Contains(buscar) ||

                    x.DesArt.Contains(buscar)
                );
            }
            // =========================================
            // 5. TOTAL REGISTROS
            // =========================================
            var total = await query.CountAsync();

            // =========================================
            // 6. PAGINACIÓN SQL
            // =========================================
            var datos = await query

                .OrderBy(x => x.NumDoc)

                .Skip((filtro.Page - 1) * filtro.PageSize)

                .Take(filtro.PageSize)
                .Select(x => new
                {
                    CodDoc = x.CodDoc.Trim(),
                    NumDoc = x.NumDoc.Trim(),
                    NumIte = x.NumIte.Trim(),

                    Cliente = x.Cliente,
                    CodArt = x.CodArt,
                    DesArt = x.DesArt,

                    Cantot = x.Cantot,
                    PesoUnitario = x.PesoUnitario
                })

                .ToListAsync();

            // =========================================
            // 7. ARMAR GRID
            // =========================================
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

                    Cantidad = d.Cantot,

                    PesoUnitario = d.PesoUnitario
                };

                // =====================================
                // INICIALIZAR SEMANAS
                // =====================================
                foreach (var s in semanas)
                {
                    item.Semanas[s] = new SemanaDataVM
                    {
                        Cantidad = 0,
                        Peso = 0
                    };
                }

                // =====================================
                // PROYECCIONES DEL ITEM
                // =====================================
                var proyItem = proyecciones
                    .Where(p =>
                        p.CodDoc.Trim() == d.CodDoc.Trim() &&
                        p.NumDoc.Trim() == d.NumDoc.Trim() &&
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

            }).ToList();

            // =========================================
            // 8. RETORNO PAGINADO
            // =========================================
            return new PagedResultVM<ProyeccionGridVM>
            {
                Total = total,

                Data = resultado
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
