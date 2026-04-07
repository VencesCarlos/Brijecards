using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;

public class EnemyManager : MonoBehaviour
{
    public string nombre;
    public int vida;
    public int ataque;
    public int magia;

    private int eleccion_carta;
    private CardsSO cartaElegida;

    [SerializeField] GameManager gameManager;

    [SerializeField] CharactersSO enemyInfo;


    CharacterController character_Enem;
    [SerializeField] CharacterController[] characters;

    int[] prob_Cartas = { 40, 50, 10 };
    int[] prob_Accion = { 20, 20, 60 };

    [SerializeField] CardsSO[] posiblesCartas;
    [SerializeField] CardsSO[] CartasEscogidas;
    
    [SerializeField] RectTransform ImgCartaEnem;
    [SerializeField] RectTransform PosicionCentro;
    [SerializeField] RectTransform PosicionFuera;

    /*
    ---- Nivel 1: -----
[ELEGIR CARTA]
    BUFFO = 40%
    DEBUFFO = 50%
    CURACION/CURACION'T = 10%

[ACCIÓN]
    ATACAR ATQ = 20%
    ATACAR MGA = 20%
    PASAR = 60%

---- Nivel 2: -----
[ELEGIR CARTA]
BUFFO = 50%
DEBUFFO = 20%
CURACION/CURACION'T = 30%


[ACCIÓN]
    ATACAR ATQ = 25%
    ATACAR MGA = 25%
    PASAR = 50%


---- Nivel 3: -----
[ELEGIR CARTA]
    BUFFO = 25%
    DEBUFFO = 25%
    CURACION/CURACION'T = 50%

[ACCIÓN]
    ATACAR ATQ = 35%
    ATACAR MGA = 35%
    PASAR = 30%
     */

    [SerializeField] TextMeshProUGUI textoInfo;

    DebuggerSc debuggerSc;

    //Los valores de arriba se deberían obtener del gameobject o algo así

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        DOTween.Init();

        debuggerSc = FindFirstObjectByType<DebuggerSc>();

        //ActualizarUI();
        
        for (int i = 0; i < 3; i++)
        {
            CambiarCarta(i);
        }

