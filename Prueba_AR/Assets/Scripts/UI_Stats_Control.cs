using UnityEngine;
using TMPro;


public class UI_Stats_Control : MonoBehaviour
{
    public int vida;
    private float contador;
    public TextMeshPro textoInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida = 5;
        contador = 10;
        textoInfo.SetText($"Brije Card Name\nVida: {vida}pts\nAtaque: 2pts\nDefensa: 5pts");
    }

    // Update is called once per frame
    void Update()
    {
        contador -= Time.deltaTime;
        if (contador <= 0)
        {
            contador = 10;
            vida++;
            if (vida >= 15)
            {
                vida = 0;
            }
            textoInfo.SetText($"Brije Card Name\nVida: {vida}pts\nAtaque: 2pts\nDefensa: 5pts");
        }
    }
}
