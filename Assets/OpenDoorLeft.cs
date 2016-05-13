using UnityEngine;
using System.Collections;

public class OpenDoorLeft : MonoBehaviour {
	bool opened = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(opened == true){
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(transform.localEulerAngles.x, 90, transform.localEulerAngles.z), 1 * Time.deltaTime);

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
