using UnityEngine;

[CreateAssetMenu(fileName = "CharactersSO", menuName = "Scriptable Objects/CharactersSO")]
public class CharactersSO : ScriptableObject
{
    public string nombre;
    public Sprite imagen;
    public int vida;
    public int ataque;
    public int magia;

    [TextArea(5, 10)]  public string Descripcion;

}
