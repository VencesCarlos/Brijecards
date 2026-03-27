using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RevisionScript : MonoBehaviour
{
    public CharactersSO[] personajesScr;
    [SerializeField] CharactersSO perAct;
    [SerializeField] GameObject[] personajesEnEscena;

    Vector2 startPos;
    Vector2 actualPosition;
    Vector2 swipe;

    bool touching;
    float despProp;
    float rotacionOriginal;
    [SerializeField] float factRot = 200;

    [SerializeField] Transform posicionCilindro;

    [SerializeField] TextMeshProUGUI textoInfo;
    [SerializeField] Image imagenCarta;
    [SerializeField] Sprite[] imgsCartas;

    [Header("Camara")]
    [SerializeField] Slider sliderZ;
    [SerializeField] Transform cameraT;
    [SerializeField] Vector3 PosicionOriginalCamera;
    [SerializeField] float factorZoomAum;
    [SerializeField] float factorZoomDis;

    [Header("Paneles")]
    [SerializeField] GameObject panelInfo;
    [SerializeField] GameObject panelSeleccion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        despProp = 0;
        rotacionOriginal = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            touching = true;
            rotacionOriginal = transform.localEulerAngles.y;
        }

        if (Input.GetMouseButtonUp(0))
        {
            touching = false;
        }

        if (touching)
        {
            CalcularDesplazamiento();
        }
    }

    public void CambiarPersonaje(int personaje)
    {
        panelSeleccion.SetActive(false);
        MostrarInfo(false);
        sliderZ.value = 0;

        foreach (GameObject obj in personajesEnEscena)
        {
            obj.SetActive(false);
        }

        if (personaje == 2)
        {
            posicionCilindro.localPosition = new Vector3(0, -5.5f, 0);
        }
        else
        {
            posicionCilindro.localPosition = new Vector3(0, -1.18f, 0);
        }

        perAct = personajesScr[personaje];
        personajesEnEscena[personaje].SetActive(true);

        textoInfo.text = $"Nombre: {perAct.nombre}\nVida: {perAct.vida}\nAtaque: {perAct.ataque}\nMagia: {perAct.magia}\nDescripción: {perAct.Descripcion}";
        imagenCarta.sprite = imgsCartas[personaje];
    }

    void CalcularDesplazamiento()
    {
        actualPosition = Input.mousePosition;
        swipe = startPos - actualPosition;
        despProp = swipe.x / Screen.width;
        //Debug.Log("RotO: " + rotacionOriginal + "DespP: " + despProp + "Camb: " + (rotacionOriginal + (despProp * factRot)));
        transform.rotation = Quaternion.Euler(0, rotacionOriginal + (despProp * factRot), 0);
    }

    public void MostrarInfo(bool activar)
    {
        panelInfo.SetActive(activar);
        if (!activar)
        {
            cameraT.position = new Vector3(PosicionOriginalCamera.x, PosicionOriginalCamera.y, cameraT.position.z);
        }
        else
        {
            cameraT.position = new Vector3(2.5f, PosicionOriginalCamera.y, cameraT.position.z);
        }
    }

    public void ElegirCarta(bool activar)
    {
        panelSeleccion.SetActive(activar);
    }

    public void cambiarZoom(float distancia)
    {
        if (distancia > 0)
        {
            cameraT.position = new Vector3(cameraT.position.x, PosicionOriginalCamera.y, PosicionOriginalCamera.z + (distancia*factorZoomAum));
        }
        else
        {
            cameraT.position = new Vector3(cameraT.position.x, PosicionOriginalCamera.y, PosicionOriginalCamera.z + (-(distancia* distancia) * factorZoomDis));
        }
    }

    public void Salir()
    {
        SceneManager.LoadScene(0);
    }

    public void MandarAr()
    {
        SceneManager.LoadScene(4);
    }
}
