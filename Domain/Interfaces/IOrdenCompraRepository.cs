using ControlGastos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Interfaces
{
    public interface IOrdenCompraRepository
    {
        Task<OrdenCompra> GetByIdAsync(Guid id);
        Task<List<OrdenCompra>> GetAllAsync();
        Task AddAsync(OrdenCompra ordenCompra);
        Task UpdateAsync(OrdenCompra ordenCompra);
    }
}
