using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
public class UI_Menu_Inicio : MonoBehaviour
{
    public RectTransform MenuAjustes;
    public GameObject PanelFondoConfig;
    //public RectTransform PanelGaleria;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DOTween.Init();
        CerrarAjustes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IniciarJuego()
    {
        SceneManager.LoadScene(1);  //1 - AR        / 2 - No AR :)
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
        SceneManager.LoadScene(3);

    }
}
