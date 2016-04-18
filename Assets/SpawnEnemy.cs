using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

    public GameObject Enemy; 
    private GameObject Player;
    private Transform SpawnPoint;
    private bool Activated;

    void Awake()
    {
        SpawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        Player = GameObject.FindGameObjectWithTag("Player");
        Activated = false;
    }
	
	// Update is called once per frame
	
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player && !Activated)
        {

            Instantiate(Enemy, SpawnPoint.position, Quaternion.identity);
            Destroy(gameObject);
            
        }
    }
}
