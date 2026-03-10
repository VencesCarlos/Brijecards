using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
public class UI_Menu_Inicio : MonoBehaviour
{
    public RectTransform MenuAjustes;
    public GameObject PanelFondoConfig;
    public GameObject BotonSiguiente;
    public GameObject PanelTutorial;
    public GameObject[] Tutorial;
    int tutorialCont;


    public Toggle MostrarTutoToggle;
    //public RectTransform PanelGaleria;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DOTween.Init();
        CerrarAjustes();
        tutorialCont = 0;
        foreach (GameObject item in Tutorial)
        {
            item.SetActive(false);
        }
        PanelTutorial.SetActive(false);
        BotonSiguiente.SetActive(false);
        if (PlayerPrefs.GetInt("TutoAlways") == 1)
        {
            MostrarTutoToggle.isOn = true;
        }
        else
        {
            MostrarTutoToggle.isOn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IniciarJuego()
    {
        //No ha jugado
        if (PlayerPrefs.GetInt("Juego_Iniciado") == 0 || PlayerPrefs.GetInt("TutoAlways") == 1)
        {
            PlayerPrefs.SetInt("Juego_Iniciado", 1);
            PanelTutorial.SetActive(true);
            BotonSiguiente.SetActive(true);
            MostrarTutorial();
        }
        else
        {
            SceneManager.LoadScene(1);  //1 - AR        / 2 - No AR :)
        }
    }

    public void MostrarTutorial()
    {
        if (tutorialCont >= Tutorial.Length)
        {
            SceneManager.LoadScene(1);
            return;
        }
        Tutorial[tutorialCont].SetActive(true);
        tutorialCont++;
    }

    public void ToggleTutorial(bool togle)
    {
        if (togle)
        {
            PlayerPrefs.SetInt("TutoAlways", 1);
        }
        else
        {
            PlayerPrefs.SetInt("TutoAlways", 0);

        }
    }

    public void AbrirAjustes()
    {
        //Debug.Log("Abrir ajustes");
        //DOTween.Init();
        Sequence sequence = DOTween.Sequence();
        PanelFondoConfig.SetActive(true);
        sequence.Append(MenuAjustes.DOLocalMoveY(0, 0.1f));
        sequence.Append(MenuAjustes.DOScale(1f, 0.3f));
    }

    public void CerrarAjustes()
    {
        //Debug.Log("Cerrar ajustes");
        //DOTween.Init();
        Sequence sequence = DOTween.Sequence();
        PanelFondoConfig.SetActive(false);
        sequence.Append(MenuAjustes.DOScale(0.1f, 0.2f));
        sequence.Join(MenuAjustes.DOLocalMoveY(800, 0.1f).SetDelay(0.1f));
    }

    public void SalirApp()
    {
        Debug.Log("Salir APP");
        Application.Quit();

    }

    public void AbrirGaleria()
    {
        Debug.Log("Abrir galeria u otras cosas");

    }

    public void ReiniciarPP()
    {
        PlayerPrefs.SetInt("Juego_Iniciado", 0);
        PlayerPrefs.SetInt("TutoAlways", 0);
    }
}
