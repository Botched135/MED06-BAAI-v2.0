using UnityEngine;
using System.Collections;

public class MonsterUSound : MonoBehaviour {

	public int timer;
	public int number = 0;
	public AudioClip girlSound1;
	public AudioClip girlSound2;
	public AudioClip girlSound3;
	public AudioClip girlSound4;
	public AudioClip girlSound5;
	public AudioClip playGirlSound;

	// Use this for initialization
	void Start () {
		timer = 0;
		//AudioSource.PlayClipAtPoint(playGirlSound, transform.position);

	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		switch (number)
		{
	    	case 1:
	        playGirlSound= girlSound1;
	        break;
	    	case 2:
	        playGirlSound = girlSound2;
	        break;
	        case 3:
	        playGirlSound = girlSound3;
	        break;
	        case 4:
	        playGirlSound = girlSound4;
	        break;
	        case 5:
	        playGirlSound = girlSound5;
	        break;
		}
		
		if(timer>=100){
			timer = 0;
			number++;
			AudioSource.PlayClipAtPoint(playGirlSound, transform.position);
			
			if(number>=5){
				number = 0;
			}
		}
		
	}
}
