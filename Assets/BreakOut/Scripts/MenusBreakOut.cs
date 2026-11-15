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

    public enum Pantalla { Principal, Partida, Pausa, Victoria, Derrota }

    public void Mostrar(Pantalla pantalla, int puntos, int vidas)
    {
        GameObject activa = null;
        switch (pantalla)
        {
            case Pantalla.Principal:
                activa = principal;
                break;
            case Pantalla.Pausa:
                activa = pausa;
                puntosPausa.text = $"{puntos} pts   ·   Vidas {vidas}";
                break;
            case Pantalla.Victoria:
                activa = victoria;
                puntosVictoria.text = $"{puntos} pts";
                break;
            case Pantalla.Derrota:
                activa = derrota;
                puntosDerrota.text = $"{puntos} pts";
                break;
        }

        eventos.SetSelectedGameObject(null);
        principal.SetActive(activa == principal);
        pausa.SetActive(activa == pausa);
        victoria.SetActive(activa == victoria);
        derrota.SetActive(activa == derrota);

        if (activa != null)
            eventos.SetSelectedGameObject(activa.GetComponentInChildren<Button>().gameObject);
    }
}
