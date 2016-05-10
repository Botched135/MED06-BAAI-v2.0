using UnityEngine;
using System.Collections;

public class Chains : MonoBehaviour {
	public AudioClip chainSound;
	 Animator anim;
	 
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
	}

void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player"){
		anim.Play("Move", 0);
	}
}
}

