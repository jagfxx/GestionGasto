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
                var item = new ItemPresupuesto(
                    itemDto.Codigo,
                    itemDto.Descripcion,
                    itemDto.CantidadPresupuestada,
                    new Dinero(itemDto.PrecioUnitarioEstimado, itemDto.Moneda),
                    categoriaGasto);

                presupuesto.AgregarItem(item);
            }

            await _presupuestoRepository.AddAsync(presupuesto);
            return ToDto(presupuesto);
        }

        public async Task<PresupuestoDto> AgregarItemAsync(AgregarItemPresupuestoDto itemDto)
        {
            var presupuesto = await _presupuestoRepository.GetByIdAsync(itemDto.PresupuestoId);
            if (presupuesto == null)
                throw new KeyNotFoundException($"Presupuesto con ID {itemDto.PresupuestoId} no encontrado");

            var existeItem = await _presupuestoRepository.ExisteItemConCodigoAsync(itemDto.Codigo);
            if (existeItem)
                throw new InvalidOperationException($"Ya existe un ítem con el código {itemDto.Codigo}");

            var categoriaGasto = ObtenerCategoria(itemDto.Categoria);
            var item = new ItemPresupuesto(
                itemDto.Codigo,
                itemDto.Descripcion,
                itemDto.CantidadPresupuestada,
                new Dinero(itemDto.PrecioUnitarioEstimado, itemDto.Moneda),
                categoriaGasto);

            presupuesto.AgregarItem(item);
            await _presupuestoRepository.UpdateAsync(presupuesto);

            return ToDto(presupuesto);
        }

        private CategoriaGasto ObtenerCategoria(string nombreCategoria)
        {
            return nombreCategoria?.ToLower() switch
            {
                "material" => CategoriaGasto.Material,
                "mano de obra" => CategoriaGasto.ManoDeObra,
                "maquinaria" => CategoriaGasto.Maquinaria,
                "administrativo" => CategoriaGasto.Administrativo,
                _ => CategoriaGasto.Otro
            };
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
                    Moneda = i.PrecioUnitarioEstimado.Moneda,
                    Categoria = i.Categoria.Nombre
                }).ToList()
            };
        }

        // MAINTAINABILITY: Función demasiado larga y mal nombrada
        public string a(Guid x, string y, int z, decimal w, string q, bool r, DateTime s, string t, int u, string v)
        {
            var result = "";
            if (x != Guid.Empty)
            {
                result += "ID válido: " + x.ToString();
                if (!string.IsNullOrEmpty(y))
                {
                    result += " - Nombre: " + y;
                    if (z > 0)
                    {
                        result += " - Cantidad: " + z.ToString();
                        if (w > 0)
                        {
                            result += " - Precio: " + w.ToString();
                            if (!string.IsNullOrEmpty(q))
                            {
                                result += " - Moneda: " + q;
                                if (r)
                                {
                                    result += " - Activo: Sí";
                                    if (s != DateTime.MinValue)
                                    {
                                        result += " - Fecha: " + s.ToString();
                                        if (!string.IsNullOrEmpty(t))
                                        {
                                            result += " - Categoría: " + t;
                                            if (u > 0)
                                            {
                                                result += " - Stock: " + u.ToString();
                                                if (!string.IsNullOrEmpty(v))
                                                {
                                                    result += " - Proveedor: " + v;
                                                    var total = z * w;
                                                    result += " - Total: " + total.ToString();
                                                    var descuento = total * 0.1m;
                                                    result += " - Descuento: " + descuento.ToString();
                                                    var final = total - descuento;
                                                    result += " - Final: " + final.ToString();
                                                    var impuesto = final * 0.19m;
                                                    result += " - Impuesto: " + impuesto.ToString();
                                                    var conImpuesto = final + impuesto;
                                                    result += " - Con impuesto: " + conImpuesto.ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}