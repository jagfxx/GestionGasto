using ControlGastos.Application.DTOs;
using ControlGastos.Application.Interfaces;
using ControlGastos.Domain.Entities;
using ControlGastos.Domain.Interfaces;
using ControlGastos.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlGastos.Application.Services
{
    public class PresupuestoService : IPresupuestoService
    {
        private readonly IPresupuestoRepository _presupuestoRepository;

        public PresupuestoService(IPresupuestoRepository presupuestoRepository)
        {
            _presupuestoRepository = presupuestoRepository;
        }

        public async Task<List<PresupuestoDto>> GetAllAsync()
        {
            var presupuestos = await _presupuestoRepository.GetAllAsync();
            return presupuestos.Select(ToDto).ToList();
        }

        public async Task<PresupuestoDto> GetByIdAsync(Guid id)
        {
            var presupuesto = await _presupuestoRepository.GetByIdAsync(id);
            return presupuesto != null ? ToDto(presupuesto) : null;
        }

        public async Task<PresupuestoDto> CreateAsync(CrearPresupuestoDto presupuestoDto)
        {
            var presupuesto = new Presupuesto(presupuestoDto.Nombre);

            foreach (var itemDto in presupuestoDto.Items)
            {
                var categoriaGasto = ObtenerCategoria(itemDto.Categoria);
                // Ensure required values are not null before creating the item
                if (!itemDto.CantidadPresupuestada.HasValue || !itemDto.PrecioUnitarioEstimado.HasValue)
                {
                    throw new InvalidOperationException("Cantidad y precio unitario son requeridos");
                }

                var item = new ItemPresupuesto(
                    itemDto.Codigo ?? throw new ArgumentNullException(nameof(itemDto.Codigo)),
                    itemDto.Descripcion ?? throw new ArgumentNullException(nameof(itemDto.Descripcion)),
                    itemDto.CantidadPresupuestada.Value,
                    new Dinero(itemDto.PrecioUnitarioEstimado.Value, itemDto.Moneda ?? "MXN"),
                    categoriaGasto);

                presupuesto.AgregarItem(item);
            }

            await _presupuestoRepository.AddAsync(presupuesto);
            return ToDto(presupuesto);
        }

        public async Task<PresupuestoDto> AgregarItemAsync(AgregarItemPresupuestoDto itemDto)
        {
            if (itemDto == null) throw new ArgumentNullException(nameof(itemDto));
            if (!itemDto.PresupuestoId.HasValue) throw new ArgumentException("El ID del presupuesto es requerido", nameof(itemDto.PresupuestoId));
            
            var presupuesto = await _presupuestoRepository.GetByIdAsync(itemDto.PresupuestoId.Value);
            if (presupuesto == null)
                throw new KeyNotFoundException($"Presupuesto con ID {itemDto.PresupuestoId} no encontrado");

            if (itemDto == null)
                throw new ArgumentNullException(nameof(itemDto));

            if (string.IsNullOrWhiteSpace(itemDto.Codigo))
                throw new ArgumentException("El código del ítem es requerido", nameof(itemDto.Codigo));
                
            if (string.IsNullOrWhiteSpace(itemDto.Descripcion))
                throw new ArgumentException("La descripción del ítem es requerida", nameof(itemDto.Descripcion));
                
            if (itemDto.CantidadPresupuestada <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(itemDto.CantidadPresupuestada));
                
            if (itemDto.PrecioUnitarioEstimado <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor a cero", nameof(itemDto.PrecioUnitarioEstimado));

            var existeItem = await _presupuestoRepository.ExisteItemConCodigoAsync(itemDto.Codigo);
            if (existeItem)
                throw new InvalidOperationException($"Ya existe un ítem con el código {itemDto.Codigo}");

            var categoriaGasto = ObtenerCategoria(itemDto.Categoria);
            
            var item = new ItemPresupuesto(
                itemDto.Codigo,
                itemDto.Descripcion,
                itemDto.CantidadPresupuestada,
                new Dinero(itemDto.PrecioUnitarioEstimado, itemDto.Moneda ?? "MXN"),
                categoriaGasto);

            await _presupuestoRepository.UpdateAsync(presupuesto);

            return ToDto(presupuesto);
        }

        private CategoriaGasto ObtenerCategoria(string? nombreCategoria)
        {
            if (string.IsNullOrWhiteSpace(nombreCategoria))
            {
                throw new ArgumentException("El nombre de la categoría es requerido", nameof(nombreCategoria));
            }
            
            var categoria = nombreCategoria.Trim().ToLower() switch
            {
                "material" => CategoriaGasto.Material,
                "mano de obra" or "manodeobra" => CategoriaGasto.ManoDeObra,
                "maquinaria" => CategoriaGasto.Maquinaria,
                "administrativo" or "gastos administrativos" => CategoriaGasto.Administrativo,
                _ => throw new ArgumentException($"Categoría '{nombreCategoria}' no válida. Las categorías válidas son: Material, Mano de obra, Maquinaria, Administrativo", nameof(nombreCategoria))
            };
            
            return categoria;
        }

        private PresupuestoDto ToDto(Presupuesto presupuesto)
        {
            return new PresupuestoDto
            {
                Id = presupuesto.Id,
                Nombre = presupuesto.Nombre,
                FechaCreacion = presupuesto.FechaCreacion,
                Items = presupuesto.Items.Select(i => new ItemPresupuestoDto
                {
                    Codigo = i.Codigo,
                    Descripcion = i.Descripcion,
                    CantidadPresupuestada = i.CantidadPresupuestada,
                    PrecioUnitarioEstimado = i.PrecioUnitarioEstimado.Valor,
                    Categoria = i.Categoria.Nombre
                }).ToList()
            };
        }

        /// <summary>
        /// Genera un resumen detallado de un presupuesto con cálculos financieros
        /// </summary>
        /// <param name="presupuestoId">Identificador único del presupuesto</param>
        /// <param name="nombre">Nombre del presupuesto</param>
        /// <param name="cantidad">Cantidad de unidades</param>
        /// <param name="precioUnitario">Precio por unidad</param>
        /// <param name="moneda">Tipo de moneda</param>
        /// <param name="estaActivo">Indica si el presupuesto está activo</param>
        /// <param name="fecha">Fecha del presupuesto</param>
        /// <param name="categoria">Categoría del presupuesto</param>
        /// <param name="stock">Cantidad de stock</param>
        /// <param name="proveedor">Nombre del proveedor</param>
        /// <returns>Un string con la información del presupuesto</returns>
        private string GenerarResumenDetallado(
            Guid presupuestoId,
            string nombre,
            int cantidad,
            decimal precioUnitario,
            string moneda,
            bool estaActivo,
            DateTime fecha,
            string categoria,
            int stock,
            string proveedor)
        {
            // Validaciones iniciales
            if (presupuestoId == Guid.Empty) return "ID de presupuesto no válido";
            if (string.IsNullOrEmpty(nombre)) return "Nombre de presupuesto requerido";
            if (cantidad <= 0) return "La cantidad debe ser mayor a cero";
            if (precioUnitario <= 0) return "El precio unitario debe ser mayor a cero";
            if (string.IsNullOrEmpty(moneda)) return "Moneda requerida";
            if (fecha == DateTime.MinValue) return "Fecha no válida";
            if (string.IsNullOrEmpty(categoria)) return "Categoría requerida";
            if (stock <= 0) return "Stock debe ser mayor a cero";
            if (string.IsNullOrEmpty(proveedor)) return "Proveedor requerido";

            // Cálculos financieros
            var total = CalcularTotal(cantidad, precioUnitario);
            var descuento = CalcularDescuento(total);
            var subtotal = total - descuento;
            var impuesto = CalcularImpuesto(subtotal);
            var totalConImpuesto = subtotal + impuesto;

            // Construcción del resumen
            var resumen = new List<string>
            {
                $"ID: {presupuestoId}",
                $"Nombre: {nombre}",
                $"Cantidad: {cantidad}",
                $"Precio Unitario: {precioUnitario} {moneda}",
                $"Estado: {(estaActivo ? "Activo" : "Inactivo")}",
                $"Fecha: {fecha:dd/MM/yyyy}",
                $"Categoría: {categoria}",
                $"Stock: {stock}",
                $"Proveedor: {proveedor}",
                $"Subtotal: {total} {moneda}",
                $"Descuento (10%): {descuento} {moneda}",
                $"Base Imponible: {subtotal} {moneda}",
                $"Impuesto (19%): {impuesto} {moneda}",
                $"TOTAL: {totalConImpuesto} {moneda}"
            };

            return string.Join(Environment.NewLine, resumen);
        }

        private decimal CalcularTotal(int cantidad, decimal precioUnitario)
        {
            return cantidad * precioUnitario;
        }

        private decimal CalcularDescuento(decimal monto)
        {
            return monto * 0.1m; // 10% de descuento
        }

        private decimal CalcularImpuesto(decimal monto)
        {
            return monto * 0.19m; // 19% de IVA
        }
    }
}