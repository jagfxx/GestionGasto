# Hallazgos Introducidos Intencionalmente para SonarQube

Este documento detalla los 5 tipos de hallazgos que se introdujeron intencionalmente en el código para demostración y análisis con SonarQube.

---

## 1. 🔴 NEW ISSUE - Code Smells y Variables sin Usar

**Ubicación:** `Application/Services/OrdenCompraService.cs` - Método `CreateAsync()` (líneas 42-65)

**Hallazgos introducidos:**
- ✗ Variable `unusedVariable` declarada pero nunca utilizada
- ✗ Variable `unusedCounter` declarada pero nunca utilizada  
- ✗ Variable `tempData` declarada con valor null y nunca utilizada

```csharp
public async Task<OrdenCompraDto> CreateAsync(CrearOrdenCompraDto ordenCompraDto)
{
    // NEW ISSUE: Variables sin usar
    var unusedVariable = "Esta variable nunca se usa";
    int unusedCounter = 0;
    string tempData = null;
    
    var ordenCompra = new OrdenCompra(...);
    // ... resto del código
}
```

**Impacto:** 
- Código muerto que reduce la legibilidad
- Aumenta el tamaño del código sin aportar funcionalidad
- Warnings del compilador (CS0219)

---

## 2. 🟡 MAINTAINABILITY - Función Larga y Mal Nombrada

**Ubicación:** `Application/Services/PresupuestoService.cs` - Método `a()` (líneas 110-165)

**Hallazgos introducidos:**
- ✗ Nombre de método no descriptivo: `a()`
- ✗ Parámetros con nombres de una sola letra: `x, y, z, w, q, r, s, t, u, v`
- ✗ Complejidad ciclomática alta (10+ niveles de anidación)
- ✗ Método demasiado largo (55+ líneas)
- ✗ Concatenación de strings ineficiente

```csharp
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
                // ... 10 niveles de anidación más
            }
        }
    }
    return result;
}
```

**Impacto:**
- Código difícil de entender y mantener
- Imposible realizar pruebas unitarias efectivas
- Violación de principios SOLID
- Alta deuda técnica

---

## 3. 🟠 DUPLICATIONS - Código Duplicado

**Ubicación:** `Application/Services/OrdenCompraService.cs` (líneas 95-160)

**Hallazgos introducidos:**
- ✗ Método `ToDto()` - líneas 95-115
- ✗ Método `ConvertirADto()` - líneas 118-138 (DUPLICADO)
- ✗ Método `MapearOrdenCompra()` - líneas 140-160 (DUPLICADO)

```csharp
// Método original
private OrdenCompraDto ToDto(OrdenCompra ordenCompra) { ... }

// DUPLICADO 1
private OrdenCompraDto ConvertirADto(OrdenCompra ordenCompra) { ... }

// DUPLICADO 2
private OrdenCompraDto MapearOrdenCompra(OrdenCompra ordenCompra) { ... }
```

