using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenusBreakOut : MonoBehaviour
{
    [SerializeField] private EventSystem eventos;
    [SerializeField] private GameObject principal;
    [SerializeField] private GameObject pausa;
    [SerializeField] private GameObject victoria;
    [SerializeField] private GameObject derrota;
    [SerializeField] private Text puntosPausa;
    [SerializeField] private Text puntosVictoria;
    [SerializeField] private Text puntosDerrota;
    [SerializeField] private Text contextoVictoria;
    [SerializeField] private Text tituloVictoria;
    [SerializeField] private Text rotuloPuntosVictoria;
    [SerializeField] private Text accionVictoria;
    [SerializeField] private GameObject hud;
    [SerializeField] private Text numeroNivel;
    [SerializeField] private Text nombreNivel;
    [SerializeField] private Text puntosHud;
    [SerializeField] private Text vidasHud;
    [SerializeField] private Text bloquesHud;
    [SerializeField] private Text modoHud;
    [SerializeField] private Text ayudaHud;
    [SerializeField] private Text dificultadPrincipal;
    [SerializeField] private Text dificultadPausa;
    [SerializeField] private Text recordHud;

    public enum Pantalla { Principal, Partida, Pausa, Victoria, Derrota, EtapaSuperada }

    public void Mostrar(Pantalla pantalla, int puntos, int vidas, int etapa, int totalEtapas)
    {
        GameObject activa = null;
        switch (pantalla)
        {
            case Pantalla.Principal:
                activa = principal;
                break;
            case Pantalla.Pausa:
                activa = pausa;
                puntosPausa.text = $"Nivel {etapa}/{totalEtapas}   ·   {puntos:N0} pts   ·   Vidas {vidas}";
                break;
            case Pantalla.EtapaSuperada:
                activa = victoria;
                contextoVictoria.text = $"NIVEL {etapa:00} DE {totalEtapas:00}   ·   VIDAS {vidas}";
                tituloVictoria.text = "¡Nivel superado!";
                rotuloPuntosVictoria.text = "PUNTUACIÓN ACUMULADA";
                accionVictoria.text = "Siguiente nivel";
                puntosVictoria.text = $"{puntos:N0} pts";
                break;
            case Pantalla.Victoria:
                activa = victoria;
                contextoVictoria.text = "CAMPAÑA COMPLETADA";
                tituloVictoria.text = "¡Victoria!";
                rotuloPuntosVictoria.text = "PUNTUACIÓN FINAL";
                accionVictoria.text = "Volver a jugar";
                puntosVictoria.text = $"{puntos:N0} pts";
                break;
            case Pantalla.Derrota:
                activa = derrota;
                puntosDerrota.text = $"{puntos:N0} pts";
                break;
        }

        eventos.SetSelectedGameObject(null);
        principal.SetActive(activa == principal);
        pausa.SetActive(activa == pausa);
        victoria.SetActive(activa == victoria);
        derrota.SetActive(activa == derrota);
        hud.SetActive(pantalla == Pantalla.Partida);

        if (activa != null)
            eventos.SetSelectedGameObject(activa.GetComponentInChildren<Button>().gameObject);
    }

    public void ActualizarMarcador(int etapa, int totalEtapas, string nombre, int puntos, int vidas, int bloques)
    {
        numeroNivel.text = $"NIVEL {etapa:00} / {totalEtapas:00}";
        nombreNivel.text = nombre;
        puntosHud.text = puntos.ToString("N0");
        vidasHud.text = vidas.ToString();
        bloquesHud.text = bloques.ToString();
    }

    public void ActualizarControles(bool pelotaEnJuego, string modo)
    {
        if (modoHud.text != modo)
            modoHud.text = modo;

        string ayuda = pelotaEnJuego ? "Esc / Options para pausar" : "Espacio / ✕ para lanzar";
        if (ayudaHud.text != ayuda)
            ayudaHud.text = ayuda;
    }

    public void ActualizarOpciones(string dificultad, int puntajeAlto)
    {
        dificultadPrincipal.text = $"Dificultad   ·   {dificultad}";
        dificultadPausa.text = $"{dificultad}   ·   Récord del nivel {puntajeAlto:N0}";
        recordHud.text = $"Récord del nivel  {puntajeAlto:N0}";
    }

    public void SeleccionarDificultad() =>
        eventos.SetSelectedGameObject(dificultadPrincipal.transform.parent.gameObject);
}
