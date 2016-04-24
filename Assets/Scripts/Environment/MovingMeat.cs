using UnityEngine;
using System.Collections;

public class MovingMeat : MonoBehaviour {
	bool movingUp = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.localScale.y>=60f){
			movingUp = false;
		}
		if(transform.localScale.y<=50f){
			movingUp = true;
		}
		if(movingUp == true){
			//transform.localScale += new Vector3(0.2F,Time.deltaTime*2F,0);
			transform.localScale += new Vector3(0,Time.deltaTime*2F,0);
		}
		if(movingUp == false){
			//transform.localScale -= new Vector3(0.2F,Time.deltaTime/2F,0);
			transform.localScale -= new Vector3(0,Time.deltaTime/2F,0);
		}

		

	
	}
}
