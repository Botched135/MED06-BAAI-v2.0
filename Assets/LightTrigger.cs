using UnityEngine;
using System.Collections;

public class LightTrigger : MonoBehaviour {
    public GameObject Enemy;
    public int index;
    private GameObject Player;

    private bool Activated;
    public GameObject partnerTriggerZone;
    public GameObject[] Lamps = new GameObject[12];

    // Use this for initialization
    void OnEnabled () {
        Player = GameObject.FindGameObjectWithTag("Player");
        Enemy = GameObject.FindGameObjectWithTag("Enemy");	
	}
    IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("TRIGGERED");
            foreach (GameObject lamp in Lamps)
            {
                lamp.SetActive(false);
                
            }
            RenderSettings.ambientIntensity = 0;
            //Teleport the girl
            yield return new WaitForSeconds(4f);
            foreach (GameObject lamp in Lamps)
            {
                lamp.SetActive(true);

            }
            RenderSettings.ambientIntensity = 1;

            Destroy(partnerTriggerZone);
            Destroy(gameObject);
            
        }
        yield return null;
    }
}
