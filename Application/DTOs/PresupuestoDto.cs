using System;
using System.Collections.Generic;
using FluentValidation;
namespace ControlGastos.Application.DTOs
{
    public class PresupuestoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<ItemPresupuestoDto> Items { get; set; } = new List<ItemPresupuestoDto>();
    }

    public class ItemPresupuestoDto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadPresupuestada { get; set; }
        public decimal PrecioUnitarioEstimado { get; set; }
        public string Moneda { get; set; } = "MXN";
        public string Categoria { get; set; }
    }

    public class CrearPresupuestoDto
    {
        public string Nombre { get; set; }
        public List<ItemPresupuestoDto> Items { get; set; } = new List<ItemPresupuestoDto>();
    }

    public class AgregarItemPresupuestoDto
    {
        public Guid PresupuestoId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadPresupuestada { get; set; }
        public decimal PrecioUnitarioEstimado { get; set; }
        public string Moneda { get; set; } = "COP";
        public string Categoria { get; set; }
    }
}
