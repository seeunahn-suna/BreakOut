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
- **R** reinicia el nivel actual con los puntos y vidas que tenías al empezarlo.

Para la comparación del módulo 12, juega una ronda con **Ratón** y otra con **Eje**. Las dos versiones usan 16 unidades/s para la paleta y 10,5 para la pelota. Pídele a la misma persona que compare la precisión al devolver la pelota, la facilidad para llegar a los bordes y el modo que prefiere.

## Campaña

Empiezas con tres vidas. Rompe todos los bloques destructibles y elige **Siguiente nivel**. Los puntos y las vidas pasan al siguiente nivel. Si completas el séptimo, ganas la campaña.

| Nivel | Color | Bloques destructibles | Velocidad de la pelota | Ancho de la paleta |
| --- | --- | ---: | ---: | ---: |
| 1 · Coral | Rojo coral | 30 | 10,5 | 2,3 |
| 2 · Salvia | Verde salvia | 33 | 11,8 | 2,18 |
| 3 · Miel | Amarillo mantequilla | 34 | 13 | 2,03 |
| 4 · Arcilla | Marrón arcilla | 36 | 14,2 | 1,92 |
| 5 · Mandarina | Naranja suave | 38 | 15,6 | 1,8 |
| 6 · Lavanda | Lavanda pastel | 40 | 16,4 | 1,72 |
| 7 · Rosa | Rosa pastel | 44 | 17,2 | 1,64 |

El fondo beige y la cancha turquesa se comparten en toda la campaña. Los bloques normales dan 100 puntos y los largos, marcados con dos franjas, dan 200. Los bloques con candado son invulnerables y quedan fuera del contador.

Las siete formaciones se editan en `Assets/BreakOut/Formaciones`. Puedes ajustar el orden, la velocidad y el ancho de la paleta en **Control del nivel** dentro de cada escena. El marcador y los menús se editan en `MenusBreakOut.prefab`.

## Módulo 14 · Scene Manager y ScriptableObjects

`Nivel6.unity` y `Nivel7.unity` se crearon duplicando `Nivel1.unity`, con formaciones nuevas. Están registradas en Build Settings. Los cinco primeros niveles siguen en `Nivel1`. Al terminar el quinto, `SceneManager.LoadScene` carga `Nivel6` y después `Nivel7`. Volver al menú y volver a jugar regresan a `Nivel1`.

El menú de pausa se abre con **Esc**, **Options** o el botón **Pausa**. Detiene la física y permite reanudar, reiniciar el nivel o volver al menú. Se puede recorrer con teclado, ratón o mando.

El botón **Dificultad** del menú principal recorre las opciones guardadas en `Assets/BreakOut/Datos/Opciones.asset`.

| Dificultad | Golpes por bloque destructible | Multiplicador de velocidad |
| --- | ---: | ---: |
| Fácil | 1 | 0,9 |
| Normal | 2 | 1 |
| Difícil | 3 | 1,15 |

La dificultad se aplica al empezar. Los bloques se aclaran al recibir daño. Los bloques invulnerables conservan su candado. La velocidad de la tabla de campaña se multiplica por la dificultad.

Cada nivel tiene su ScriptableObject `PuntajeAltoNivel01.asset` a `PuntajeAltoNivel07.asset`. **Administrador de persistencia** contiene las siete referencias y las opciones, permanece activo mediante `DontDestroyOnLoad` y guarda con `PlayerPrefs`. El récord muestra los puntos obtenidos en ese nivel. Se conserva al reiniciar y al volver a abrir el juego.

Para verificarlo, elige una dificultad, juega y pausa. Rompe un bloque, vuelve al menú y comprueba el récord al iniciar otra partida. Puedes abrir directamente `Nivel6` o `Nivel7` en Unity para revisar las escenas nuevas.
