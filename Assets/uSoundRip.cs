using UnityEngine;
using System.Collections;

public class uSoundRip : MonoBehaviour {
	public int timer;
	public AudioClip ripSound;
    AudioSource eventSound;
    GameAI GameAI;
	// Use this for initialization
	void Start () {
        GameObject _temp = GameObject.FindGameObjectWithTag("EditorOnly");
        GameAI = _temp.GetComponent<GameAI>();
        eventSound = GetComponent<AudioSource>();
        eventSound.clip = ripSound;
	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		if(timer==300){
            GameAI.SaveToFile(GameAI.time);
            eventSound.Play();
		    //AudioSource.PlayClipAtPoint(ripSound, transform.position,1.0F);
	}
	
	}
}
