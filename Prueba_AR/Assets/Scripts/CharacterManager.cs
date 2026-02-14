using UnityEngine;
using TMPro;

public class CharacterManager : MonoBehaviour
{
    CharacterController characterP;
    [SerializeField] CharacterController[] characters;
    GameManager gameManager;

    DebuggerSc debuggerSc;

    public string nombre;
    public int vida;
    public int ataque;
    public int magia;

    [SerializeField] CharactersSO characterInfo;

    [SerializeField] TextMeshProUGUI textoInfo;

    private void Start()
    {
        vida = characterInfo.vida;
        ataque = characterInfo.ataque;
        magia = characterInfo.magia;
        nombre = characterInfo.nombre;
        BuscarPersonaje();
        ActualizarUI();

        gameManager = FindFirstObjectByType<GameManager>();
        debuggerSc = FindFirstObjectByType<DebuggerSc>();
    }

    public void BuscarPersonaje()
    {
        characters = Object.FindObjectsByType<CharacterController>(FindObjectsSortMode.None);
        foreach (var item in characters)
        {
            if (item.isPlayer)
            {
                characterP = item;
            }
        }
        //characterP = FindFirstObjectByType<CharacterController>();
        //characters = FindObjectsOfType<CharacterController>();
        //characters = FindAnyObjectByType<CharacterController>();
        if (characters == null)
        {
            Debug.Log("No se encontró nadota");
            return;
        }
    }

    public void Atacar()
    {
        /*foreach (var item in characters)
        {
            item.Atacar();
        }*/
        BuscarPersonaje();
        characterP.Atacar();
    }

    public void Morir()
    {
        /*foreach (var item in characters)
        {
            item.Morir();
        }*/
        BuscarPersonaje();
        characterP.Morir();
    }

    public void ReiniciarStatAtq(int tipo)
    {
        if (tipo == 2)
        {
            return;
        }

        if (tipo == 0)
        {
            ataque = characterInfo.ataque;
        }
        else
        {
            magia = characterInfo.magia;
        }
        ActualizarUI();
    }

    public void CambiarStats(TipoCarta tipo, int cantidad)
    {
        debuggerSc = FindFirstObjectByType<DebuggerSc>();
        string Aum = "";
        if (cantidad > 0)
        {
            Aum = "aumento";
        } else
        {
            Aum = "disminuyo";
        }
        string txt_sal = $"{nombre} {Aum} su {tipo} en {cantidad}";
        debuggerSc.CambiarTexto(txt_sal, false);
        switch (tipo)
        {
            case TipoCarta.Vida:
                vida += cantidad;
                break;
            case TipoCarta.Magia:
                magia += cantidad;
                break;
            case TipoCarta.Ataque:
                ataque += cantidad;
                break;
            default:
                break;
        }
        ActualizarUI();
    }

    public void RecibirAtaque(int cantidad)
    {
        debuggerSc = FindFirstObjectByType<DebuggerSc>();
        string txt_sal = $"El jugador perdió {cantidad} de vida. Pasó de {vida} a {vida - cantidad}";
        debuggerSc.CambiarTexto(txt_sal, true);
        vida -= cantidad;
        ActualizarUI();
    }

    public void ActualizarUI()
    {
        textoInfo.SetText($"{nombre}\nVida: {vida}pts\nAtaque: {ataque}pts\nMagia: {magia}pts");
        
        Debug.Log($"Cambiando stats char: {nombre}\nVida: { vida} pts\nAtaque: { ataque} pts\nMagia: { magia} pts");
        
        if (vida <= 0)
        {
            vida = 0;
            gameManager.CambioEstadoEspera(5, 0f);
            Debug.Log("Vita menore ziro");
            Morir();

            string txt_sal = "El jugador ha muerto";
            debuggerSc.CambiarTexto(txt_sal,false);

            Morir();
        }

    }
}
