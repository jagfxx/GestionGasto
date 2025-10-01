using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Codigo { get; set; }
        
        [Required]
        public string Descripcion { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int CantidadPresupuestada { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal PrecioUnitarioEstimado { get; set; }
        
        [Required]
        public string Moneda { get; set; } = "MXN";
        
        [Required]
        public string Categoria { get; set; }
    }

    public class CrearPresupuestoDto
    {
        [Required(ErrorMessage = "El nombre del presupuesto es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "Debe incluir al menos un ítem en el presupuesto")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un ítem en el presupuesto")]
        public List<ItemPresupuestoDto> Items { get; set; } = new List<ItemPresupuestoDto>();
    }

    public class AgregarItemPresupuestoDto
    {
        [Required(ErrorMessage = "El ID del presupuesto es requerido")]
        public Guid PresupuestoId { get; set; }
        
        [Required(ErrorMessage = "El código es requerido")]
        public string Codigo { get; set; }
        
        [Required(ErrorMessage = "La descripción es requerida")]
        public string Descripcion { get; set; }
        
        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int CantidadPresupuestada { get; set; }
        
        [Required(ErrorMessage = "El precio unitario es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal PrecioUnitarioEstimado { get; set; }
        
        [Required(ErrorMessage = "La moneda es requerida")]
        public string Moneda { get; set; } = "COP";
        
        [Required(ErrorMessage = "La categoría es requerida")]
        public string Categoria { get; set; }
    }
}
