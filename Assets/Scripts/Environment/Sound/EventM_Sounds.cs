using UnityEngine;
using System.Collections;

public class EventM_Sounds : MonoBehaviour {

    public bool hasCollided = false;
    private bool trigger;
    private Rigidbody _body;
    private Rigidbody thisBody;
    private GameObject player;
    private GameObject laughingMan;
    public AudioClip rolling;
    public AudioClip crash;
    public AudioClip laugh;
    AudioSource rollingSource;
  

    

	// Use this for initialization
	void OnEnable () {
        player = GameObject.FindGameObjectWithTag("Player");
        laughingMan = GameObject.FindGameObjectWithTag("Laugh01");
        _body = player.GetComponent<Rigidbody>();
        thisBody = GetComponent<Rigidbody>();
        rollingSource = GetComponent<AudioSource>();
       

        // AudioSource.PlayClipAtPoint(rolling , transform.position , 1.0F);

        rollingSource.clip = rolling;
        rollingSource.Play();
        

    }

    IEnumerator OnCollisionEnter(Collision other){

        if (other.collider.gameObject.tag == "Player" || other.collider.gameObject.tag == "PossibleHits")
        {
            rollingSource.Stop();

            if (hasCollided == false)
            {
                AudioSource.PlayClipAtPoint(crash, transform.position, 1.0F);
                hasCollided = true;
                yield return new WaitForSeconds(1.5f);
                //play laugh here
                AudioSource.PlayClipAtPoint(laugh, laughingMan.transform.position , 1.0F);
            }

        }


    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log(GetComponent<Rigidbody>().velocity.sqrMagnitude);
        if (GetComponent<Rigidbody>().velocity.sqrMagnitude < 10)
        {
            rollingSource.volume -= Time.deltaTime/1.5f;
            if(rollingSource.volume == 0 && hasCollided == false)
            {
                //play laugh here
                AudioSource.PlayClipAtPoint(laugh, laughingMan.transform.position, 1.0F);
                hasCollided = true;
               
            }
          
        }


    }

}
