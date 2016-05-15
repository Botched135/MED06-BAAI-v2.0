using UnityEngine;
using System.Collections;

public class ClosedDoor : MonoBehaviour
{
    bool tryingToOpen = false;
    bool soundPlayed = false;
    public AudioClip closed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tryingToOpen == true)
        {
            if (soundPlayed == false)
            {
                AudioSource.PlayClipAtPoint(closed, transform.position, 0.7F);
                soundPlayed = true;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetButtonDown("Fire1"))
            {
               tryingToOpen = true;
            }
        }
    }
}
