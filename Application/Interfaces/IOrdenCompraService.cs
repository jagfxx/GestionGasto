using ControlGastos.Application.DTOs;
using System.Threading.Tasks;

namespace ControlGastos.Application.Interfaces
{
    public interface IOrdenCompraService
    {
        Task<List<OrdenCompraDto>> GetAllAsync();
        Task<OrdenCompraDto> GetByIdAsync(Guid id);
        Task<OrdenCompraDto> CreateAsync(CrearOrdenCompraDto ordenCompraDto);
        Task<ResultadoValidacionDto> ValidarOrdenCompraAsync(Guid ordenCompraId, Guid presupuestoId);
    }
}
