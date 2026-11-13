using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Paleta : MonoBehaviour
{
    [SerializeField] private float velocidad = 12f;
    [SerializeField] private float limiteHorizontal = 7.1f;
    private Rigidbody2D cuerpo;

    public Vector2 Posicion => cuerpo.position;
    public float Semiancho => transform.localScale.x / 2f;

    private void Awake()
    {
        cuerpo = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float x = cuerpo.position.x + Input.GetAxisRaw("Horizontal") * velocidad * Time.fixedDeltaTime;
        x = Mathf.Clamp(x, -limiteHorizontal, limiteHorizontal);
        cuerpo.MovePosition(new Vector2(x, cuerpo.position.y));
    }
}
