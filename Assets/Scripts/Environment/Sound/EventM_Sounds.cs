using UnityEngine;
using System.Collections;

public class EventM_Sounds : MonoBehaviour {

    public bool hasCollided = false;
    private GameObject player;
    public AudioClip rolling;
    public AudioClip crash;
    AudioSource rollingSource;

    

	// Use this for initialization
	void OnEnable () {

        player = GameObject.FindGameObjectWithTag("Player");
        rollingSource = GetComponent<AudioSource>();

        // AudioSource.PlayClipAtPoint(rolling , transform.position , 1.0F);

        rollingSource.clip = rolling;
        rollingSource.Play();

    }

    void OnCollisionEnter(Collision other){

        if (other.gameObject == player)
        {
            rollingSource.Stop();

            if (hasCollided == false)
            {
                AudioSource.PlayClipAtPoint(crash, transform.position, 1.0F);
                hasCollided = true;
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
	
  


	}

}
