﻿using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

    public GameObject Enemy; 
    private GameObject Player;
    [SerializeField]
    private GameObject SpawnPoint;
    private bool Activated;
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
            //Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            Instantiate(Enemy, SpawnPoint.transform.position, Quaternion.identity);
            Destroy(partnerTriggerZone);
            Destroy(gameObject);
            
        }
    }
}
