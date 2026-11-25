using UnityEngine;

public class Bloque : MonoBehaviour
{
    [SerializeField] private OpcionesBreakOut opciones;
    private bool destruido;
    private int impactosRestantes;
    private int resistenciaInicial;
    private SpriteRenderer cara;
    private Color colorInicial;

    public virtual int Puntos => 100;
    public virtual bool EsDestructible => true;
    public int ImpactosRestantes => impactosRestantes;

    private void Awake()
    {
        resistenciaInicial = opciones == null ? 1 : opciones.ResistenciaBloques;
        impactosRestantes = resistenciaInicial;
        Transform frente = transform.Find("Cara");
        cara = frente == null ? GetComponent<SpriteRenderer>() : frente.GetComponent<SpriteRenderer>();
        colorInicial = cara.color;
    }

    public bool RecibirImpacto()
    {
        if (!EsDestructible || destruido)
            return false;

        impactosRestantes--;
        if (impactosRestantes > 0)
        {
            float dano = 1f - (float)impactosRestantes / resistenciaInicial;
            cara.color = Color.Lerp(colorInicial, Color.white, dano * 0.65f);
            return false;
        }

        destruido = true;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
        return true;
    }
}
