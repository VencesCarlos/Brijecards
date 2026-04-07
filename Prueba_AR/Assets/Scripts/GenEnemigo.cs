using UnityEngine;

public class GenEnemigo : MonoBehaviour
{
    public GameObject[] enemigos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MostrarEnemigo(int idEnemigo)
    {
        foreach (GameObject gameObj in enemigos)
        {
            gameObj.SetActive(false);
        }

        enemigos[idEnemigo].SetActive(true);
    }
}
