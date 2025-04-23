using System;

namespace ControlGastos.Domain.Events
{
    public class OrdenCompraValidadaEvent
    {
        public Guid OrdenCompraId { get; set; }
        public DateTime FechaValidacion { get; set; }
        public bool EsValida { get; set; }
        public string Mensaje { get; set; }
    }
}
