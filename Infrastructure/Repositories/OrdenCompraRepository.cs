using ControlGastos.Domain.Entities;
using ControlGastos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlGastos.Infrastructure.Repositories
{
    public class OrdenCompraRepository : IOrdenCompraRepository
    {
        // Para simplificar, usamos una lista en memoria para esta primera versión
        private static readonly List<OrdenCompra> _ordenesCompra = new List<OrdenCompra>();

        public Task<OrdenCompra> GetByIdAsync(Guid id)
        {
            var ordenCompra = _ordenesCompra.FirstOrDefault(o => o.Id == id);
            return Task.FromResult(ordenCompra);
        }

        public Task<List<OrdenCompra>> GetAllAsync()
        {
            return Task.FromResult(_ordenesCompra.ToList());
        }

        public Task AddAsync(OrdenCompra ordenCompra)
        {
            _ordenesCompra.Add(ordenCompra);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(OrdenCompra ordenCompra)
        {
            var index = _ordenesCompra.FindIndex(o => o.Id == ordenCompra.Id);
            if (index != -1)
            {
                _ordenesCompra[index] = ordenCompra;
            }
            return Task.CompletedTask;
        }
    }
}
