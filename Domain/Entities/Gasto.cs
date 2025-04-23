using ControlGastos.Domain.ValueObjects;
using System;

namespace ControlGastos.Domain.Entities
{
    public class Gasto
    {
        public Guid Id { get; private set; }
        public string Descripcion { get; private set; }
        public Dinero Monto { get; private set; }
        public CategoriaGasto Categoria { get; private set; }
        public DateTime FechaRegistro { get; private set; }
        public Guid? FacturaId { get; private set; }
        public Guid OrdenCompraId { get; private set; }

        public Gasto(string descripcion, Dinero monto, CategoriaGasto categoria, Guid ordenCompraId)
        {
            Id = Guid.NewGuid();
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            Monto = monto ?? throw new ArgumentNullException(nameof(monto));
            Categoria = categoria ?? throw new ArgumentNullException(nameof(categoria));
            FechaRegistro = DateTime.Now;
            OrdenCompraId = ordenCompraId;
        }

        public void AsociarFactura(Guid facturaId)
        {
            FacturaId = facturaId;
        }
    }
}
