using UnityEngine;
using System.Collections;

public class RedLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Light>().color = new Color (1F, 0.2F, 0F);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
