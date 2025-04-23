using ControlGastos.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ControlGastos.Domain.Entities
{
    public class Factura
    {
        public Guid Id { get; private set; }
        public string Numero { get; private set; }
        public DateTime Fecha { get; private set; }
        public Dinero Total { get; private set; }
        public string Proveedor { get; private set; }
        public List<Guid> GastosIds { get; private set; }

        public Factura(string numero, DateTime fecha, Dinero total, string proveedor)
        {
            Id = Guid.NewGuid();
            Numero = numero ?? throw new ArgumentNullException(nameof(numero));
            Fecha = fecha;
            Total = total ?? throw new ArgumentNullException(nameof(total));
            Proveedor = proveedor ?? throw new ArgumentNullException(nameof(proveedor));
            GastosIds = new List<Guid>();
        }

        public void AgregarGasto(Guid gastoId)
        {
            GastosIds.Add(gastoId);
        }
    }
}