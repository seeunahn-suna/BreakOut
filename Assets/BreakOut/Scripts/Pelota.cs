using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pelota : MonoBehaviour
{
    [SerializeField] private NivelBreakOut nivel;
    [SerializeField] private float velocidad = 9f;
    private Rigidbody2D cuerpo;

    public bool EnJuego { get; private set; }
    public float Altura => cuerpo.position.y;

    private void Awake()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        Detener();
    }

    public void Colocar(Vector2 posicion)
    {
        cuerpo.position = posicion;
    }

    public void Lanzar()
    {
        cuerpo.simulated = true;
        cuerpo.velocity = new Vector2(0.42f, 0.91f).normalized * velocidad;
        EnJuego = true;
    }

    public void Detener()
    {
        cuerpo.velocity = Vector2.zero;
        cuerpo.simulated = false;
        EnJuego = false;
    }

    private void FixedUpdate()
    {
        if (!EnJuego || cuerpo.velocity.sqrMagnitude == 0f)
            return;

        Vector2 direccion = cuerpo.velocity.normalized;
        if (Mathf.Abs(direccion.y) < 0.25f)
            direccion.y = Mathf.Sign(direccion.y) * 0.25f;
        cuerpo.velocity = direccion.normalized * velocidad;
    }

    private void OnCollisionEnter2D(Collision2D choque)
    {
        if (!EnJuego)
            return;

        if (choque.collider.TryGetComponent(out Paleta paleta) && cuerpo.position.y > paleta.Posicion.y)
        {
            float distancia = (cuerpo.position.x - paleta.Posicion.x) / paleta.Semiancho;
            cuerpo.velocity = new Vector2(Mathf.Clamp(distancia, -1f, 1f), 1f).normalized * velocidad;
        }

        if (choque.collider.TryGetComponent(out Bloque bloque) && bloque.RecibirImpacto())
            nivel.RegistrarBloqueRoto(bloque.Puntos);
    }
}
