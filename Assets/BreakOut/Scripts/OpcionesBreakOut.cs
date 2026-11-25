using UnityEngine;

[CreateAssetMenu(menuName = "BreakOut/Opciones", fileName = "OpcionesBreakOut")]
public class OpcionesBreakOut : ScriptableObject
{
    [SerializeField, Range(1, 3)] private int dificultad = 2;

    public int Dificultad => dificultad;
    public int ResistenciaBloques => dificultad;
    public float MultiplicadorVelocidad => dificultad == 1 ? 0.9f : dificultad == 3 ? 1.15f : 1f;
    public string NombreDificultad => dificultad == 1 ? "Fácil" : dificultad == 3 ? "Difícil" : "Normal";

    public void CambiarDificultad(int valor) => dificultad = Mathf.Clamp(valor, 1, 3);
}
