using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {
    private SphereCollider col;
    private float angle;
    private float fieldOfViewAngle = 110f;
    private Vector3 direction;
    private GameObject player;
    private Vector3 playerPosition;
    private EnemyMovement Enemy;
 

	// Use this for initialization
	void Awake () {
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;
        Enemy = GetComponent<EnemyMovement>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            
            direction = other.transform.position - transform.position;
            angle = Vector3.Angle(direction, transform.forward);

           if (angle < fieldOfViewAngle*0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, col.radius))
                {              
                    if (hit.collider.gameObject == player)
                    {
                        Enemy.seenPlayer = true;
                    }
                }
            }
        }
    }
}
