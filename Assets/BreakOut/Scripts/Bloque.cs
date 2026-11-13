using UnityEngine;

public class Bloque : MonoBehaviour
{
    private bool destruido;

    public virtual int Puntos => 100;
    public virtual bool EsDestructible => true;

    public bool RecibirImpacto()
    {
        if (!EsDestructible || destruido)
            return false;

        destruido = true;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
        return true;
    }
}
