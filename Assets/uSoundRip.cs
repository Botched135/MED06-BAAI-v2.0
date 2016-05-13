using UnityEngine;
using System.Collections;

public class uSoundRip : MonoBehaviour {
	public int timer;
	public AudioClip ripSound;
    AudioSource eventSound;

	// Use this for initialization
	void Start () {

        eventSound = GetComponent<AudioSource>();
        eventSound.clip = ripSound;
	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		if(timer==300){
            eventSound.Play();
		    //AudioSource.PlayClipAtPoint(ripSound, transform.position,1.0F);
	}
	
	}
}
