using UnityEngine;

[CreateAssetMenu(menuName = "BreakOut/Puntaje alto", fileName = "PuntajeAlto")]
public class PuntajeAlto : ScriptableObject
{
    [SerializeField] private string identificador;
    private int valor;

    public string Identificador => identificador;
    public int Valor => valor;

    public void Cargar(int guardado) => valor = Mathf.Max(0, guardado);

    public void Registrar(int puntos) => valor = Mathf.Max(valor, puntos);
}
