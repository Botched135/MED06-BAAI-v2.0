﻿using UnityEngine;
using System.Collections;

public class MonsterU : Monster {
    private Vector3 TeleportVector;
    private bool trigger;

    //Soundclips
    public AudioClip teleportSound;
    public AudioClip giggle01;
    public AudioClip giggle02;
    public AudioClip giggle03;
    public AudioClip giggle04;
    public AudioClip attackSound;
    bool attacked = true;
    public bool firstTimeSeenPlayer = true;
    int select = 1;

    // Use this for initialization
    void Start () {
        //GUI
        GameObject _temp;
        _temp = GameObject.FindGameObjectWithTag("EditorOnly");
        fade = _temp.GetComponent<Fading>();
        //
        trigger = false;
        anim = GetComponent<Animator>();
        Directions = GameObject.FindGameObjectsWithTag("Front");
        wayPointIndex = 0;
        wayPoints = new GameObject[2];
       

        rotationTime = 3f;

        wayPoints = GameObject.FindGameObjectsWithTag("Point1");
        player = GameObject.FindGameObjectWithTag("Player");

        time = 0;
        //nav.destination = wayPoints[0].transform.position;

        pivotPoint = GameObject.FindGameObjectWithTag("TransformPos");

        //EnemySight
        seenPlayer = false;
        anim.SetBool("SeenPlayer", false);
        col = GetComponent<SphereCollider>();
    }
	
	// Update is called once per frame
	void Update () {
    

        if (seenPlayer)
        {
            
            playerPosition = player.transform.position;
            TeleportVector = playerPosition - transform.position;
            transform.rotation = Quaternion.LookRotation(new Vector3(playerPosition.x - transform.position.x, transform.position.y, playerPosition.z - transform.position.z));
            if (!trigger)
            {
                StartCoroutine(Teleport());
                StartCoroutine(giggle());
                trigger = true;
                
            }
            NoticePlayer();
            Attack();
        
        }
        //else if (!seenPlayer)
            //Move();

    
    }

    public IEnumerator giggle()
    {
        yield return new WaitForSeconds(4.0f);
            switch (select)
            {
                case 1:
                    AudioSource.PlayClipAtPoint(giggle01, transform.position);
                    break;
                case 2:
                    AudioSource.PlayClipAtPoint(giggle02, transform.position);
                    break;
                case 3:
                    AudioSource.PlayClipAtPoint(giggle03, transform.position);
                    break;
                case 4:
                    AudioSource.PlayClipAtPoint(giggle04, transform.position);
                    break;
            }
            select++;
            if (select == 5) { select = 1; }
            
        
    }

    public void OnTriggerStay(Collider other) //EnemySight - should use base keyword in children classes
    {
        if (other.gameObject == player)
        {
            direction = other.transform.position - transform.position;
            angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                Debug.DrawRay(transform.position + direction.normalized, direction, Color.red);
                RaycastHit hit;
                if (Physics.Raycast(transform.position+direction.normalized, direction, out hit, col.radius))
                {
                    if (hit.collider.gameObject == player)
                    {
                        
                        seenPlayer = true;
                        StopCoroutine(LookingSequence());

                    }
                }
            }
        }
    }

    public override void NoticePlayer()
    {
        //play soundtrack of noticing the player
        // Jacob - Måske skal vi ændre ambience om til musik når man bliver set af et monster
        if (firstTimeSeenPlayer)
        {
           // AudioSource.PlayClipAtPoint(giggle03, transform.position);
            firstTimeSeenPlayer = false;
        }


}
    private IEnumerator Teleport()
    {
        
        yield return new WaitForSeconds(5f);

        //soundfile play for teleport
        AudioSource.PlayClipAtPoint(teleportSound, transform.position);
        if (Mathf.Abs(playerPosition.x - TeleportVector.x) < 25)
        {
            Debug.Log("Surprise Motherfucker");
            transform.position = new Vector3(playerPosition.x - AttackRange/2, 0, playerPosition.z - AttackRange/2);
        }
        else
            transform.position = new Vector3(playerPosition.x - (TeleportVector.x / 2.5f), 0, playerPosition.z - (TeleportVector.z / 2.5f));
        transform.rotation = Quaternion.LookRotation(new Vector3(playerPosition.x-transform.position.x, 0, playerPosition.z-transform.position.z));

        StartCoroutine(Teleport());
    }
    public override IEnumerator KnockBack() // do some sound of attack
    {
        
        StopCoroutine(Teleport());
        anim.SetBool("Attack", true);

        //Sound play
        if (attacked == true)
        {
            AudioSource.PlayClipAtPoint(attackSound, player.transform.position, 1.0F);
            attacked = false;
        }
       //GUI
        fade.Die();
        //
         //GUI
        if(fade.alpha >= 1){
            fade.BeginFade(-1);
        }
        
        yield return null;
        anim.SetBool("Attack", false);
        yield return new WaitForSeconds(3);
        PlayerKnockedDown = true;
        StartCoroutine(Teleport());
    }
    public override IEnumerator KnockBack(bool killer)
    {
        if (!killer)
        {
            StartCoroutine(KnockBack());
            yield return null;
        }
        else
        {
            anim.SetBool("Attack", true);
            //GUI
            fade.OnLevelWasLoaded();
            //
            yield return null;
            anim.SetBool("Attack", false);
            attacked = true;
            yield return new WaitForSeconds(0.5f);
            StopCoroutine(Teleport());
        }
    }
}
