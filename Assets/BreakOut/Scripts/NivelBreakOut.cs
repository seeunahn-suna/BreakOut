using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Pantalla = MenusBreakOut.Pantalla;

public class NivelBreakOut : MonoBehaviour
{
    [System.Serializable]
    private class Etapa
    {
        public string nombre;
        public GameObject formacion;
        [Min(1f)] public float velocidadPelota;
        [Min(0.5f)] public float anchoPaleta;
        public PuntajeAlto puntajeAlto;
    }

    private const int VidasIniciales = 3;

    [SerializeField] private Paleta paleta;
    [SerializeField] private Pelota pelota;
    [SerializeField] private Camera camara;
    [SerializeField] private MenusBreakOut menus;
    [SerializeField] private Transform zonaBloques;
    [SerializeField] private Etapa[] etapas;
    [SerializeField] private string escenaSiguiente;
    [SerializeField] private string escenaInicial = "Nivel1";
    [SerializeField] private int primerNivel = 1;
    [SerializeField] private int totalNiveles = 7;

    private GameObject bloques;
    private Vector2 inicioPaleta;
    private Pantalla pantallaActual;
    private int frameDeCambio;
    private int etapaActual;
    private int vidas;
    private int puntos;
    private int vidasAlEntrar;
    private int puntosAlEntrar;
    private int bloquesPendientes;
    private AdministradorPersistencia persistencia;
    private bool cambiandoEscena;

    private void Start()
    {
        inicioPaleta = paleta.Posicion;
        persistencia = AdministradorPersistencia.Instancia;
        if (persistencia.ConsumirTransicion(out int puntosGuardados, out int vidasGuardadas))
            CargarEtapa(0, puntosGuardados, vidasGuardadas, Pantalla.Partida);
        else
            IniciarCampana(Pantalla.Principal);
    }

    private void Update()
    {
        if (cambiandoEscena)
            return;

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
            ColocarPelota();
            pelota.LeerLanzamiento();
        }
        else if (pelota.Altura < -5.2f)
        {
            vidas--;
            pelota.Detener();
            ActualizarMarcador();
            if (vidas == 0)
                CambiarPantalla(Pantalla.Derrota);
        }

