# BreakOut

Abre este repositorio como proyecto en **Unity 2022.3.62f3** y carga `Assets/Scenes/Nivel1.unity`. Pulsa **Play** y elige **Jugar**.

## Módulo 13 · Menús y mockups

Los cuatro mockups se exportaron desde Unity en JPG de 1280 × 720. La dirección visual usa beige, azul pastel y blanco.

- [Menú principal](Docs/Modulo13/01-menu-principal.jpg)
- [Menú de pausa](Docs/Modulo13/02-menu-pausa.jpg)
- [Fin de partida con victoria](Docs/Modulo13/03-fin-victoria.jpg)
- [Fin de partida con derrota](Docs/Modulo13/04-fin-derrota.jpg)

El diseño se puede editar en `Assets/BreakOut/Prefabs/MenusBreakOut.prefab`. En partida, pulsa **Esc** u **Options** en el DualSense para pausar o reanudar. Los menús admiten ratón, flechas y **Enter**, o stick/cruceta y **✕**.

## Controles de la partida

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
