using UnityEngine;
using System.Collections;

public class SpiderMovement : MonoBehaviour {
	float x;
	float y;
	float z;
	private bool MovingUp = false;
	float xRot;
	float yRot;
	float zRot;

	// Use this for initialization
	void Start () {
		//take current pos of spider
		x = transform.position.x;
		y = transform.position.y;
		z = transform.position.z;

		xRot = transform.rotation.x;
		yRot = transform.rotation.y;
		zRot = transform.rotation.z;
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(""+MovingUp);
		if(MovingUp == false){
			if(transform.position.y>=-3f && transform.position.y<=4f){
				transform.position = new Vector3 (x, y,z);
				y += 0.008f;
				transform.eulerAngles = new Vector3(xRot, yRot+280, zRot+450);
				xRot = 270f;
				//transform.rotation = new Vector3 (xRot,yRot,zRot);
			//xRot	 += 160f;

		} else if (transform.position.y>=4f)
		MovingUp = true;

		}
		if(MovingUp == true){
				transform.position = new Vector3 (x, y,z);
				y -= 0.008f;

				transform.eulerAngles = new Vector3(xRot, yRot+280, zRot+450);
				xRot = 450f;
				//transform.rotation = new Vector3 (xRot,yRot, zRot);
				//xRot += 360f;

		}if (transform.position.y<=0f)
		MovingUp = false;
	}

}
