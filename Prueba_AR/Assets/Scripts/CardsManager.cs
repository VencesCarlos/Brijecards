using UnityEngine;
using System.Collections;

public class CardsManager : MonoBehaviour
{
    [SerializeField] CardsSO[] posiblesCartas;
    [SerializeField] UI_Cartas[] CartasEnJuego;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < CartasEnJuego.Length; i++)
        {
            int eleccion = Random.Range(0, CartasEnJuego.Length);
            CartasEnJuego[i].CambiarCarta(posiblesCartas[eleccion]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CambiarCarta(UI_Cartas uI_Cartas)
    {
        int eleccion = Random.Range(0, posiblesCartas.Length);
        //Debug.Log(eleccion);
        uI_Cartas.CambiarCarta(posiblesCartas[eleccion]);
    }

    public void CambiarEstadoCartas(bool estado)
    {
        for (int i = 0; i < CartasEnJuego.Length; i++)
        {
            CartasEnJuego[i].Interactable = estado;
        }
    }

    public void QuitarCartas()
    {

    }

    IEnumerator AparecerCartas()
    {
        yield return new WaitForSeconds(3.0f);
        for (int i = 0; i < CartasEnJuego.Length; i++)
        {
            yield return new WaitForSeconds(.2f);
            CartasEnJuego[i].UbicarCartas();
        }
    }

    IEnumerator DesaparecerCartas()
    {
        //yield return new WaitForSeconds(2.1f);

        for (int i = 0; i < CartasEnJuego.Length; i++)
        {
            yield return new WaitForSeconds(.2f);
            CartasEnJuego[i].QuitarCartas();
        }
    }
}
