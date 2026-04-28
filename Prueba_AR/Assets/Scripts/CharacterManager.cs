using UnityEngine;
using TMPro;
using System.Collections;

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
        
        BuscarPersonaje();

        gameManager = FindFirstObjectByType<GameManager>();
        debuggerSc = FindFirstObjectByType<DebuggerSc>();
    }

    public void PonerStats(CharactersSO charInfo)
    {
        characterInfo = charInfo;
        vida = characterInfo.vida;
        ataque = characterInfo.ataque;
        magia = characterInfo.magia;
        nombre = characterInfo.nombre;
        textoInfo.SetText($"HOLAAAAAAAA\nMagia: {magia}pts");
        StartCoroutine(ActualizarUI());
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
        int val_Prev = 0;
        if (tipo == 2)
        {
            return;
        }

        if (tipo == 0)
        {
            val_Prev = ataque;
            ataque = characterInfo.ataque;
            StartCoroutine(ActualizarUI(true, TipoCarta.Ataque, val_Prev));
        }
        else
        {
            val_Prev = magia;
            magia = characterInfo.magia;
            StartCoroutine(ActualizarUI(true, TipoCarta.Magia, val_Prev));
        }

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
        //debuggerSc.CambiarTexto(txt_sal, false);
        //CAMBIARSTATS
        int valPrev = 0;
        switch (tipo)
        {
            case TipoCarta.Vida:
                valPrev = vida;
                vida += cantidad;
                break;
            case TipoCarta.Magia:
                valPrev = magia;
                magia += cantidad;
                break;
            case TipoCarta.Ataque:
                valPrev = ataque;
                ataque += cantidad;
                break;
            default:
                break;
        }
        StartCoroutine(ActualizarUI(true, tipo, valPrev));
    }

    public void RecibirAtaque(int cantidad)
    {
        debuggerSc = FindFirstObjectByType<DebuggerSc>();
        string txt_sal = $"El jugador perdió {cantidad} de vida. Pasó de {vida} a {vida - cantidad}";
        //debuggerSc.CambiarTexto(txt_sal, true);
        //CAMBIARSTATS
        int valPrev = vida;
        vida -= cantidad;
        StartCoroutine(ActualizarUI(true, TipoCarta.Vida, valPrev));
    }

    public IEnumerator ActualizarUI(bool mark=false, TipoCarta tipo= TipoCarta.Magia, int valorPrev=0)
    {
        if (vida <= 0)
        {
            vida = 0;
            gameManager.CambioEstadoEspera(5, 4f);
            Debug.Log("Vita menore ziro");
            Morir();

            string txt_sal = "El jugador ha muerto";
            debuggerSc.CambiarTexto(txt_sal,false);

            Morir();
        }

        
        if (mark)
        {
            string color = "blue";
            switch (tipo)
            {
                case TipoCarta.Vida:
                    if (vida > valorPrev)
                        color = "green";
                    else
                        color = "red";
                    textoInfo.SetText($"{nombre}\nVida: {valorPrev} -> <color={color}>{vida}</color> pts\nAtaque: {ataque}pts\nMagia: {magia}pts");
                    break;
                case TipoCarta.Magia:
                    if (magia > valorPrev)
                        color = "green";
                    else
                        color = "red";
                    textoInfo.SetText($"{nombre}\nVida: {vida}pts\nAtaque: {ataque}pts\nMagia: {valorPrev} -> <color={color}>{magia}</color> pts");
                    break;
                case TipoCarta.Ataque:
                    if (ataque > valorPrev)
                        color = "green";
                    else
                        color = "red";
                    textoInfo.SetText($"{nombre}\nVida: {vida}pts\nAtaque: {valorPrev} -> <color={color}>{ataque}</color> pts\nMagia: {magia}pts");
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(3.8f);

        }

        textoInfo.SetText($"{nombre}\nVida: {vida}pts\nAtaque: {ataque}pts\nMagia: {magia}pts");

        Debug.Log($"Cambiando stats char: {nombre}\nVida: { vida} pts\nAtaque: { ataque} pts\nMagia: { magia} pts");
    }
}
