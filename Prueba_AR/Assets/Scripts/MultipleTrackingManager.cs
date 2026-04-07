using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleTrackingManager : MonoBehaviour
{
    //Prefabs
    [SerializeField] List<GameObject> prefabsToSpawn = new List<GameObject>();

    private ARTrackedImageManager _trackedImageManager;

    private Dictionary<string, GameObject> _arObjects;

    GameManager gameManager;

    //public GameObject prueba;

    bool isInCamera;
    bool isStarted;
    [SerializeField] GameObject UISearching;
    [SerializeField] TextMeshProUGUI textoCargando;
    float cronom;

    private void Start()
    {
        ARSession session = FindFirstObjectByType<ARSession>();
        if (session != null)
        {
            session.Reset();
        }

        gameManager = FindFirstObjectByType<GameManager>();

        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        _trackedImageManager.enabled = false;
        _trackedImageManager.enabled = true;
        if (_trackedImageManager == null) return;
        _trackedImageManager.trackablesChanged.AddListener(OnImagesTrackedChanged);
        _arObjects = new Dictionary<string, GameObject>();
        SetupSceneElements();
        isInCamera = false;
        isStarted = false;
        UISearching.SetActive(true);
        cronom = 0f;
    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        _trackedImageManager.trackablesChanged.RemoveListener(OnImagesTrackedChanged);
    }

    private void SetupSceneElements()
    {
        foreach (var prefab in prefabsToSpawn)
        {
            var arObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            arObject.name = prefab.name;
            
            GenEnemigo genEnemigo = arObject.gameObject.transform.GetComponentInChildren<GenEnemigo>();
            genEnemigo.MostrarEnemigo(PlayerPrefs.GetInt("EnemyID"));
            
            arObject.gameObject.SetActive(false);

            
            _arObjects.Add(arObject.name, arObject);
        }
    }

    private void OnImagesTrackedChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateTrackedImages(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateTrackedImages(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            UpdateTrackedImages(trackedImage.Value);
        }
    }

    private void UpdateTrackedImages(ARTrackedImage trackedImage)
    {
        if (trackedImage == null) return;
        if (trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
            cronom += Time.deltaTime;
            Debug.Log("TIempo transcurrido: " + cronom);
            if (!isInCamera)
            {
                return;
            }
            Time.timeScale = 0f;
            isInCamera = false;
            UISearching.SetActive(true);
            return;
        }

        if (!isStarted)
        {
            isStarted = true;
            int enemyID = PlayerPrefs.GetInt("EnemyID");
            switch (trackedImage.referenceImage.name)
            {
                case "Brije1_Game":
                    gameManager.IniciarJuego(0, enemyID);
                    break;
                case "Brije3_Game":
                    gameManager.IniciarJuego(2, enemyID);
                    break;
                case "Brije2_Game":
                    gameManager.IniciarJuego(1, enemyID);
                    break;
                default:
                    gameManager.IniciarJuego(2, enemyID);
                    break;
            }
            
            
            UISearching.SetActive(false);
        }
        
        //Activar objeto y ubicar sobre la carta
        _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(true);
        _arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        _arObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
        

        //prueba.SetActive(true);
        if (isInCamera)
        {
            return;
        }
        // Cambio y "apareció"
        Time.timeScale = 1f;
        UISearching.SetActive(false);



        isInCamera = true;
        //prueba.transform.localScale = new Vector3(prueba.transform.localScale.x+1f, prueba.transform.localScale.y + 1f, prueba.transform.localScale.z + 1f);
        //gameManager.CambioEstadoEspera(1, 5f); (aca esto cambiar)

        // Falta un arranque para el inicio
    }
}
