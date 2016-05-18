using UnityEngine;
using System.Collections;

public class EventTriggerZone : MonoBehaviour {
    public GameObject MannequineManager;
    private GameObject PlayerRef;
    public GameObject TPSet;
    private GameAI GameAI;
	// Use this for initialization
	void Start () {
        GameObject _temp = GameObject.FindGameObjectWithTag("EditorOnly");
        GameAI = _temp.GetComponent<GameAI>();
        PlayerRef = GameObject.FindGameObjectWithTag("Player");
        MannequineManager.GetComponent<MannequineScript>().enabled = false;
        TPSet.SetActive(false);
	}
	
	// Update is called once per frame
	public void OnTriggerEnter(Collider col)
    {
            if(col.gameObject == PlayerRef)
        {
            GameAI.SaveToFile(GameAI.time,1);
            TPSet.SetActive(true);
            MannequineManager.GetComponent<MannequineScript>().enabled = true;
            Destroy(gameObject);
        }
    }
}
