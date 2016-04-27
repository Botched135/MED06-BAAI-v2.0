using UnityEngine;
using System.Collections;

public class MarvelousTrigger : MonoBehaviour {
    private GameObject Player;
    public GameObject otherTrigger;
    public GameObject _object;
    private NavMeshAgent nav;
	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        nav = _object.GetComponent<NavMeshAgent>();
        _object.SetActive(false);
        
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player)
        {
            _object.SetActive(true);
            nav.SetDestination(Player.transform.position);
            nav.speed = 5f;
            Destroy(otherTrigger);
            Destroy(gameObject);
        }
    }
}
