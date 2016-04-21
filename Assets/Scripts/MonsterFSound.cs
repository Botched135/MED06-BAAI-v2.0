using UnityEngine;
using System.Collections;

public class MonsterFSound : MonoBehaviour {
	public int timer;
	public int number = 0;
	public AudioClip fanSound1;
	public AudioClip fanSound2;
	public AudioClip fanSound3;
	public AudioClip fanSound4;
	public AudioClip fanSound5;
	public AudioClip fanSound6;
	public AudioClip playFanSound;

	// Use this for initialization
	void Start () {
		timer = 0;
		//AudioSource.PlayClipAtPoint(playFanSound, transform.position);

	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		switch (number)
		{
	    	case 1:
	        playFanSound = fanSound1;
	        break;
	    	case 2:
	        playFanSound = fanSound2;
	        break;
	        case 3:
	        playFanSound = fanSound3;
	        break;
	        case 4:
	        playFanSound = fanSound4;
	        break;
	        case 5:
	        playFanSound = fanSound5;
	        break;
	        case 6:
	        playFanSound = fanSound6;
	        break;
		}
		
		if(timer>=100){
			timer = 0;
			number++;
			AudioSource.PlayClipAtPoint(playFanSound, transform.position);
			
			if(number>=6){
				number = 0;
			}
		}
		
	}
}