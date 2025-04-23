// ControlGastos.Infrastructure/Repositories/PresupuestoRepository.cs
using ControlGastos.Domain.Entities;
using ControlGastos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlGastos.Infrastructure.Repositories
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        // Para simplificar, usamos una lista en memoria para esta primera versión
        private static readonly List<Presupuesto> _presupuestos = new List<Presupuesto>();

        public Task<Presupuesto> GetByIdAsync(Guid id)
        {
            var presupuesto = _presupuestos.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(presupuesto);
        }

        public Task<List<Presupuesto>> GetAllAsync()
        {
            return Task.FromResult(_presupuestos.ToList());
        }

        public Task AddAsync(Presupuesto presupuesto)
        {
            _presupuestos.Add(presupuesto);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Presupuesto presupuesto)
        {
            var index = _presupuestos.FindIndex(p => p.Id == presupuesto.Id);
            if (index != -1)
            {
                _presupuestos[index] = presupuesto;
            }
            return Task.CompletedTask;
        }

        public Task<bool> ExisteItemConCodigoAsync(string codigo)
        {
            var existe = _presupuestos
                .SelectMany(p => p.Items)
                .Any(i => i.Codigo == codigo);

            return Task.FromResult(existe);
        }
    }
}
