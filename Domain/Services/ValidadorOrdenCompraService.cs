using ControlGastos.Domain.Entities;
using ControlGastos.Domain.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ControlGastos.Domain.Services
{
    public class ValidadorOrdenCompraService
    {
        public class ResultadoValidacion
        {
            public bool EsValida { get; set; }
            public string Mensaje { get; set; }
        }

        public ResultadoValidacion ValidarOrdenCompra(OrdenCompra ordenCompra, Presupuesto presupuesto)
        {
            if (ordenCompra == null)
                throw new ArgumentNullException(nameof(ordenCompra));

            if (presupuesto == null)
                throw new ArgumentNullException(nameof(presupuesto));

            // Validar cada ítem en la orden de compra contra el presupuesto
            foreach (var item in ordenCompra.Items)
            {
                if (!presupuesto.ValidarCantidadItem(item.Codigo, item.Cantidad))
                {
                    var itemPresupuesto = presupuesto.Items.FirstOrDefault(i => i.Codigo == item.Codigo);
                    int cantidadDisponible = itemPresupuesto?.CantidadPresupuestada ?? 0;

                    return new ResultadoValidacion
                    {
                        EsValida = false,
                        Mensaje = $"El ítem {item.Codigo} excede la cantidad presupuestada. Solicitado: {item.Cantidad}, Disponible: {cantidadDisponible}"
                    };
                }
            }

            // Si todos los ítems son válidos
            return new ResultadoValidacion
            {
                EsValida = true,
                Mensaje = "Orden de compra válida"
            };
        }

        public OrdenCompraValidadaEvent GenerarEventoValidacion(OrdenCompra ordenCompra, ResultadoValidacion resultado)
        {
            return new OrdenCompraValidadaEvent
            {
                OrdenCompraId = ordenCompra.Id,
                FechaValidacion = DateTime.Now,
                EsValida = resultado.EsValida,
                Mensaje = resultado.Mensaje
            };
        }
    }
}