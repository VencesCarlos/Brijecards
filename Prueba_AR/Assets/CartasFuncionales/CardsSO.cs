using UnityEngine;

[CreateAssetMenu(fileName = "CardsSO", menuName = "Scriptable Objects/CardsSO")]
public class CardsSO : ScriptableObject
{
    public string nombre;
    public Sprite imagen;
    public TipoCarta tipo;
    public int cantidad;
    public bool propio;
    public string descripcion;
}

public enum TipoCarta
{
    Vida,
    Magia,
    Ataque
}

public enum TipoAtaque
{
    Normal,
    Magia
}