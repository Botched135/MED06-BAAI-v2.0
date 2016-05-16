using UnityEngine;
using System.Collections;

public class IronMaidenSound : MonoBehaviour {
    public AudioClip ironMaidenScare;
    AudioSource IronMaiden;


	// Use this for initialization
	void Start () {
        IronMaiden = GetComponent<AudioSource>();
        IronMaiden.clip = ironMaidenScare;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player"){
            IronMaiden.Play();
		}
	}
}
