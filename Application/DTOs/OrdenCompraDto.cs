using System;
using System.Collections.Generic;

namespace ControlGastos.Application.DTOs
{
    public class OrdenCompraDto
    {
        public Guid Id { get; set; }
        public string Numero { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Solicitante { get; set; }
        public string Proveedor { get; set; }
        public List<ItemOrdenCompraDto> Items { get; set; } = new List<ItemOrdenCompraDto>();
        public string Estado { get; set; }
        public string MotivoRechazo { get; set; }
    }

    public class ItemOrdenCompraDto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string Moneda { get; set; } = "COP";
    }

    public class CrearOrdenCompraDto
    {
        public string Numero { get; set; }
        public string Solicitante { get; set; }
        public string Proveedor { get; set; }
        public List<ItemOrdenCompraDto> Items { get; set; } = new List<ItemOrdenCompraDto>();
    }
}
