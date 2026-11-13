# BreakOut

Abre este repositorio como proyecto en **Unity 2022.3.62f3** y carga `Assets/Scenes/Nivel1.unity`. Pulsa **Play**.

- **F1** activa el ratón. **F2**, una tecla de movimiento o un movimiento del stick vuelven al modo por eje.
- Usa **← / →**, **A/D** o el **stick izquierdo del DualSense**. El juego cambia de control al detectar un movimiento nuevo y lo indica en pantalla. El teclado pasa por `Input.GetAxis("Horizontal")`.
- **Espacio** o **✕ en el DualSense** para lanzar la pelota al empezar o al perder una vida.
- **R** para reiniciar el nivel.

Para la comparación del módulo 12, juega una ronda con **Ratón** y otra con **Eje**. Las dos versiones usan 16 unidades/s para la paleta y 10,5 para la pelota. Pídele a la misma persona que compare la precisión al devolver la pelota, la facilidad para llegar a los bordes y el modo que prefiere.

Empiezas con tres vidas. Destruye los 30 bloques que dan puntos para completar el nivel.

| Prefab | Comportamiento | Puntos |
| --- | --- | --- |
| `BloqueNormal` | Azul. Se rompe con un impacto. | 100 |
| `BloqueLargo` | Naranja. Ocupa el espacio de dos bloques normales y se rompe con un impacto. | 200 |
| `BloqueInvulnerable` | Violeta. La pelota rebota y el bloque queda en el nivel. | 0 |

Los prefabs están en `Assets/BreakOut/Prefabs` y sus materiales en `Assets/BreakOut/Materiales`. `Bloque` concentra la lógica de impacto. `BloqueLargo` y `BloqueInvulnerable` heredan de esa clase y cambian sus propiedades mediante `override`.
