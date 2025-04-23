// ControlGastos.Application/Validators/OrdenCompraValidator.cs
using ControlGastos.Application.DTOs;
using FluentValidation;

namespace ControlGastos.Application.Validators
{
    public class CrearOrdenCompraValidator : AbstractValidator<CrearOrdenCompraDto>
    {
        public CrearOrdenCompraValidator()
        {
            RuleFor(x => x.Numero)
                .NotEmpty().WithMessage("El número de la orden de compra es obligatorio")
                .MaximumLength(50).WithMessage("El número no puede tener más de 50 caracteres");

            RuleFor(x => x.Solicitante)
                .NotEmpty().WithMessage("El solicitante es obligatorio")
                .MaximumLength(100).WithMessage("El solicitante no puede tener más de 100 caracteres");

            RuleFor(x => x.Proveedor)
                .NotEmpty().WithMessage("El proveedor es obligatorio")
                .MaximumLength(100).WithMessage("El proveedor no puede tener más de 100 caracteres");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("La orden debe tener al menos un ítem");

            RuleForEach(x => x.Items).SetValidator(new ItemOrdenCompraValidator());
        }
    }

    public class ItemOrdenCompraValidator : AbstractValidator<ItemOrdenCompraDto>
    {
        public ItemOrdenCompraValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("El código del ítem es obligatorio")
                .MaximumLength(50).WithMessage("El código no puede tener más de 50 caracteres");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción del ítem es obligatoria")
                .MaximumLength(200).WithMessage("La descripción no puede tener más de 200 caracteres");

            RuleFor(x => x.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0");

            RuleFor(x => x.PrecioUnitario)
                .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a 0");

            RuleFor(x => x.Moneda)
                .NotEmpty().WithMessage("La moneda es obligatoria")
                .MaximumLength(3).WithMessage("La moneda debe tener máximo 3 caracteres");
        }
    }
}

