using UnityEngine;
using System.Collections;

public class SlamDoor : MonoBehaviour {

    public GameObject Door;
    public AudioClip SlamSound;
    public AudioClip Laugh;
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
            Door.GetComponent<OpenDoorStright>().enabled = false;
            AudioSource.PlayClipAtPoint(SlamSound, new Vector3(Door.transform.position.x+1, Door.transform.position.y+1, Door.transform.position.z),110f);
            StartCoroutine(Laughter());
            AI.SaveToFile(AI.time, 2);
            Destroy(Trigger);
            Destroy(gameObject);
        }
    }
    IEnumerator Laughter()
    {
        yield return new WaitForSeconds(2f);
        AudioSource.PlayClipAtPoint(Laugh, new Vector3(Door.transform.position.x + 1, Door.transform.position.y + 1, Door.transform.position.z), 110f);
    }
}