        //Debug.Log("Mazo enemigo:" + CartasEscogidas[0].name + " - " + CartasEscogidas[1].name + " - " + CartasEscogidas[2].name + " - ");
    }

    public void PonerStats(CharactersSO charInfo)
    {
        enemyInfo = charInfo;
        vida = enemyInfo.vida;
        ataque = enemyInfo.ataque;
        magia = enemyInfo.magia;
        nombre = enemyInfo.nombre;
        StartCoroutine(ActualizarUI());
    }

    public void CambiarCarta(int carta) // carta 0-2
    {
        int eleccion = Random.Range(0, posiblesCartas.Length);
        CartasEscogidas[carta] = posiblesCartas[eleccion];
        //Aqui agregar logica de probabilidades
    }

    public void EscogerCarta()
    {
        if (vida <= 0)
        {
            return;
        }
        eleccion_carta = Random.Range(0,3);
        cartaElegida = CartasEscogidas[eleccion_carta];
        Debug.Log("Carta elegida num:" + eleccion_carta + "  Y sus propiedades es: " + cartaElegida.nombre);
        //Debug.Log("Mazo enemigo:" + CartasEscogidas[0].name + " - " + CartasEscogidas[1].name + " - " + CartasEscogidas[2].name + " - ");
        //Agregar Animacion de carta
        //Luego de la animacion (o a la par llamar a )
        string texto_Desc = $"El enemigo usa \n{cartaElegida.nombre}";
        debuggerSc.CambiarTexto(texto_Desc, true);
        StartCoroutine(MostrarCarta());
    }

    public IEnumerator MostrarCarta()
    {
        yield return new WaitForSeconds(3.0f);
        ImgCartaEnem.GetComponent<UnityEngine.UI.Image>().sprite = cartaElegida.imagen;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(ImgCartaEnem.DOMove(PosicionCentro.position, 0.3f)); //On complete Poner hacer la accion
        sequence.Append(ImgCartaEnem.DOScale(0.8f, 0.2f));
        sequence.Append(ImgCartaEnem.DOScale(1.2f, 0.2f).SetDelay(0.9f).OnComplete(HacerEfectoCarta));
        sequence.Append(ImgCartaEnem.DOMove(PosicionFuera.position, 1.0f).SetDelay(3.0f).OnComplete(TerminarTurnoCarta));
        sequence.Join(ImgCartaEnem.DOScale(0.3f, 0.5f));

    }

    private void HacerEfectoCarta()
    {
        gameManager.UtilizarCarta(cartaElegida, false);
        CambiarCarta(eleccion_carta);
    }

    private void TerminarTurnoCarta()
    {
        gameManager.CambioEstadoEspera(4, 4f);
    }

    public void EscogerAccion()
    {
        //Aqui agregar logica de acciones
        int eleccion = Random.Range(0, 3);
        gameManager.EnemigoRealizarAccion(eleccion);
        ReiniciarStatAtq(eleccion);
        string accion = "";
        if (eleccion == 0)
        {
            accion = "ataca";
            Atacar();
        }
        else if (eleccion == 1)
        {
            accion = "ataca con magia";
            Atacar();
        }
        else
        {
            accion = "pasó su turno";
        }
        Debug.Log("Accion elegida num:" + accion);
        string txt_sal = $"El enemigo {accion}";
        debuggerSc.CambiarTexto(txt_sal, true);
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
            ataque = enemyInfo.ataque;
            StartCoroutine(ActualizarUI(true, TipoCarta.Ataque, val_Prev));
        }
        else
        {
            val_Prev = magia;
            magia = enemyInfo.magia;
            StartCoroutine(ActualizarUI(true, TipoCarta.Magia, val_Prev));
        }
    }

    public void CambiarStats(TipoCarta tipo, int cantidad)
    {
        string Aum = "";
        if (cantidad > 0)
        {
            Aum = "aumento";
        }
        else
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
        int valPrev = vida;
        vida -= cantidad;
        StartCoroutine(ActualizarUI(true, TipoCarta.Vida, valPrev));
    }

    public IEnumerator ActualizarUI(bool mark = false, TipoCarta tipo = TipoCarta.Magia, int valorPrev = 0)
    {
        if (vida <= 0)
        {
            vida = 0;
            gameManager.CambioEstadoEspera(6, 4f);
            Morir();
        }
        if (mark)
        {
            switch (tipo)
            {
                case TipoCarta.Vida:
                    textoInfo.SetText($"{nombre}\nVida: {valorPrev} -> {vida} pts\nAtaque: {ataque}pts\nMagia: {magia}pts");
                    break;
                case TipoCarta.Magia:
                    textoInfo.SetText($"{nombre}\nVida: {vida}pts\nAtaque: {ataque}pts\nMagia: {valorPrev} -> {magia} pts");
                    break;
                case TipoCarta.Ataque:
                    textoInfo.SetText($"{nombre}\nVida: {vida}pts\nAtaque: {valorPrev} -> {ataque} pts\nMagia: {magia}pts");
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(3.0f);

        }
        textoInfo.SetText($"{nombre}\nVida: {vida}pts\nAtaque: {ataque}pts\nMagia: {magia}pts");

        Debug.Log($"Cambiando stats char: {nombre}\nVida: { vida} pts\nAtaque: { ataque} pts\nMagia: { magia} pts");
    }

    public void Atacar()
    {
        BuscarCharacters();
        character_Enem.Atacar();
    }

    public void Morir()
    {
        BuscarCharacters();
        character_Enem.Morir();
    }

    public void BuscarCharacters()
    {
        characters = Object.FindObjectsByType<CharacterController>(FindObjectsSortMode.None);
        foreach (var item in characters)
        {
            if (!item.isPlayer)
            {
                character_Enem = item;
            }
        }
    }
}
