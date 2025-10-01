using ControlGastos.Application.DTOs;
using ControlGastos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ControlGastos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenCompraController : ControllerBase
    {
        private readonly IOrdenCompraService _ordenCompraService;

        public OrdenCompraController(IOrdenCompraService ordenCompraService)
        {
            _ordenCompraService = ordenCompraService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrdenCompraDto>>> GetAll()
        {
            var ordenesCompra = await _ordenCompraService.GetAllAsync();
            return Ok(ordenesCompra);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenCompraDto>> GetById(Guid id)
        {
            var ordenCompra = await _ordenCompraService.GetByIdAsync(id);
            if (ordenCompra == null)
                return NotFound();

            return Ok(ordenCompra);
        }

        [HttpPost]
        public async Task<ActionResult<OrdenCompraDto>> Create([FromBody] CrearOrdenCompraDto ordenCompraDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevaOrdenCompra = await _ordenCompraService.CreateAsync(ordenCompraDto);
            return CreatedAtAction(nameof(GetById), new { id = nuevaOrdenCompra.Id }, nuevaOrdenCompra);
        }

        [HttpPost("validar")]
        public async Task<ActionResult<ResultadoValidacionDto>> ValidarOrdenCompra([FromBody] ValidarOrdenCompraRequest request)
        {
            try
            {
                var resultado = await _ordenCompraService.ValidarOrdenCompraAsync(request.OrdenCompraId, request.PresupuestoId);
                return Ok(resultado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class ValidarOrdenCompraRequest
    {
        [Required]
        public Guid OrdenCompraId { get; set; }
        
        [Required]
        public Guid PresupuestoId { get; set; }
    }
}
