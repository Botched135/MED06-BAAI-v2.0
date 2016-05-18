using UnityEngine;
using System.Collections;

public class SlamDoor : MonoBehaviour {

    public GameObject Door;
    public AudioClip SlamSound;
    public GameAI AI;
    private GameObject Player;
    private BoxCollider Trigger;
	// Use this for initialization
	void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("EditorOnly");
        AI = temp.GetComponent<GameAI>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Trigger = Door.GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player)
        {
            Door.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.x);
            AudioSource.PlayClipAtPoint(SlamSound, Door.transform.position, 1.0F);
            AI.SaveToFile(AI.time, 2);
            Destroy(Trigger);
            Destroy(gameObject);
        }
    }
}
