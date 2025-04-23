using ControlGastos.Application.DTOs;
using FluentValidation;

namespace ControlGastos.Application.Validators
{
    public class CrearPresupuestoValidator : AbstractValidator<CrearPresupuestoDto>
    {
        public CrearPresupuestoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del presupuesto es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("El presupuesto debe tener al menos un ítem");

            RuleForEach(x => x.Items).SetValidator(new ItemPresupuestoValidator());
        }
    }

    public class ItemPresupuestoValidator : AbstractValidator<ItemPresupuestoDto>
    {
        public ItemPresupuestoValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("El código del ítem es obligatorio")
                .MaximumLength(50).WithMessage("El código no puede tener más de 50 caracteres");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción del ítem es obligatoria")
                .MaximumLength(200).WithMessage("La descripción no puede tener más de 200 caracteres");

            RuleFor(x => x.CantidadPresupuestada)
                .GreaterThan(0).WithMessage("La cantidad presupuestada debe ser mayor a 0");

            RuleFor(x => x.PrecioUnitarioEstimado)
                .GreaterThan(0).WithMessage("El precio unitario estimado debe ser mayor a 0");

            RuleFor(x => x.Categoria)
                .NotEmpty().WithMessage("La categoría es obligatoria");
        }
    }

    public class AgregarItemPresupuestoValidator : AbstractValidator<AgregarItemPresupuestoDto>
    {
        public AgregarItemPresupuestoValidator()
        {
            RuleFor(x => x.PresupuestoId)
                .NotEmpty().WithMessage("El ID del presupuesto es obligatorio");

            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("El código del ítem es obligatorio")
                .MaximumLength(50).WithMessage("El código no puede tener más de 50 caracteres");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción del ítem es obligatoria")
                .MaximumLength(200).WithMessage("La descripción no puede tener más de 200 caracteres");

            RuleFor(x => x.CantidadPresupuestada)
                .GreaterThan(0).WithMessage("La cantidad presupuestada debe ser mayor a 0");

            RuleFor(x => x.PrecioUnitarioEstimado)
                .GreaterThan(0).WithMessage("El precio unitario estimado debe ser mayor a 0");

            RuleFor(x => x.Categoria)
                .NotEmpty().WithMessage("La categoría es obligatoria");
        }
    }
}
