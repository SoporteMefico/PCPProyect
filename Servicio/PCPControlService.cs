using Microsoft.EntityFrameworkCore;
using PCPProyect.Datos;
using PCPProyect.ViewModel;

namespace PCPProyect.Servicio
{
    public class PCPControlService : IPCPControlService
    {
        private readonly ApplicationDbContext _context;

        public PCPControlService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultVM<PCPControlGridVM>> ObtenerGrid(PCPControlFiltroVM filtro)
        {
            filtro ??= new PCPControlFiltroVM();

            var query = _context.VW_PCP_CONTROL
                .AsNoTracking()
                .AsQueryable();

            // filtros
            if (filtro != null && filtro.FechaDesde.HasValue)
            {
                query = query.Where(x =>
                    x.FechaEntrega.HasValue &&
                    x.FechaEntrega.Value >= filtro.FechaDesde.Value);
            }

            if (filtro != null && filtro.FechaHasta.HasValue)
            {
                query = query.Where(x =>
                    x.FechaEntrega.HasValue &&
                    x.FechaEntrega.Value <= filtro.FechaHasta.Value);
            }

            // =====================================
            // ESTADO OT
            // =====================================

            if (!string.IsNullOrWhiteSpace(filtro.EstadoOT))
            {
                query = query.Where(x =>
                    x.EstadoOT == filtro.EstadoOT);
            }

            // =====================================
            // ESTADO PCP
            // =====================================

            if (!string.IsNullOrWhiteSpace(filtro.EstadoPCP))
            {
                query = query.Where(x =>
                    x.EstadoPCP == filtro.EstadoPCP);
            }

            // =====================================
            // ATRASADO
            // =====================================

            if ((filtro.Atrasado))
            {
                bool atrasado =
                    filtro.Atrasado == true;

                query = query.Where(x =>
                    x.Atrasado == atrasado);
            }

            if (!string.IsNullOrWhiteSpace(filtro.Buscar))
            {
                string b = filtro.Buscar.Trim();

                query = query.Where(x =>

                    x.NumDoc.Contains(b) ||

                    x.Cliente.Contains(b) ||

                    x.CodArt.Contains(b) ||

                    x.DesArt.Contains(b));
            }

            var total = await query.CountAsync();

            var data = await query

                .OrderBy(x => x.FechaEntrega)

                .Skip((filtro.Page - 1) * filtro.PageSize)

                .Take(filtro.PageSize)

                .Select(x => new PCPControlGridVM
                {
                    CodDoc = x.CodDoc,
                    NumDoc = x.NumDoc,
                    Cliente = x.Cliente,
                    Pedido = x.Pedido,
                    CodArt = x.CodArt,
                    DesArt = x.DesArt,
                    Cantidad = x.Cantidad,
                    EstadoOT = x.EstadoOT,
                    EstadoPCP = x.EstadoPCP,
                    TotalFabricado = x.TotalFabricado,
                    TotalProyectado = x.TotalProyectado,
                    FechaEntrega = x.FechaEntrega,
                    Atrasado = x.Atrasado
                })

                .ToListAsync();

            return new PagedResultVM<PCPControlGridVM>
            {
                Total = total,
                Data = data
            };
        }
    }
}
