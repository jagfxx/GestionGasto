using System;

namespace ControlGastos.Domain.ValueObjects
{
    public class Dinero
    {
        public decimal Valor { get; private set; }
        public string Moneda { get; private set; }

        public Dinero(decimal valor, string moneda = "MXN")
        {
            if (valor < 0)
                throw new ArgumentException("El valor no puede ser negativo", nameof(valor));

            Valor = valor;
            Moneda = moneda ?? throw new ArgumentNullException(nameof(moneda));
        }

        public static Dinero operator +(Dinero a, Dinero b)
        {
            if (a.Moneda != b.Moneda)
                throw new InvalidOperationException("No se pueden sumar montos con diferentes monedas");

            return new Dinero(a.Valor + b.Valor, a.Moneda);
        }

        public static Dinero operator *(Dinero a, int factor)
        {
            return new Dinero(a.Valor * factor, a.Moneda);
        }
    }
}