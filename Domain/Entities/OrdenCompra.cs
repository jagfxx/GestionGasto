using ControlGastos.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ControlGastos.Domain.Entities
{
    public class OrdenCompra
    {
        public Guid Id { get; private set; }
        public string Numero { get; private set; }
        public DateTime FechaSolicitud { get; private set; }
        public string Solicitante { get; private set; }
        public string Proveedor { get; private set; }
        public List<ItemOrdenCompra> Items { get; private set; }
        public EstadoOrdenCompra Estado { get; private set; }
        public string MotivoRechazo { get; private set; }

        public OrdenCompra(string numero, string solicitante, string proveedor)
        {
            Id = Guid.NewGuid();
            Numero = numero ?? throw new ArgumentNullException(nameof(numero));
            FechaSolicitud = DateTime.Now;
            Solicitante = solicitante ?? throw new ArgumentNullException(nameof(solicitante));
            Proveedor = proveedor ?? throw new ArgumentNullException(nameof(proveedor));
            Items = new List<ItemOrdenCompra>();
            Estado = EstadoOrdenCompra.Pendiente;
        }

        public void AgregarItem(string codigo, string descripcion, int cantidad, Dinero precioUnitario)
        {
            var item = new ItemOrdenCompra(codigo, descripcion, cantidad, precioUnitario);
            Items.Add(item);
        }

        public void AprobarOrden()
        {
            Estado = EstadoOrdenCompra.Aprobada;
        }

        public void RechazarOrden(string motivo)
        {
            Estado = EstadoOrdenCompra.Rechazada;
            MotivoRechazo = motivo;
        }

        public class ItemOrdenCompra
        {
            public string Codigo { get; private set; }
            public string Descripcion { get; private set; }
            public int Cantidad { get; private set; }
            public Dinero PrecioUnitario { get; private set; }

            public ItemOrdenCompra(string codigo, string descripcion, int cantidad, Dinero precioUnitario)
            {
                Codigo = codigo ?? throw new ArgumentNullException(nameof(codigo));
                Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
                Cantidad = cantidad > 0 ? cantidad : throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(cantidad));
                PrecioUnitario = precioUnitario ?? throw new ArgumentNullException(nameof(precioUnitario));
            }
        }
    }

    public enum EstadoOrdenCompra
    {
        Pendiente,
        Aprobada,
        Rechazada
    }
}