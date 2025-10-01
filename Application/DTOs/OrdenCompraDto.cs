using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ControlGastos.Application.DTOs
{
    public class OrdenCompraDto
    {
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "El número de orden es requerido")]
        [StringLength(50, ErrorMessage = "El número no puede exceder los 50 caracteres")]
        public string Numero { get; set; } = string.Empty;
        
        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
        
        [Required(ErrorMessage = "El nombre del solicitante es requerido")]
        [StringLength(100, ErrorMessage = "El nombre del solicitante no puede exceder los 100 caracteres")]
        public string Solicitante { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El nombre del proveedor es requerido")]
        [StringLength(100, ErrorMessage = "El nombre del proveedor no puede exceder los 100 caracteres")]
        public string Proveedor { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Debe incluir al menos un ítem en la orden")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un ítem en la orden")]
        public List<ItemOrdenCompraDto> Items { get; set; } = new List<ItemOrdenCompraDto>();
        
        [Required(ErrorMessage = "El estado es requerido")]
        public string Estado { get; set; } = "Pendiente";
        
        public string? MotivoRechazo { get; set; }
    }

    public class ItemOrdenCompraDto
    {
        [Required(ErrorMessage = "El código del ítem es requerido")]
        [StringLength(50, ErrorMessage = "El código no puede exceder los 50 caracteres")]
        public string Codigo { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres")]
        public string Descripcion { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; } = 1;
        
        [Required(ErrorMessage = "El precio unitario es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal PrecioUnitario { get; set; } = 0.01m;
        
        [Required(ErrorMessage = "La moneda es requerida")]
        public string Moneda { get; set; } = "COP";
    }

    public class CrearOrdenCompraDto
    {
        [Required(ErrorMessage = "El número de orden es requerido")]
        [StringLength(50, ErrorMessage = "El número no puede exceder los 50 caracteres")]
        public string Numero { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El nombre del solicitante es requerido")]
        [StringLength(100, ErrorMessage = "El nombre del solicitante no puede exceder los 100 caracteres")]
        public string Solicitante { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El nombre del proveedor es requerido")]
        [StringLength(100, ErrorMessage = "El nombre del proveedor no puede exceder los 100 caracteres")]
        public string Proveedor { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Debe incluir al menos un ítem en la orden")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un ítem en la orden")]
        public List<ItemOrdenCompraDto> Items { get; set; } = new List<ItemOrdenCompraDto>();
    }
}
