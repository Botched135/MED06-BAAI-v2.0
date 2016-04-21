using UnityEngine;
using System.Collections;

public class MonsterMSound : MonoBehaviour {

	public int timer;
	public int number = 0;
	public AudioClip laughSound1;
	public AudioClip laughSound2;
	public AudioClip laughSound3;
	public AudioClip laughSound4;
	public AudioClip playLaughSound;

	// Use this for initialization
	void Start () {
		timer = 0;
		AudioSource.PlayClipAtPoint(laughSound1, transform.position);

	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		switch (number)
		{
	    	case 1:
	        playLaughSound = laughSound2;
	        break;
	    	case 2:
	        playLaughSound = laughSound3;
	        break;
	        case 3:
	        playLaughSound = laughSound4;
	        break;
		}
		
		if(timer>=100){
			AudioSource.PlayClipAtPoint(playLaughSound, transform.position);
			timer = 0;
			number++;
			if(number>=4){
				number = 0;
			}
		}
		
	}
}
