using UnityEngine;
using System.Collections;

public class LightTrigger : MonoBehaviour {
    private GameObject Enemy;
    private GameObject Player;

    private bool Activated;
    public GameObject partnerTriggerZone;
    public GameObject[] Lamps = new GameObject[12];

    public AudioClip jumpScare;
    public AudioClip lightsOut;

    // Use this for initialization
    IEnumerator OnTriggerEnter(Collider other)
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (other.gameObject.tag == "Player")
        {

            GetComponent<BoxCollider>().enabled = false;
            AudioSource.PlayClipAtPoint(lightsOut, new Vector3(Player.transform.position.x, Player.transform.position.y + 2, Player.transform.position.z));
            foreach (GameObject lamp in Lamps)
            {
                lamp.SetActive(false);
                
            }
            RenderSettings.ambientIntensity = 0;
            
            
            yield return new WaitForSeconds(3f);
            AudioSource.PlayClipAtPoint(jumpScare, Enemy.transform.position, 1.0F);
            Enemy.transform.position = new Vector3(Player.transform.position.x-4, Enemy.transform.position.y, Player.transform.position.z);
            Enemy.transform.LookAt(Player.transform);
            yield return new WaitForSeconds(0.75f);
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
