using UnityEngine;
using System.Collections;

public class OpenDoorStright : MonoBehaviour {
	bool opened = false;
    bool soundPlayed = false;
    public AudioClip opening;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(opened == true){
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(transform.localEulerAngles.x, 90, transform.localEulerAngles.z), 1 * Time.deltaTime);
            if (soundPlayed == false)
            {
                AudioSource.PlayClipAtPoint(opening, transform.position, 0.7F);
                soundPlayed = true;
            }
        }
	}
	void OnTriggerStay(Collider other){
		if (other.gameObject.tag == "Player"){
			if(Input.GetButtonDown("Fire1")){
			opened = true;
			}
		}
	}
}