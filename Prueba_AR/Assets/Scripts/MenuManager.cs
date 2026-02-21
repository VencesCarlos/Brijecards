using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject PanelPausa;
    [SerializeField] GameObject PanelConfig;
    [SerializeField] float escalaDeTiempo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        escalaDeTiempo = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MostrarMenu()
    {
        escalaDeTiempo = Time.timeScale;
        Time.timeScale = 0f;
        PanelPausa.SetActive(true);
    }

    public void OcultarMenu()
    {
        Time.timeScale = escalaDeTiempo;
        PanelPausa.SetActive(false);
    }

    public void SalirAlInicio()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void MostrarConfig()
    {
        PanelConfig.SetActive(true);
    }
    
    public void OcultarConfig()
    {
        PanelConfig.SetActive(false);
    }
}
