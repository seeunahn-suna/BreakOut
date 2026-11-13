using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Paleta : MonoBehaviour
{
    [SerializeField] private float velocidad = 16f;
    [SerializeField] private float limiteHorizontal = 7.1f;
    [SerializeField] private bool usarRaton;
    private Rigidbody2D cuerpo;
    private Camera camara;
    private float entradaHorizontal;
    private float destinoRaton;

    public Vector2 Posicion => cuerpo.position;
    public float Semiancho => transform.localScale.x / 2f;
    public bool UsaRaton => usarRaton;

    private void Awake()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        camara = Camera.main;
        destinoRaton = cuerpo.position.x;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            usarRaton = true;
        if (Input.GetKeyDown(KeyCode.F2))
            usarRaton = false;

        if (usarRaton)
        {
            Vector3 cursor = Input.mousePosition;
            cursor.z = cuerpo.transform.position.z - camara.transform.position.z;
            destinoRaton = Mathf.Clamp(camara.ScreenToWorldPoint(cursor).x, -limiteHorizontal, limiteHorizontal);
            return;
        }

        float teclado = Input.GetAxis("Horizontal");
        Gamepad mando = Gamepad.current;
        float stick = mando == null ? 0f : mando.leftStick.ReadValue().x;
        entradaHorizontal = Mathf.Abs(stick) > Mathf.Abs(teclado) ? stick : teclado;
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
