using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shurikenPrefab;
    [SerializeField] private float spawnDelay = 1f;
    private int rngX;
    public static ObstacleSpawner instance = null;


    // Awake is called before Start
    private void Awake()
    {
        //Singleton Pattern
        //if there isn't another ObstacleSpawner script, run normally
        if(instance == null)
        {
            instance = this;
        //if there's already another game object with ObstacleSpawner script, destroy this game object
        } else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //spawn enemies every _ seconds
        InvokeRepeating("SpawnObject", 5, spawnDelay);
    }

    public void SpawnObject()
    {
        rngX = Random.Range(-5, 5);

        //Vector3 pixelPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Instantiate(shurikenPrefab, new Vector3(rngX, this.transform.position.y, 0), transform.rotation);
    }
}
