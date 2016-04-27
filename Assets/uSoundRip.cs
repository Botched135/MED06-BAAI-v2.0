using UnityEngine;
using System.Collections;

public class uSoundRip : MonoBehaviour {
	public int timer;
	public AudioClip ripSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		if(timer==300){
		AudioSource.PlayClipAtPoint(ripSound, transform.position,1.0F);
	}
	
	}
}
