using UnityEngine;
using System.Collections;

public class LightTrigger : MonoBehaviour {
    private GameObject Enemy;
    private GameObject Player;

    private bool Activated;
    public GameObject partnerTriggerZone;
    public GameObject[] Lamps = new GameObject[12];

    // Use this for initialization
    void OnEnabled () {
       

    }
    IEnumerator OnTriggerEnter(Collider other)
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (other.gameObject.tag == "Player")
        {
            
            foreach (GameObject lamp in Lamps)
            {
                lamp.SetActive(false);
                
            }
            RenderSettings.ambientIntensity = 0;
            //Play light sound
            
            yield return new WaitForSeconds(3f);
            Enemy.transform.position = new Vector3(Player.transform.position.x-4, 0, Player.transform.position.z);
            Enemy.transform.LookAt(Player.transform);
            yield return new WaitForSeconds(0.75f);
            foreach (GameObject lamp in Lamps)
            {
                lamp.SetActive(true);

            }
            RenderSettings.ambientIntensity = 1;
            //Click for light plus jump scare

            Destroy(partnerTriggerZone);
            Destroy(gameObject);
            
        }
        yield return null;
    }
}
