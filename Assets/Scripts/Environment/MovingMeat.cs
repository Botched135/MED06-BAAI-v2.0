using UnityEngine;
using System.Collections;

public class MovingMeat : MonoBehaviour {
	//bool movingUp = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*if(transform.localScale.y>=60f){
			movingUp = false;
		}
		if(transform.localScale.y<=50f){
			movingUp = true;
		}
		if(movingUp == true){
			//transform.localScale += new Vector3(0.2F,Time.deltaTime*2F,0);
			transform.localScale += new Vector3(0,Time.deltaTime*4F,0);
		}
		if(movingUp == false){
			//transform.localScale -= new Vector3(0.2F,Time.deltaTime/2F,0);
			transform.localScale -= new Vector3(0,Time.deltaTime/0.12F,0);
			transform.position = new Vector3(Mathf.Sin(Time.time), 0.0f, 0.0f);
		}*/


		transform.localScale = new Vector3((transform.localScale.x + Mathf.Sin(Time.time)/4),(transform.localScale.y + Mathf.Sin(Time.time)/4), (transform.localScale.z + Mathf.Sin(Time.time)/4));
		

	
	}
}
