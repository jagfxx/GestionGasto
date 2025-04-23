using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlGastos.Domain.Entities
{
    public class Presupuesto
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public List<ItemPresupuesto> Items { get; private set; }

        public Presupuesto(string nombre)
        {
            Id = Guid.NewGuid();
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            FechaCreacion = DateTime.Now;
            Items = new List<ItemPresupuesto>();
        }

        public void AgregarItem(ItemPresupuesto item)
        {
            if (Items.Any(i => i.Codigo == item.Codigo))
            {
                throw new InvalidOperationException($"Ya existe un item con el código {item.Codigo}");
            }

            Items.Add(item);
        }

        public bool ValidarCantidadItem(string codigo, int cantidad)
        {
            var item = Items.FirstOrDefault(i => i.Codigo == codigo);
            if (item == null)
                return false;

            return item.CantidadPresupuestada >= cantidad;
        }
    }
}
