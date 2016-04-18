using UnityEngine;
using System.Collections;

public class MannequineScript : MonoBehaviour {
    private PlayerSight PlayerSight;
    private GameObject Player;
    private Vector3 direction;
    private Vector3[] teleportPoints;
    public float maxDistance;
	
	void Awake () {
        Player = GameObject.FindGameObjectWithTag("MainCamera");
        PlayerSight = Player.GetComponent<PlayerSight>();
        maxDistance = 10f;

	
	}
	
	void Update () {
        direction = Player.transform.position - transform.position;
        if(!PlayerSight.mannequineSeen && direction.sqrMagnitude > maxDistance * maxDistance)
        {
            Teleport();
        }
	}

    void Teleport()
    {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z-2);
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);


    }
}
