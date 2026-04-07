using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] CharacterManager characterManager;

    CardsManager cardsManager;

    bool juegoTerminado;

    [SerializeField] RectTransform botonesAcciones_0;
    [SerializeField] RectTransform botonesAcciones_1;
    [SerializeField] RectTransform botonesAcciones_2;
    [SerializeField] Vector3 ArribaBotones;
    [SerializeField] RectTransform AbajoBotones;

    [SerializeField] RectTransform PantallaGameOver;
    [SerializeField] RectTransform PantallaGameSucces;

    [SerializeField] int estado = -1;

    [SerializeField] AudioSource MusFondo;
    [SerializeField] AudioSource GameOver;

    [SerializeField] GameObject BotonesPersonajes;
    [SerializeField] GameObject[] PersonajesObjs;

    DebuggerSc debuggerSc;

    [SerializeField] CharactersSO[] alebrijesScr;
    CharactersSO playerCarSO;
    [SerializeField] CharactersSO enemyCarSO;

    // 0 - Iniciando / 1 - Elegir carta / 2 - Elegir ataque / 3 - Esperar Carta / 4 - Esperar Ataque / 
    // 5 - GameOver / 6 - GameSucces

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DOTween.Init();

        //enemyManager = FindFirstObjectByType<EnemyManager>();
        //characterManager = FindFirstObjectByType<CharacterManager>();
        cardsManager = FindFirstObjectByType<CardsManager>();
        ArribaBotones = botonesAcciones_0.position;

        debuggerSc = FindFirstObjectByType<DebuggerSc>();
        //botonesAcciones.SetActive(false);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(botonesAcciones_0.DOMoveY(AbajoBotones.position.y, 0f));
        sequence.Append(botonesAcciones_1.DOMoveY(AbajoBotones.position.y, 0f));
        sequence.Append(botonesAcciones_2.DOMoveY(AbajoBotones.position.y, 0f));
        Debug.Log("Iniciando");
        //CambioEstadoEspera(-1, 0.5f);

        juegoTerminado = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UtilizarCarta(CardsSO cardsSO, bool sendByPlayer)
    {
        //Encontrar referencias
        //enemyManager = FindFirstObjectByType<EnemyManager>();
        //characterManager = FindFirstObjectByType<CharacterManager>();
        //characterManager = FindFirstObjectByType<CharacterManager>();
        //cardsManager = FindFirstObjectByType<CardsManager>();
        string nombCarta = "";
        
        //Hacer una interfaz o herencia para enemy y player(character[cambiar nombre])
        if (sendByPlayer)
        {
            nombCarta = $"El jugador usa {cardsSO.nombre}";
            debuggerSc.CambiarTexto(nombCarta, true);
            if (cardsSO.propio == true) //Al jugador
            {
                characterManager.CambiarStats(cardsSO.tipo, cardsSO.cantidad);
            }
            else //Al enemigo
            {
                enemyManager.CambiarStats(cardsSO.tipo, -cardsSO.cantidad);
            }
        }
        else
        {
            nombCarta = $"El enemigo usa \n{cardsSO.nombre}";
            debuggerSc.CambiarTexto(nombCarta, true);
            if (cardsSO.propio == true) //Al enemigo
            {
                enemyManager.CambiarStats(cardsSO.tipo, cardsSO.cantidad);
            }
            else //Al enemigo
            {
                characterManager.CambiarStats(cardsSO.tipo, -cardsSO.cantidad);
            }
        }
        //estado = 2;
    }

    //Botones Accion
    public void RealizarAccion(int accion)
    {
        string txt_sal = "";
        if (estado != 2)
        {
            return;
        }
        if (accion == 0)    // Atacar
        {
            enemyManager.RecibirAtaque(characterManager.ataque);
            characterManager.Atacar();
            txt_sal = "El jugador ataca";
        }
        else if (accion == 1)    // Magia
        {
            enemyManager.RecibirAtaque(characterManager.magia);
            characterManager.Atacar();
            txt_sal = "El jugador ataca con magia";
        }
        else if (accion == 2)    // Pasar
        {
            // Mostrar que no hace nada
            txt_sal = "El jugador pasó su turno";
        }
        characterManager.ReiniciarStatAtq(accion);
        CambioEstadoEspera(3, 4f);
        debuggerSc.CambiarTexto(txt_sal, false);
    }

    public void EnemigoRealizarAccion(int accion)
    {
        if (estado != 4)
        {
            return;
        }
        if (accion == 0)    // Atacar
        {
            characterManager.RecibirAtaque(enemyManager.ataque);
        }
        else if (accion == 1)    // Magia
        {
            characterManager.RecibirAtaque(enemyManager.magia);
        }
        else if (accion == 2)    // Pasar
        {
            // Mostrar que no hace nada
        }
        //characterManager.ReiniciarStatAtq(accion); ESTO YA NO
        CambioEstadoEspera(1, 3f);
    }

    public void ElegirPersonaje(int pers)
    {
        BotonesPersonajes.SetActive(false);
        PersonajesObjs[pers].SetActive(true);
        //Aparecer modelo de personaje
        CambioEstadoEspera(1, 3f);
    }

    public void IniciarJuego(int pAl, int eAl)
    {
        playerCarSO = alebrijesScr[pAl];
        enemyCarSO = alebrijesScr[eAl];
        characterManager.PonerStats(playerCarSO);
        enemyManager.PonerStats(enemyCarSO);
        CambioEstadoEspera(1, 3f);
    }

    public void CambioEstadoEspera(int nuevoEstado, float tiempo)
    {

        //CambioEstado(nuevoEstado);
        // Habrá estado 0?
        /*if (nuevoEstado >= 5 || nuevoEstado < 0)
        {
            nuevoEstado = 1;
        }*/
        StartCoroutine(CambioEstado(nuevoEstado, tiempo));

    }

    public IEnumerator CambioEstado(int nuevoEstado, float tiempo)
    {
        estado = nuevoEstado;
        yield return new WaitForSeconds(tiempo);

        Debug.Log("Cambiando al estado: " + nuevoEstado);

        if (juegoTerminado)
        {
            yield break;
        }
        // -1 - Iniciando / 1 - Elegir carta / 2 - Elegir ataque / 3 - Esperar Carta / 4 - Esperar Ataque /
        // 5 - GameOver / 6 - GameSucces        0 - Eligiendo personaje

        switch (nuevoEstado)
        {
            case -1:
                CambioEstadoEspera(1, 3f);
                break;
            case 0:

                break;
            case 1: //Inicio juego aparecen cartas (esperando carta)
                cardsManager.StartCoroutine("AparecerCartas");
                // -----------------
                break;
            case 2: // Desaparecen cartas y aparecen botones (esperando accion jugador)
                cardsManager.StartCoroutine("DesaparecerCartas");
                //botonesAcciones.SetActive(true);
                Sequence sequence_C2 = DOTween.Sequence();
                sequence_C2.Append(botonesAcciones_0.DOMoveY(ArribaBotones.y, 0.4f).SetDelay(0.1f));
                sequence_C2.Join(botonesAcciones_1.DOMoveY(ArribaBotones.y, 0.4f).SetDelay(0.15f));
                sequence_C2.Join(botonesAcciones_2.DOMoveY(ArribaBotones.y, 0.4f).SetDelay(0.15f));
                break;
            case 3: // Desaparecen botones y el enemigo elige carta
                Sequence sequence_C3 = DOTween.Sequence();
                sequence_C3.Append(botonesAcciones_0.DOMoveY(AbajoBotones.position.y, 0.4f).SetDelay(0.1f));
                sequence_C3.Join(botonesAcciones_1.DOMoveY(AbajoBotones.position.y, 0.4f).SetDelay(0.15f));
                sequence_C3.Join(botonesAcciones_2.DOMoveY(AbajoBotones.position.y, 0.4f).SetDelay(0.15f));
                enemyManager.EscogerCarta();
                break;
            case 4: // El enemigo elige accion
                enemyManager.EscogerAccion();
                break;
            case 5: // Muerte del jugador
                Debug.Log("Ya me voy a morir");
                Sequence sequence_C5 = DOTween.Sequence();
                sequence_C5.Append(PantallaGameOver.DOLocalMoveY(0f, 0.4f).SetDelay(4.0f));
                Debug.Log("Ya me mori");
                juegoTerminado = true;
                StartCoroutine(GoToMenu());

                break; // Jugador gana
            case 6:
                Sequence sequence_C6 = DOTween.Sequence();
                sequence_C6.Append(PantallaGameSucces.DOLocalMoveY(0f, 0.4f).SetDelay(4.0f));
                juegoTerminado = true;
                StartCoroutine(GoToMenu());
                break;
            default:
                break;
        }
    }

    public IEnumerator GoToMenu()
    {
        MusFondo.enabled = false;
        GameOver.enabled = true;
        GameOver.Play();

        
        yield return new WaitForSeconds(10f);

        SceneManager.LoadScene(0);
    }

}
