using UnityEngine;

[DefaultExecutionOrder(-100)]
public class AdministradorPersistencia : MonoBehaviour
{
    private const string ClaveDificultad = "BreakOut.Opciones.Dificultad";
    private const string PrefijoRecord = "BreakOut.Record.";

    [SerializeField] private OpcionesBreakOut opciones;
    [SerializeField] private PuntajeAlto[] puntajes;
    private bool hayTransicion;
    private int puntosEnTransicion;
    private int vidasEnTransicion;

    public static AdministradorPersistencia Instancia { get; private set; }
    public OpcionesBreakOut Opciones => opciones;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);
        opciones.CambiarDificultad(PlayerPrefs.GetInt(ClaveDificultad, opciones.Dificultad));
        foreach (PuntajeAlto puntaje in puntajes)
            puntaje.Cargar(PlayerPrefs.GetInt(PrefijoRecord + puntaje.Identificador, 0));
    }

    public void Guardar()
    {
        PlayerPrefs.SetInt(ClaveDificultad, opciones.Dificultad);
        foreach (PuntajeAlto puntaje in puntajes)
            PlayerPrefs.SetInt(PrefijoRecord + puntaje.Identificador, puntaje.Valor);
        PlayerPrefs.Save();
    }

    public void PrepararTransicion(int puntos, int vidas)
    {
        puntosEnTransicion = puntos;
        vidasEnTransicion = vidas;
        hayTransicion = true;
        Guardar();
    }

    public bool ConsumirTransicion(out int puntos, out int vidas)
    {
        puntos = puntosEnTransicion;
        vidas = vidasEnTransicion;
        bool continuar = hayTransicion;
        hayTransicion = false;
        return continuar;
    }

    public void CancelarTransicion() => hayTransicion = false;

    private void OnApplicationPause(bool enPausa)
    {
        if (enPausa)
            Guardar();
    }

    private void OnApplicationQuit() => Guardar();

    private void OnDestroy()
    {
        if (Instancia == this)
        {
            Guardar();
            Instancia = null;
        }
    }
}
