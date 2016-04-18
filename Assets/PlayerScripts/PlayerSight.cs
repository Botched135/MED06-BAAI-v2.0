using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSight : MonoBehaviour {
    private float fieldOfViewAngle = 120f;
    private float angle;
    private Vector3 direction;
    private GameObject[] PreMannequines;
    private List<GameObject> Mannequines;
    private SphereCollider col;

    public bool mannequineSeen;
	// Use this for initialization
	void Awake () {
        Mannequines = new List<GameObject>();
        PreMannequines = GameObject.FindGameObjectsWithTag("Mannequine");
        Mannequines.Add(PreMannequines[0]);
        

	}
	

    public void OnTriggerStay(Collider other)
    {
        if (Mannequines.Contains(other.gameObject))
        {
            direction = other.transform.position - transform.position;
            angle = Vector3.Angle(direction, transform.forward);
            
            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit))
                {
                    Debug.DrawRay(transform.position, direction, Color.red);
                    if (Mannequines.Contains(hit.collider.gameObject))
                    {
                        mannequineSeen = true;
                        return;
                    }
                }
            }
            else mannequineSeen = false;
        }
    }
}
