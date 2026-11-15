using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Pantalla = MenusBreakOut.Pantalla;

public class NivelBreakOut : MonoBehaviour
{
    [SerializeField] private Paleta paleta;
    [SerializeField] private Pelota pelota;
    [SerializeField] private Camera camara;
    [SerializeField] private MenusBreakOut menus;
    private static Pantalla pantallaTrasRecarga = Pantalla.Principal;
    private Pantalla pantallaActual;
    private int frameDeCambio;
    private int vidas = 3;
    private int puntos;
    private int bloquesPendientes;
    private GUIStyle texto;
    private GUIStyle centro;

    private void Start()
    {
        foreach (Bloque bloque in FindObjectsByType<Bloque>(FindObjectsSortMode.None))
        {
            if (bloque.gameObject.scene == gameObject.scene && bloque.EsDestructible)
                bloquesPendientes++;
        }

        CambiarPantalla(pantallaTrasRecarga);
        pantallaTrasRecarga = Pantalla.Principal;
    }

    private void Update()
    {
        Gamepad mando = Gamepad.current;
        bool pausaPulsada = Input.GetKeyDown(KeyCode.Escape) ||
            (mando != null && mando.startButton.wasPressedThisFrame);

        if (pausaPulsada && pantallaActual == Pantalla.Partida)
        {
            Pausar();
            return;
        }

        if (pausaPulsada && pantallaActual == Pantalla.Pausa)
        {
            Reanudar();
            return;
        }

        if (pantallaActual != Pantalla.Partida || frameDeCambio == Time.frameCount)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reiniciar();
            return;
        }

        if (!pelota.EnJuego)
        {
            pelota.Colocar(paleta.Posicion + Vector2.up * 0.52f);
            pelota.LeerLanzamiento();
        }
        else if (pelota.Altura < -5.2f)
        {
            vidas--;
            pelota.Detener();
            if (vidas == 0)
                CambiarPantalla(Pantalla.Derrota);
        }
    }

    private void LateUpdate()
    {
        camara.orthographicSize = Mathf.Max(5.5f, 9f / camara.aspect);
    }

    public void RegistrarBloqueRoto(int valor)
    {
        if (pantallaActual != Pantalla.Partida)
            return;

        puntos += valor;
        bloquesPendientes--;
        if (bloquesPendientes == 0)
        {
            pelota.Detener();
            CambiarPantalla(Pantalla.Victoria);
        }
    }

    public void Jugar()
    {
        if (pantallaActual == Pantalla.Principal)
            CambiarPantalla(Pantalla.Partida);
    }

    public void Pausar()
    {
        if (pantallaActual == Pantalla.Partida)
            CambiarPantalla(Pantalla.Pausa);
    }

    public void Reanudar()
    {
        if (pantallaActual == Pantalla.Pausa)
            CambiarPantalla(Pantalla.Partida);
    }

    public void Reiniciar() => Recargar(Pantalla.Partida);
    public void VolverAlMenu() => Recargar(Pantalla.Principal);

    public void Salir() => Application.Quit();

    private void Recargar(Pantalla destino)
    {
        pantallaTrasRecarga = destino;
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameObject.scene.name);
    }

    private void CambiarPantalla(Pantalla pantalla)
    {
        pantallaActual = pantalla;
        frameDeCambio = Time.frameCount;
        paleta.enabled = pantalla == Pantalla.Partida;
        Time.timeScale = pantalla == Pantalla.Pausa ? 0f : 1f;
        menus.Mostrar(pantalla, puntos, vidas);
    }

    private void OnDestroy()
    {
        if (pantallaActual == Pantalla.Pausa)
            Time.timeScale = 1f;
    }

    private void OnGUI()
    {
        if (pantallaActual != Pantalla.Partida)
            return;

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

        string mensaje = pelota.EnJuego ? "Esc o Options para pausar" : "Espacio o X para lanzar";

        string modo = paleta.UsaRaton ? "Ratón" : paleta.UsaMando ? "Mando" : "Teclado";
        GUI.Label(new Rect(16, tamano + 16, Screen.width - 32, tamano + 8),
            $"{modo}   |   F1 Ratón   F2 Eje", texto);
        if (GUI.Button(new Rect(Screen.width - 100, 10, 84, 32), "Pausa"))
            Pausar();
        GUI.Label(new Rect(8, Screen.height - tamano - 12, Screen.width - 16, tamano + 8), mensaje, centro);
    }
}
