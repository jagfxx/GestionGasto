using ControlGastos.Application.DTOs;
using ControlGastos.Application.Interfaces;
using ControlGastos.Domain.Entities;
using ControlGastos.Domain.Interfaces;
using ControlGastos.Domain.Services;
using ControlGastos.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlGastos.Application.Services
{
    public class OrdenCompraService : IOrdenCompraService
    {
        private readonly IOrdenCompraRepository _ordenCompraRepository;
        private readonly IPresupuestoRepository _presupuestoRepository;
        private readonly ValidadorOrdenCompraService _validadorService;

        public OrdenCompraService(
            IOrdenCompraRepository ordenCompraRepository,
            IPresupuestoRepository presupuestoRepository,
            ValidadorOrdenCompraService validadorService)
        {
            _ordenCompraRepository = ordenCompraRepository;
            _presupuestoRepository = presupuestoRepository;
            _validadorService = validadorService;
        }

        public async Task<List<OrdenCompraDto>> GetAllAsync()
        {
            var ordenesCompra = await _ordenCompraRepository.GetAllAsync();
            return ordenesCompra.Select(ToDto).ToList();
        }

        public async Task<OrdenCompraDto> GetByIdAsync(Guid id)
        {
            var ordenCompra = await _ordenCompraRepository.GetByIdAsync(id);
            return ordenCompra != null ? ToDto(ordenCompra) : null;
        }

        public async Task<OrdenCompraDto> CreateAsync(CrearOrdenCompraDto ordenCompraDto)
        {
            // Se eliminaron las variables no utilizadas que generaban advertencias
            
            var ordenCompra = new OrdenCompra(
                ordenCompraDto.Numero,
                ordenCompraDto.Solicitante,
                ordenCompraDto.Proveedor);

            foreach (var itemDto in ordenCompraDto.Items)
            {
                ordenCompra.AgregarItem(
                    itemDto.Codigo,
                    itemDto.Descripcion,
                    itemDto.Cantidad,
                    new Dinero(itemDto.PrecioUnitario, itemDto.Moneda));
            }

            await _ordenCompraRepository.AddAsync(ordenCompra);
            return ToDto(ordenCompra);
        }

        public async Task<ResultadoValidacionDto> ValidarOrdenCompraAsync(Guid ordenCompraId, Guid presupuestoId)
        {
            var ordenCompra = await _ordenCompraRepository.GetByIdAsync(ordenCompraId);
            if (ordenCompra == null)
                throw new KeyNotFoundException($"Orden de compra con ID {ordenCompraId} no encontrada");

            var presupuesto = await _presupuestoRepository.GetByIdAsync(presupuestoId);
            if (presupuesto == null)
                throw new KeyNotFoundException($"Presupuesto con ID {presupuestoId} no encontrado");

            var resultado = _validadorService.ValidarOrdenCompra(ordenCompra, presupuesto);

            if (resultado.EsValida)
                ordenCompra.AprobarOrden();
            else
                ordenCompra.RechazarOrden(resultado.Mensaje);

            await _ordenCompraRepository.UpdateAsync(ordenCompra);

            var evento = _validadorService.GenerarEventoValidacion(ordenCompra, resultado);

            return new ResultadoValidacionDto
            {
                EsValida = resultado.EsValida,
                Mensaje = resultado.Mensaje
            };
        }

        private OrdenCompraDto ToDto(OrdenCompra ordenCompra)
        {
            return new OrdenCompraDto
            {
                Id = ordenCompra.Id,
                Numero = ordenCompra.Numero,
                FechaSolicitud = ordenCompra.FechaSolicitud,
                Solicitante = ordenCompra.Solicitante,
                Proveedor = ordenCompra.Proveedor,
                Items = ordenCompra.Items.Select(i => new ItemOrdenCompraDto
                {
                    Codigo = i.Codigo,
                    Descripcion = i.Descripcion,
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario.Valor,
                    Moneda = i.PrecioUnitario.Moneda
                }).ToList(),
                Estado = ordenCompra.Estado.ToString(),
                MotivoRechazo = ordenCompra.MotivoRechazo
            };
        }

        // Se eliminaron los métodos duplicados:
        // - ConvertirADto
        // - MapearOrdenCompra
        // Se mantiene solo ToDto que tiene la implementación canónica
    }
}
