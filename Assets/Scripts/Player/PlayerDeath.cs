using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour {

    public bool playerDie = false;
    public AudioClip playerDeath;

    public void PlayDied()
    {
        AudioSource.PlayClipAtPoint(playerDeath, transform.position);
    }
}
