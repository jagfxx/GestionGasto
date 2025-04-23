using ControlGastos.Application.DTOs;
using ControlGastos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlGastos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresupuestoController : ControllerBase
    {
        private readonly IPresupuestoService _presupuestoService;

        public PresupuestoController(IPresupuestoService presupuestoService)
        {
            _presupuestoService = presupuestoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PresupuestoDto>>> GetAll()
        {
            var presupuestos = await _presupuestoService.GetAllAsync();
            return Ok(presupuestos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PresupuestoDto>> GetById(Guid id)
        {
            var presupuesto = await _presupuestoService.GetByIdAsync(id);
            if (presupuesto == null)
                return NotFound();

            return Ok(presupuesto);
        }

        [HttpPost]
        public async Task<ActionResult<PresupuestoDto>> Create([FromBody] CrearPresupuestoDto presupuestoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevoPresupuesto = await _presupuestoService.CreateAsync(presupuestoDto);
            return CreatedAtAction(nameof(GetById), new { id = nuevoPresupuesto.Id }, nuevoPresupuesto);
        }

        [HttpPost("item")]
        public async Task<ActionResult<PresupuestoDto>> AgregarItem([FromBody] AgregarItemPresupuestoDto itemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var presupuesto = await _presupuestoService.AgregarItemAsync(itemDto);
                return Ok(presupuesto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


