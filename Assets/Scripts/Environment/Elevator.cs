using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {
	public bool open = false;

    //Sound
    public AudioClip ding;
    bool soundPlayed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(open== true){
            if (soundPlayed == false)
            {
                soundPlayed = true;
                AudioSource.PlayClipAtPoint(ding, transform.position, 0.8F);
                
            }
			transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, -1f), 1 * Time.deltaTime);
		}
		if(open== false){
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, + 2.8f), 1 * Time.deltaTime);
		}
	}
	void OnTriggerEnter(Collider other){
		Debug.Log("col1");
		if (other.gameObject.tag == "Player"){
			Debug.Log("col2");
			open = true;
		}
	}	
}

	