        ActualizarControles();
    }

    private void LateUpdate()
    {
        camara.orthographicSize = Mathf.Max(5.85f, 9.4f / camara.aspect);
    }

    public void RegistrarBloqueRoto(int valor)
    {
        if (pantallaActual != Pantalla.Partida || cambiandoEscena)
            return;

        puntos += valor;
        etapas[etapaActual].puntajeAlto.Registrar(puntos - puntosAlEntrar);
        bloquesPendientes--;
        ActualizarMarcador();
        if (bloquesPendientes == 0)
        {
            pelota.Detener();
            persistencia.Guardar();
            bool ultimaEtapa = etapaActual == etapas.Length - 1 && string.IsNullOrEmpty(escenaSiguiente);
            CambiarPantalla(ultimaEtapa ? Pantalla.Victoria : Pantalla.EtapaSuperada);
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

    public void Reiniciar()
    {
        if (pantallaActual == Pantalla.Partida || pantallaActual == Pantalla.Pausa)
        {
            persistencia.Guardar();
            CargarEtapa(etapaActual, puntosAlEntrar, vidasAlEntrar, Pantalla.Partida);
        }
    }

    public void SiguienteORepetir()
    {
        if (cambiandoEscena)
            return;

        if (pantallaActual == Pantalla.EtapaSuperada)
        {
            if (etapaActual + 1 < etapas.Length)
                CargarEtapa(etapaActual + 1, puntos, vidas, Pantalla.Partida);
            else
                CargarEscena(escenaSiguiente, true);
        }
        else if (pantallaActual == Pantalla.Victoria || pantallaActual == Pantalla.Derrota)
        {
            if (gameObject.scene.name == escenaInicial)
                IniciarCampana(Pantalla.Partida);
            else
            {
                puntos = 0;
                vidas = VidasIniciales;
                CargarEscena(escenaInicial, true);
            }
        }
    }

    public void VolverAlMenu()
    {
        persistencia.Guardar();
        if (gameObject.scene.name == escenaInicial)
            IniciarCampana(Pantalla.Principal);
        else
            CargarEscena(escenaInicial, false);
    }

    public void CambiarDificultad()
    {
        if (pantallaActual != Pantalla.Principal)
            return;

        var opciones = persistencia.Opciones;
        opciones.CambiarDificultad(opciones.Dificultad % 3 + 1);
        persistencia.Guardar();
        CargarEtapa(etapaActual, puntosAlEntrar, vidasAlEntrar, Pantalla.Principal);
        menus.SeleccionarDificultad();
    }

    public void Salir() => Application.Quit();

    private void IniciarCampana(Pantalla destino) => CargarEtapa(0, 0, VidasIniciales, destino);

    private void CargarEscena(string nombre, bool continuar)
    {
        if (cambiandoEscena)
            return;

        cambiandoEscena = true;
        pelota.Detener();
        paleta.enabled = false;
        Time.timeScale = 1f;
        if (continuar)
            persistencia.PrepararTransicion(puntos, vidas);
        else
            persistencia.CancelarTransicion();
        SceneManager.LoadScene(nombre);
    }

    private void CargarEtapa(int indice, int nuevosPuntos, int nuevasVidas, Pantalla destino)
    {
        etapaActual = indice;
        puntos = puntosAlEntrar = nuevosPuntos;
        vidas = vidasAlEntrar = nuevasVidas;

        if (bloques != null)
        {
            bloques.SetActive(false);
            Destroy(bloques);
        }

        Etapa etapa = etapas[indice];
        bloques = Instantiate(etapa.formacion, zonaBloques);
        bloquesPendientes = 0;
        foreach (Bloque bloque in bloques.GetComponentsInChildren<Bloque>())
        {
            if (bloque.EsDestructible)
                bloquesPendientes++;
        }

        paleta.Preparar(inicioPaleta, etapa.anchoPaleta);
        pelota.Preparar(etapa.velocidadPelota);
        ColocarPelota();
        CambiarPantalla(destino);
    }

    private void ColocarPelota() => pelota.Colocar(paleta.Posicion + Vector2.up * 0.52f);

    private void CambiarPantalla(Pantalla pantalla)
    {
        pantallaActual = pantalla;
        frameDeCambio = Time.frameCount;
        paleta.enabled = pantalla == Pantalla.Partida;
        Time.timeScale = pantalla == Pantalla.Pausa ? 0f : 1f;
        if (pantalla == Pantalla.Pausa || pantalla == Pantalla.Derrota)
            persistencia.Guardar();
        menus.Mostrar(pantalla, puntos, vidas, etapaActual + primerNivel, totalNiveles);
        menus.ActualizarOpciones(persistencia.Opciones.NombreDificultad, etapas[etapaActual].puntajeAlto.Valor);
        if (pantalla == Pantalla.Partida)
        {
            ActualizarMarcador();
            ActualizarControles();
        }
    }

    private void ActualizarMarcador()
    {
        menus.ActualizarMarcador(etapaActual + primerNivel, totalNiveles, etapas[etapaActual].nombre, puntos, vidas,
            bloquesPendientes);
        menus.ActualizarOpciones(persistencia.Opciones.NombreDificultad, etapas[etapaActual].puntajeAlto.Valor);
    }

    private void ActualizarControles()
    {
        string modo = paleta.UsaRaton ? "Ratón" : paleta.UsaMando ? "Mando" : "Teclado";
        menus.ActualizarControles(pelota.EnJuego, modo);
    }

    private void OnDestroy()
    {
        if (pantallaActual == Pantalla.Pausa)
            Time.timeScale = 1f;
    }
}
