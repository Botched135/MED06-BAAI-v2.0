using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {
	public bool open;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.N)){
			open = true;
			
			//transform.position = new Vector3 (transform.position.x, transform.position.y,4f);
		}
		if(Input.GetKey(KeyCode.M)){
			open = false;
			
			//transform.position = new Vector3 (transform.position.x, transform.position.y,0f);
		}
		if(open== true){
			transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, -1f), 1 * Time.deltaTime);
		}
		if(open== false){
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, + 2.8f), 1 * Time.deltaTime);
	}

	}
	
}
