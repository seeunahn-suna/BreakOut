using UnityEngine;
using UnityEngine.SceneManagement;

public class NivelBreakOut : MonoBehaviour
{
    [SerializeField] private Paleta paleta;
    [SerializeField] private Pelota pelota;
    [SerializeField] private Camera camara;
    private int vidas = 3;
    private int puntos;
    private int bloquesPendientes;
    private bool terminado;
    private bool ganado;
    private GUIStyle texto;
    private GUIStyle centro;

    private void Start()
    {
        foreach (Bloque bloque in FindObjectsByType<Bloque>(FindObjectsSortMode.None))
        {
            if (bloque.gameObject.scene == gameObject.scene && bloque.EsDestructible)
                bloquesPendientes++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(gameObject.scene.name);
            return;
        }

        if (terminado)
            return;

        if (!pelota.EnJuego)
        {
            pelota.Colocar(paleta.Posicion + Vector2.up * 0.52f);
            pelota.LeerLanzamiento();
        }
        else if (pelota.Altura < -5.2f)
        {
            vidas--;
            pelota.Detener();
            terminado = vidas == 0;
        }
    }

    private void LateUpdate()
    {
        camara.orthographicSize = Mathf.Max(5.5f, 9f / camara.aspect);
    }

    public void RegistrarBloqueRoto(int valor)
    {
        puntos += valor;
        bloquesPendientes--;
        if (bloquesPendientes == 0)
        {
            ganado = true;
            terminado = true;
            pelota.Detener();
        }
    }

    private void OnGUI()
    {
        if (texto == null)
        {
            texto = new GUIStyle(GUI.skin.label);
            texto.normal.textColor = new Color(0.88f, 0.94f, 1f);
            centro = new GUIStyle(texto) { alignment = TextAnchor.MiddleCenter };
        }

        int tamano = Mathf.Clamp(Screen.height / 30, 14, 28);
        texto.fontSize = tamano;
        centro.fontSize = tamano;
        GUI.Label(new Rect(16, 8, Screen.width - 32, tamano + 10),
            $"BREAKOUT   |   Puntos {puntos}   |   Vidas {vidas}", texto);

        string mensaje;
        if (terminado)
            mensaje = ganado ? "Nivel completado   ·   R para volver a jugar" : "Fin de la partida   ·   R para volver a jugar";
        else if (!pelota.EnJuego)
            mensaje = "Espacio o X para lanzar";
        else
            mensaje = "R para reiniciar";

        string modo = paleta.UsaRaton ? "Ratón" : "Stick izquierdo / teclado";
        GUI.Label(new Rect(16, tamano + 16, Screen.width - 32, tamano + 8),
            $"{modo}   |   F1 Ratón   F2 Eje", texto);
        GUI.Label(new Rect(8, Screen.height - tamano - 12, Screen.width - 16, tamano + 8), mensaje, centro);
    }
}
