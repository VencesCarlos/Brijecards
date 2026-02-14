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
        nombre = enemyInfo.nombre;
        vida = enemyInfo.vida;
        ataque = enemyInfo.ataque;
        magia = enemyInfo.magia;

        debuggerSc = FindFirstObjectByType<DebuggerSc>();

        ActualizarUI();
        
        for (int i = 0; i < 3; i++)
        {
            CambiarCarta(i);
        }

        //Debug.Log("Mazo enemigo:" + CartasEscogidas[0].name + " - " + CartasEscogidas[1].name + " - " + CartasEscogidas[2].name + " - ");
    }

    // Update is called once per frame
    void Update()
    {
        
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
        string texto_Desc = $"El enemigo usa {cartaElegida.nombre}";
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
        sequence.Append(ImgCartaEnem.DOScale(1.2f, 0.2f).SetDelay(1.0f).OnComplete(HacerEfectoCarta));
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
        gameManager.CambioEstadoEspera(4, 3.5f);
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
        debuggerSc.CambiarTexto(txt_sal, false);
    }

    public void ReiniciarStatAtq(int tipo)
    {
        if (tipo == 2)
        {
            return;
        }

        if (tipo == 0)
        {
            ataque = enemyInfo.ataque;
        }
        else
        {
            magia = enemyInfo.magia;
        }
        ActualizarUI();
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
        vida -= cantidad;
        ActualizarUI();
    }

    public void ActualizarUI()
    {
        if (vida <= 0)
        {
            vida = 0;
            gameManager.CambioEstadoEspera(6, 0f);
            Morir();
        }
        textoInfo.SetText($"{nombre}\nVida: {vida}pts\nAtaque: {ataque}pts\nMagia: {magia}pts"); 
        Debug.Log($"Cambiando stats enem: { nombre}\nVida: { vida} pts\nAtaque: { ataque} pts\nMagia: { magia} pts");
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
