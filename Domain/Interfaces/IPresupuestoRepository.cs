using ControlGastos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Interfaces
{
    public interface IPresupuestoRepository
    {
        Task<Presupuesto> GetByIdAsync(Guid id);
        Task<List<Presupuesto>> GetAllAsync();
        Task AddAsync(Presupuesto presupuesto);
        Task UpdateAsync(Presupuesto presupuesto);
        Task<bool> ExisteItemConCodigoAsync(string codigo);
    }
}
