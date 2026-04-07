using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UI_Cartas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool Interactable;
    RectTransform ImgCarta;
    [SerializeField] private Transform PosOr_Ar;
    [SerializeField] private Transform PosOr_Ab;
    private Vector3 PosicionOriginal_Ar;
    private Vector3 PosicionOriginal_Ab;
    private Vector3 RotacionOriginal;
    //private Sprite Img;
    private CardsSO cartaActual;
    [SerializeField] private CardsManager cardsManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RectTransform PosicionCentro;

    [SerializeField] private float distUpY = 25.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DOTween.Init();
        ImgCarta = gameObject.GetComponent<RectTransform>();
        //Debug.Log(ImgCarta.position.y);
        //Debug.Log(ImgCarta.localPosition.y);

        /*
        Debug.Log("PosicionOriginal WP " + ImgCarta.position);
        Debug.Log("PosicionOriginal LP " + ImgCarta.localPosition);

        Debug.Log("PosicionDestino WP " + PosOr_Ar.position);
        Debug.Log("PosicionDestino LP " + PosOr_Ar.localPosition);

        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA

        Hacer que las posiciones se recalculen o algo así,a demás, el mov de carta seleccionada debe ser calculado, de lo contrario
        se ve afectado por la resolución de la imagen
        */

        PosicionOriginal_Ar = PosOr_Ar.position;
        PosicionOriginal_Ab = PosOr_Ab.position;
        
        RotacionOriginal = ImgCarta.localEulerAngles;

        ImgCarta.position = PosicionOriginal_Ab;

        cardsManager = FindFirstObjectByType<CardsManager>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Interactable) return;

        //gameObject.SetActive(false);

        //Debug.Log("¡Ratón sobre la imagen!");
        // Por ejemplo, cambiar el color de la imagen
        GetComponent<UnityEngine.UI.Image>().color = new Color32(184,230,230,255);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(ImgCarta.DOMoveY(PosicionOriginal_Ar.y + distUpY, 0.1f));
        //sequence.Append(ImgCarta.DOScale(1f, 0.3f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!Interactable) return;

        //Debug.Log("Ratón ha salido de la imagen.");
        // Devolver el color original (blanco por defecto para imágenes de UI)
        GetComponent<UnityEngine.UI.Image>().color = Color.white;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(ImgCarta.DOMoveY(PosicionOriginal_Ar.y, 0.1f));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Interactable) return;

        //if (eventData.button == PointerEventData.InputButton.Left)
        if (true)
        {

            EfectoCarta(); // LLamar game manager y hacer cambios
            
            //gameObject.SetActive(false);
            //cardsManager = FindFirstObjectByType<CardsManager>();
            //Debug.Log("¡Click Izquierdo en la imagen!");
            // Poner en el centro de la imagen
            Interactable = false;
            cardsManager.CambiarEstadoCartas(false);
            GetComponent<UnityEngine.UI.Image>().color = Color.white;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(ImgCarta.DOMove(PosicionCentro.position,0.2f));
            sequence.Join(ImgCarta.DOLocalRotate(new Vector3(0,0,0), 0.2f));
            sequence.Join(ImgCarta.DOScale(1.4f, 0.2f));
            sequence.Append(ImgCarta.DOLocalMoveY(-300, 0.2f).SetDelay(1.5f).OnComplete(SolicitarCambiarCarta));
            sequence.Append(ImgCarta.DOLocalRotate(RotacionOriginal, 0.2f));
            sequence.Join(ImgCarta.DOMoveX(PosicionOriginal_Ar.x, 0.2f));
            sequence.Join(ImgCarta.DOScale(0.8f, 0.2f));
            //sequence.Append(ImgCarta.DOLocalMoveY(PosicionOriginal_Ar.y, 0.2f).OnComplete(ActivarCartas));
            sequence.Append(ImgCarta.DOMoveY(PosicionOriginal_Ar.y, 0.2f));
            //LLamar al control para desactivar las otras cartas
        }
        /*
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //Debug.Log("¡Click Derecho en la imagen!");
            // Ejecutar la acción secundaria (Mostrar imagen completa) 
            // QUITAR - no funciona en movil
        }*/
    }

    public void UbicarCartas()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(ImgCarta.DOMoveY(PosicionOriginal_Ar.y, 0.2f).OnComplete(ActivarCartas));
    }

    public void QuitarCartas()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(ImgCarta.DOMoveY(PosicionOriginal_Ab.y, 0.2f));
    }

    public void CambiarCarta(CardsSO cardsSO)
    {
        cartaActual = cardsSO;
        GetComponent<UnityEngine.UI.Image>().sprite = cartaActual.imagen;
    }

    private void ActivarCartas()
    {
        //Interactable = true;
        cardsManager.CambiarEstadoCartas(true);
    }

    private void SolicitarCambiarCarta()
    {
        cardsManager.CambiarCarta(this);
    }

    private void EfectoCarta()
    {
        //gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.UtilizarCarta(cartaActual, true);
            gameManager.CambioEstadoEspera(2, 4f);

        }
        //gameObject.SetActive(false);
        //gameManager.StartCoroutine(CambioEstadoEspera(2, 2.1f));
        //gameManager.CambioEstado(2, 2.1f);
    }
}
