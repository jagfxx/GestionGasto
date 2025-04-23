using ControlGastos.Domain.ValueObjects;
using System;

namespace ControlGastos.Domain.Entities
{
    public class ItemPresupuesto
    {
        public Guid Id { get; private set; }
        public string Codigo { get; private set; }
        public string Descripcion { get; private set; }
        public int CantidadPresupuestada { get; private set; }
        public Dinero PrecioUnitarioEstimado { get; private set; }
        public CategoriaGasto Categoria { get; private set; }

        public ItemPresupuesto(string codigo, string descripcion, int cantidadPresupuestada,
                          Dinero precioUnitarioEstimado, CategoriaGasto categoria)
        {
            Id = Guid.NewGuid();
            Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            CantidadPresupuestada = cantidadPresupuestada > 0 ? cantidadPresupuestada :
                throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(cantidadPresupuestada));
            PrecioUnitarioEstimado = precioUnitarioEstimado ?? throw new ArgumentNullException(nameof(precioUnitarioEstimado));
            Categoria = categoria ?? throw new ArgumentNullException(nameof(categoria));
        }
    }
}