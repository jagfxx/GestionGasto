# Resumen de Hallazgos Introducidos

## 1. 🆕 New Issue
**¿Qué se hizo?**  
Se agregaron variables declaradas pero nunca utilizadas en `OrdenCompraService.cs`.

**Ejemplo:**
```csharp
// Variables sin usar (generan advertencias)
var unusedVariable = "No se usa";
int counter; // Nunca se asigna
```

**Impacto:**  
- Código innecesario que confunde  
- Dificulta el mantenimiento

---

## 2. 🏗️ Maintainability
**¿Qué se hizo?**  
Se creó una función extremadamente larga con malos nombres en `PresupuestoService.cs`.

**Ejemplo:**
```csharp
public string a(Guid x, string y, int z, ...) {
    // 50+ líneas de código
    // con 10+ niveles de anidación
}
```

**Impacto:**  
- Código difícil de leer  
- Imposible de probar  
- Violación de principios SOLID

---

## 3. 🔄 Duplications
**¿Qué se hizo?**  
Se copiaron métodos idénticos con nombres diferentes en `OrdenCompraService.cs`.

**Ejemplo:**
```csharp
private OrdenCompraDto ToDto(OrdenCompra orden) { ... }
private OrdenCompraDto ConvertirADto(OrdenCompra orden) { ... } // Misma implementación
```

**Impacto:**  
- Mantenimiento complicado  
- Posibles inconsistencias  
- Código inflado

---

## 4. 🎯 Reliability
**¿Qué se hizo?**  
Se agregaron métodos con errores potenciales en `ValidadorOrdenCompraService.cs`.

**Ejemplos:**
```csharp
// 1. División por cero
public decimal CalcularPromedio(int total, int cantidad) 
    => total / cantidad;

// 2. Acceso a índice sin validar
public string PrimerItem(List<string> items) 
    => items[0];

// 3. Parse sin validación
public int AEntero(string valor) 
    => int.Parse(valor);
```

**Impacto:**  
- Errores en tiempo de ejecución  
- Caídas de la aplicación  
- Posible pérdida de datos

---

## 5. 📉 Coverage
**¿Qué se hizo?**  
No se incluyeron pruebas unitarias para ninguna clase o método.

**Impacto:**  
- Cobertura de pruebas: 0%  
- Sin validación automática  
- Alto riesgo en cambios futuros

---

## Cómo se vería en SonarQube:

```
🔴 15 Issues
  - 5 Bugs (Reliability)
  - 6 Code Smells (New Issues)
  - 4 Code Smells (Maintainability)
  - 10% Duplicación
  - 0% Cobertura
```

> **Nota:** Estos hallazgos fueron introducidos intencionalmente con fines demostrativos para mostrar el análisis estático de código con SonarQube.
