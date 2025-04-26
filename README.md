---
title: "Plataforma de Gestión de Proyectos de Construcción"
author: "Julian Guisao & Carolina Valdez"
institution: "Universidad Pontificia Bolivariana"
date: "2025"
---

# 🏗️ Objetivo del Proyecto

**Crear una plataforma que optimice la gestión operativa de proyectos de construcción, asegurando una ejecución eficiente y rentable.**

---

# 🧭 Dominio Principal

El sistema permite controlar entregas, gastos, avances, y registrar incidentes o modificaciones al plan de obra.

---

# 🗂️ Estructura del Proyecto

El proyecto está organizado en una arquitectura basada en capas, siguiendo principios de Domain-Driven Design (DDD):

- **Api**: Contiene los controladores de la API (`OrdenCompraController`, `PresupuestoController`).
- **Application**: Maneja los DTOs, interfaces de servicios, servicios de aplicación y validadores.
- **Domain**: Define las entidades principales (`Factura`, `Gasto`, `ItemPresupuesto`, `OrdenCompra`, `Presupuesto`), eventos, interfaces de repositorios, servicios de dominio y value objects (`CategoriaGasto`, `Dinero`).
- **Infrastructure**: Implementa la comunicación externa (`OrdenCompraApiClient`, `PresupuestoApiClient`) y los repositorios para persistencia de datos.

Archivos importantes adicionales:
- `appsettings.json`: Configuración de la aplicación.
- `Program.cs`: Configuración y arranque del servidor.
- `README.md`: Documentación inicial del proyecto.

---

## 📦 Gestión de Obras

### 📌 Control de Entregas

**Entidades:**

- **EntregaMaterial**
  - `fecha`: Fecha de entrega.
  - `proveedor`: Entidad responsable de la entrega.
  - `cantidad`: Número de unidades.
  - `estado`: Recibido / Pendiente / Incompleto.

- **Pedido**
  - `materialesSolicitados`
  - `fechaEsperada`

- **Proveedor**
  - `nombre`, `contacto`, `historial`

---

### 💰 Control de Gastos

**Entidades:**

- **Gasto**
  - `descripción`, `monto`, `categoría`

- **Factura**
  - `número`, `fecha`, `total`

- **Presupuesto**
  - `estimado` vs. `real`

---

### 🔧 Seguimiento de Avances

**Entidades:**

- **AvanceTarea**
  - `porcentajeCompletado`, `fecha`

- **FaseProyecto**
  - `estado`: en curso / terminada / retrasada

---

### ⚠️ Incidentes y Modificaciones

**Entidades:**

- **Incidente**
  - `tipo`, `impacto`, `fecha`

- **CambioPlan**
  - `modificación realizada`, `justificación`

---

# 🗣️ Lenguaje Ubicuo

- **Gasto**: Registro de una erogación económica.
- **Factura**: Documento que valida un gasto.
- **Presupuesto**: Comparación entre el estimado y el real.
- **Categoría de gasto**: Ej. materiales, personal, maquinaria.

---

# 💎 Value Objects

## Control de Gastos

- **`Monto`**
  - Valor económico, no negativo, puede sumarse/restarse, incluye moneda.

- **`CategoríaGasto`**
  - Valida categorías permitidas como "materiales", "mano de obra".

- **`Descripción`**
  - Texto limitado, sin caracteres inválidos.

- **`FechaRegistro`**
  - No futura, válida dentro del rango presupuestal.

---

## Factura

- **`NumeroFactura`**
  - Identificador con patrón (`FAC-0001`, etc).

- **`FechaEmisión`**
  - Validación similar a `FechaRegistro`.

- **`TotalFactura`**
  - Reutiliza `Monto`.

---

## Presupuesto

- **`PeriodoPresupuestal`**
  - Encapsula `fechaInicio` y `fechaFin`. Valida que inicio < fin.

- **`MontoEstimado`** y **`MontoReal`**
  - Ambos como objetos de tipo `Monto`.

---
![Texto alternativo](/img/agregados-01.png)
![Texto alternativo](/img/agregados-02.png)
![Texto alternativo](/img/agregados-03.png)
![Texto alternativo](/img/Imagen1.png)
---
# Bounded context
![Texto alternativo](/img/04.jpg)

---
# 🧪 Casos de Prueba para Breakpoints

## 🧾 Órdenes de Compra

```json
{
  "numero": "OC-2025-001",
  "solicitante": "Juan Pérez",
  "proveedor": "ConstruMateriales S.A.",
  "items": [
    {
      "codigo": "MAT-001",
      "descripcion": "Arena fina para concreto",
      "cantidad": 600,
      "precioUnitario": 15000,
      "moneda": "COP"
    },
    {
      "codigo": "MAT-002",
      "descripcion": "Varilla de acero 12mm",
      "cantidad": 50,
      "precioUnitario": 32000,
      "moneda": "COP"
    }
  ]
}
```
## 🧾 Presupuesto

```json
{
  "numero": "OC-2025-001",
  "solicitante": "Juan Pérez",
  "proveedor": "ConstruMateriales S.A.",
  "items": [
    {
      "codigo": "MAT-001",
      "descripcion": "Arena fina para concreto",
      "cantidad": 600,
      "precioUnitario": 15000,
      "moneda": "COP"
    },
    {
      "codigo": "MAT-002",
      "descripcion": "Varilla de acero 12mm",
      "cantidad": 50,
      "precioUnitario": 32000,
      "moneda": "COP"
    }
  ]
}
