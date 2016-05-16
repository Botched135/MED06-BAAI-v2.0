﻿using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

    public GameObject Enemy; 
    private GameObject Player;
    [SerializeField]
    private GameObject SpawnPoint;
    private bool Activated;
    public GameObject lightTrigger;
    public GameObject partnerTriggerZone;

    void Awake()
    {
        
        Player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player && !Activated)
        {
            if(lightTrigger != null)
                lightTrigger.SetActive(true);

            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            Instantiate(Enemy, SpawnPoint.transform.position, Quaternion.Euler(new Vector3(0,-90,0)));
            Destroy(partnerTriggerZone);
            Destroy(gameObject);
            
        }
    }
}
