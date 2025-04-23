using System;

namespace ControlGastos.Domain.ValueObjects
{
    public class CategoriaGasto
    {
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }

        public CategoriaGasto(string nombre, string descripcion = null)
        {
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Descripcion = descripcion ?? string.Empty;
        }

        public static CategoriaGasto Material => new CategoriaGasto("Material", "Materiales de construcción");
        public static CategoriaGasto ManoDeObra => new CategoriaGasto("ManoDeObra", "Gastos de personal");
        public static CategoriaGasto Maquinaria => new CategoriaGasto("Maquinaria", "Alquiler y uso de maquinaria");
        public static CategoriaGasto Administrativo => new CategoriaGasto("Administrativo", "Gastos administrativos");
        public static CategoriaGasto Otro => new CategoriaGasto("Otro", "Otros gastos no categorizados");

        public override bool Equals(object obj)
        {
            if (obj is CategoriaGasto categoria)
                return Nombre.Equals(categoria.Nombre, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        public override int GetHashCode()
        {
            return Nombre.GetHashCode();
        }
    }
}