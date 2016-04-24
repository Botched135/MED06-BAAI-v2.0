using UnityEngine;
using System.Collections;

public class Ambient : MonoBehaviour {
	//AudioSource FANTASTIC;
	//AudioClip marvelousSound;
	//public AudioClip uncannySound;

	// Use this for initialization
	void Start () {
		//GetComponent<FANTASTIC>();
		 //AudioSource uncannySound = GetComponent<AudioSource>();
		 //uncannySound.Play();
	
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown (KeyCode.B)){
		//AudioSource.PlayClipAtPoint(fantasticSound, transform.position);
		Debug.Log("play1");
		//FANTASTIC.Play();
		//fantasticSound.enabled = true;
		//fantasticSound.loop = true;
		//GetComponent<FANTASTIC>().play();
	}
	if(Input.GetKeyDown (KeyCode.N)){
		//AudioSource.PlayClipAtPoint(marvelousSound, transform.position);
		Debug.Log("play2");
	}
	if(Input.GetKeyDown (KeyCode.M)){
		//AudioSource.PlayClipAtPoint(uncannySound, transform.position);
		Debug.Log("play3");
	}
	
	}
}
