using UnityEngine;
using System.Collections;

public class Howl : MonoBehaviour {
	private int timer;
	public int number = 0;
	public AudioClip howlSound1;
	public AudioClip howlSound2;
	public AudioClip howlSound3;
	public AudioClip playHowlSound;

	// Use this for initialization
	void Start () {
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		switch (number)
		{
	    	case 1:
	        playHowlSound = howlSound1;
	        break;
	    	case 2:
	        playHowlSound = howlSound2;
	        break;
	        case 3:
	        playHowlSound = howlSound3;
	        break;
	       
		}
		
		if(timer>=250){
			timer = 0;
			number++;
			AudioSource.PlayClipAtPoint(playHowlSound, transform.position);
			
			if(number>=6){
				number = 0;
			}
		}
	}
}
