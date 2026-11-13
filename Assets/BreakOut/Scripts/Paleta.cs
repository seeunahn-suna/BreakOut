using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Paleta : MonoBehaviour
{
    private const float ZonaMuertaStick = 0.15f;
    private const float CambioMinimoStick = 0.25f;

    [SerializeField] private float velocidad = 16f;
    [SerializeField] private float limiteHorizontal = 7.1f;
    [SerializeField] private bool usarRaton;
    private Rigidbody2D cuerpo;
    private Camera camara;
    private float entradaHorizontal;
    private float destinoRaton;
    private float tecladoAnterior;
    private float ultimoStickActivo;
    private bool usarMando;

    public Vector2 Posicion => cuerpo.position;
    public float Semiancho => transform.localScale.x / 2f;
    public bool UsaRaton => usarRaton;
    public bool UsaMando => !usarRaton && usarMando;

    private void Awake()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        camara = Camera.main;
        destinoRaton = cuerpo.position.x;
    }

    private void Update()
    {
        float tecladoDirecto = Input.GetAxisRaw("Horizontal");
        Gamepad mando = Gamepad.current;
        float stick = mando == null ? 0f : mando.leftStick.ReadValue().x;
        if (Mathf.Abs(stick) < ZonaMuertaStick)
            stick = 0f;

        ActualizarControl(tecladoDirecto, stick,
            Input.GetKeyDown(KeyCode.F1), Input.GetKeyDown(KeyCode.F2));

        if (usarRaton)
        {
            Vector3 cursor = Input.mousePosition;
            cursor.z = cuerpo.transform.position.z - camara.transform.position.z;
            destinoRaton = Mathf.Clamp(camara.ScreenToWorldPoint(cursor).x, -limiteHorizontal, limiteHorizontal);
            return;
        }

        entradaHorizontal = usarMando ? stick : Input.GetAxis("Horizontal");
    }

    private void ActualizarControl(float teclado, float stick, bool activarRaton, bool activarEje)
    {
        bool tecladoNuevo = teclado != 0f && teclado != tecladoAnterior;
        bool stickNuevo = stick != 0f && (ultimoStickActivo == 0f ||
            Mathf.Sign(stick) != Mathf.Sign(ultimoStickActivo) ||
            Mathf.Abs(stick - ultimoStickActivo) >= CambioMinimoStick);

        tecladoAnterior = teclado;
        if (stickNuevo || stick == 0f)
            ultimoStickActivo = stick;

        if (activarRaton)
        {
            usarRaton = true;
            return;
        }

        if (activarEje || tecladoNuevo || stickNuevo)
            usarRaton = false;
        if (usarRaton)
            return;

        // Un movimiento nuevo tiene prioridad sobre un control que sigue sostenido.
        if (tecladoNuevo || (stick == 0f && teclado != 0f))
            usarMando = false;
        else if (stickNuevo || (teclado == 0f && stick != 0f))
            usarMando = true;
    }

    private void FixedUpdate()
    {
        float paso = velocidad * Time.fixedDeltaTime;
        float x = usarRaton
            ? Mathf.MoveTowards(cuerpo.position.x, destinoRaton, paso)
            : cuerpo.position.x + entradaHorizontal * paso;
        x = Mathf.Clamp(x, -limiteHorizontal, limiteHorizontal);
        cuerpo.MovePosition(new Vector2(x, cuerpo.position.y));
    }
}
