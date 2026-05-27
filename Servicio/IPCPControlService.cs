using PCPProyect.ViewModel;

namespace PCPProyect.Servicio
{
    public interface IPCPControlService
    {
        Task<PagedResultVM<PCPControlGridVM>> ObtenerGrid(PCPControlFiltroVM filtro);
    }
}
