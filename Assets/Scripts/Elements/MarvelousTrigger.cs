using UnityEngine;
using System.Collections;

public class MarvelousTrigger : MonoBehaviour {
    private GameObject Player;
    public GameObject otherTrigger;
    public GameObject _object;
    [Range(0, 1000)]
    public float _force;
    private Rigidbody _body;
    private GameAI GameAI;

	// Use this for initialization
	void Start () {
        GameObject _temp = GameObject.FindGameObjectWithTag("EditorOnly");
        GameAI = _temp.GetComponent<GameAI>();
        Player = GameObject.FindGameObjectWithTag("Player");
        _object.SetActive(false);
        _body = _object.GetComponent<Rigidbody>();
        
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player)
        {
            GameAI.SaveToFile(GameAI.time,1);
            _object.SetActive(true);
            _body.AddForce((transform.position-_object.transform.position) * _force);
            Destroy(otherTrigger);
            Destroy(gameObject);
        }
    }
}
