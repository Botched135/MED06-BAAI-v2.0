﻿using UnityEngine;
using System.Collections;

public class EventTriggerZone : MonoBehaviour {
    public GameObject MannequineManager;
    private GameObject PlayerRef;
    public GameObject TPSet;
	// Use this for initialization
	void Start () {
        PlayerRef = GameObject.FindGameObjectWithTag("Player");
        MannequineManager.GetComponent<MannequineScript>().enabled = false;
        TPSet.SetActive(false);
	}
	
	// Update is called once per frame
	public void OnTriggerEnter(Collider col)
    {
            if(col.gameObject == PlayerRef)
        {
            TPSet.SetActive(true);
            MannequineManager.GetComponent<MannequineScript>().enabled = true;
            Destroy(gameObject);
        }
    }
}