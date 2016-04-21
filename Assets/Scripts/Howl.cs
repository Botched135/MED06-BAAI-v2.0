using UnityEngine;
using System.Collections;

public class Howl : MonoBehaviour {
	private int timer;
	public AudioClip howlSound;

	// Use this for initialization
	void Start () {
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		if(timer>=100){
			AudioSource.PlayClipAtPoint(howlSound, transform.position);
			timer = 0;
		}
	}
}
