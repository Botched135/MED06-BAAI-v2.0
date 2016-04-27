using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSight : MonoBehaviour {
    public float fieldOfViewAngle = 120f;
    private float angle;
    private Vector3 direction;
    private GameObject[] PreMannequines;
    private List<GameObject> Mannequines;
    public SphereCollider col;
    private MannequineScript MannequineScript;

    public bool mannequineSeen;
	// Use this for initialization
	void Awake () {
        col = gameObject.GetComponent<SphereCollider>();
        Mannequines = new List<GameObject>();
        PreMannequines = GameObject.FindGameObjectsWithTag("Mannequine");

        for (int i = 0; i < PreMannequines.Length; i++)
        {
            Mannequines.Add(PreMannequines[i]);
        }

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
