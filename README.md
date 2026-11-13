# BreakOut · Práctica del módulo 10

Abre este repositorio como proyecto en **Unity 2022.3.62f3** y carga `Assets/Scenes/Nivel1.unity`. Pulsa **Play**.

- **Flechas izquierda y derecha** o **A/D** para mover la paleta.
- **Espacio** para lanzar la pelota al empezar o al perder una vida.
- **R** para reiniciar el nivel.

Empiezas con tres vidas. Destruye los 30 bloques que dan puntos para completar el nivel.

| Prefab | Comportamiento | Puntos |
| --- | --- | --- |
| `BloqueNormal` | Azul. Se rompe con un impacto. | 100 |
| `BloqueLargo` | Naranja. Ocupa el espacio de dos bloques normales y se rompe con un impacto. | 200 |
| `BloqueInvulnerable` | Violeta. La pelota rebota y el bloque queda en el nivel. | 0 |

Los prefabs están en `Assets/BreakOut/Prefabs` y sus materiales en `Assets/BreakOut/Materiales`. `Bloque` concentra la lógica de impacto. `BloqueLargo` y `BloqueInvulnerable` heredan de esa clase y cambian sus propiedades mediante `override`.