**Impacto:**
- Violación del principio DRY (Don't Repeat Yourself)
- Mantenimiento triplicado ante cambios
- Mayor probabilidad de bugs por inconsistencias
- Código innecesariamente extenso

---

## 4. 🔴 RELIABILITY - Errores Potenciales en Ejecución

**Ubicación:** `Domain/Services/ValidadorOrdenCompraService.cs` (líneas 61-91)

**Hallazgos introducidos:**

### a) División por cero sin validación
```csharp
public decimal CalcularDescuento(decimal total, int cantidadItems)
{
    // División por cero sin validación
    return total / cantidadItems;  // ⚠️ DivideByZeroException si cantidadItems = 0
}
```

### b) Acceso a índice sin validar lista vacía
```csharp
public string ObtenerPrimerItem(List<string> items)
{
    // Acceso a índice sin validar si la lista está vacía
    return items[0];  // ⚠️ IndexOutOfRangeException si items.Count = 0
}
```

### c) División por cero en cálculo de porcentaje
```csharp
public decimal CalcularPorcentaje(decimal valor, decimal divisor)
{
    // Otra división por cero potencial
    var porcentaje = (valor / divisor) * 100;  // ⚠️ DivideByZeroException
    return porcentaje;
}
```

### d) Parse sin manejo de excepciones
```csharp
public int ParsearCantidad(string cantidad)
{
    // Parse sin try-catch, puede lanzar FormatException
    return int.Parse(cantidad);  // ⚠️ FormatException si cantidad no es numérico
}
```

### e) Acceso a propiedades sin validación de null
```csharp
public string AccederPropiedad(OrdenCompra orden)
{
    // Posible NullReferenceException
    return orden.Items.First().Descripcion.ToUpper();  
    // ⚠️ NullReferenceException si orden es null, Items vacío, o Descripcion null
}
```

**Impacto:**
- Aplicación propensa a crashes en runtime
- Experiencia de usuario degradada
- Pérdida de datos potencial
- Vulnerabilidades de seguridad

---

## 5. ⚫ COVERAGE - Cobertura de Pruebas 0%

**Estado:** No existen pruebas unitarias en el proyecto

**Verificación:**
```bash
# Búsqueda de archivos de prueba
find . -name "*Test*.cs"     # Resultado: 0 archivos
find . -name "*.Tests.csproj" # Resultado: 0 archivos
```

**Impacto:**
- Cobertura de código: **0%**
- Imposible detectar regresiones
- No hay validación automática de funcionalidad
- Alto riesgo al realizar cambios
- No hay documentación ejecutable del comportamiento esperado

**Clases sin pruebas:**
- ✗ `OrdenCompraService`
- ✗ `PresupuestoService`
- ✗ `ValidadorOrdenCompraService`
- ✗ `OrdenCompraController`
- ✗ `PresupuestoController`
- ✗ Todas las entidades del dominio
- ✗ Todos los value objects
- ✗ Todos los repositorios

---

## Resumen de Hallazgos

| Tipo | Cantidad | Severidad | Ubicación Principal |
|------|----------|-----------|---------------------|
| **New Issue** | 3 variables | ⚠️ Minor | `OrdenCompraService.cs` |
| **Maintainability** | 1 método | 🔴 Critical | `PresupuestoService.cs` |
| **Duplications** | 2 duplicados | 🟠 Major | `OrdenCompraService.cs` |
| **Reliability** | 5 métodos | 🔴 Critical | `ValidadorOrdenCompraService.cs` |
| **Coverage** | 0% cobertura | 🔴 Critical | Todo el proyecto |

---

## Instrucciones para Análisis con SonarQube

### 1. Configurar SonarQube Scanner para .NET

```bash
# Instalar herramienta global
dotnet tool install --global dotnet-sonarscanner

# Iniciar análisis
dotnet sonarscanner begin /k:"GestionGasto" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="tu_token"

# Compilar proyecto
dotnet build

# Finalizar análisis
dotnet sonarscanner end /d:sonar.login="tu_token"
```

### 2. Verificar Hallazgos en Dashboard

Accede a: `http://localhost:9000/dashboard?id=GestionGasto`

Deberías ver:
- **Issues:** 11+ hallazgos
- **Code Smells:** 4+ (variables sin usar + función mal nombrada)
- **Bugs:** 5+ (métodos con reliability issues)
- **Duplications:** ~10-15% del código
- **Coverage:** 0.0%
- **Technical Debt:** Alto

---

## Cómo Corregir los Hallazgos

### New Issue
```csharp
// ✓ Eliminar variables no utilizadas
public async Task<OrdenCompraDto> CreateAsync(CrearOrdenCompraDto ordenCompraDto)
{
    var ordenCompra = new OrdenCompra(...);
    // ... código sin variables innecesarias
}
```

### Maintainability
```csharp
// ✓ Refactorizar con nombre descriptivo y lógica clara
public string GenerarResumenItem(ItemPresupuesto item)
{
    var builder = new StringBuilder();
    builder.AppendLine($"ID: {item.Id}");
    builder.AppendLine($"Nombre: {item.Nombre}");
    // ... usar StringBuilder y extraer métodos
    return builder.ToString();
}
```

### Duplications
```csharp
// ✓ Mantener solo un método de mapeo
private OrdenCompraDto ToDto(OrdenCompra ordenCompra) { ... }
// Eliminar ConvertirADto() y MapearOrdenCompra()
```

### Reliability
```csharp
// ✓ Agregar validaciones
public decimal CalcularDescuento(decimal total, int cantidadItems)
{
    if (cantidadItems <= 0)
        throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(cantidadItems));
    
    return total / cantidadItems;
}
```

### Coverage
```csharp
// ✓ Crear proyecto de pruebas
// ControlGastos.Tests/Services/OrdenCompraServiceTests.cs
[Fact]
public async Task CreateAsync_DebeCrearOrdenCompra()
{
    // Arrange, Act, Assert
}
```

---

## Fecha de Introducción
**Fecha:** 2025-10-01  
**Autor:** Sistema de Demostración  
**Propósito:** Ejercicio académico para análisis de calidad de código con SonarQube
