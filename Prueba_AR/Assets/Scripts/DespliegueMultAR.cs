using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DespliegueMultAR : MonoBehaviour
{
    //Prefabs
    [SerializeField] List<GameObject> prefabsToSpawn = new List<GameObject>();

    private ARTrackedImageManager _trackedImageManager;

    private Dictionary<string, GameObject> _arObjects;


    private void Start()
    {
        ARSession session = FindFirstObjectByType<ARSession>();
        if (session != null)
        {
            session.Reset();
            //Debug.Log("---------Reseteando---------");
        }

        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        _trackedImageManager.enabled = false;
        _trackedImageManager.enabled = true;
        if (_trackedImageManager == null) return;
        _trackedImageManager.trackablesChanged.AddListener(OnImagesTrackedChanged);
        _arObjects = new Dictionary<string, GameObject>();
        SetupSceneElements();
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
            return;
        }

        //Activar objeto y ubicar sobre la carta
        _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(true);
        _arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        _arObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
        //Transform copiaTr = _arObjects[trackedImage.referenceImage.name].transform;
        //_arObjects[trackedImage.referenceImage.name].transform.localRotation = Quaternion.Euler(copiaTr.localRotation.x, copiaTr.localRotation.y, copiaTr.localRotation.z);
        //_arObjects[trackedImage.referenceImage.name].transform.localRotation = Quaternion.Euler(trackedImage.transform.localRotation.x-90, trackedImage.transform.localRotation.y + 90, trackedImage.transform.localRotation.z);
    }
}
