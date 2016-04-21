using UnityEngine;
using System.Collections;

public class Chains : MonoBehaviour {
	private int timer;
	public AudioClip chainSound;

	// Use this for initialization
	void Start () {
		//AudioSource.PlayClipAtPoint(chainSound, transform.position); // play once at start
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		if(timer>=60){
			AudioSource.PlayClipAtPoint(chainSound, transform.position);
			timer = 0;
		}
	}
}
