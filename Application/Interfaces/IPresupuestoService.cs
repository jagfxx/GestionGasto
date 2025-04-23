using ControlGastos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlGastos.Application.Interfaces
{
    public interface IPresupuestoService
    {
        Task<List<PresupuestoDto>> GetAllAsync();
        Task<PresupuestoDto> GetByIdAsync(Guid id);
        Task<PresupuestoDto> CreateAsync(CrearPresupuestoDto presupuestoDto);
        Task<PresupuestoDto> AgregarItemAsync(AgregarItemPresupuestoDto itemDto);
    }
}
