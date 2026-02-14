using UnityEngine;
using TMPro;


public class DebuggerSc : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoDeb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CambiarTexto("Iniciando Juego",true);
    }

    public void CambiarTexto(string texto, bool change)
    {
        if (change)
        {
            textoDeb.SetText(texto);
        }
        else
        {
            string text_prev = textoDeb.text;
            textoDeb.SetText(text_prev + "\n" + texto);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
