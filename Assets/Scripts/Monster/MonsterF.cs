using UnityEngine;
using System.Collections;

public class MonsterF : Monster {
    private bool Roar;
    private bool trigger = false, trigger2 =false;

    //Soundclips etc
    public AudioClip attack01;
    public AudioClip attack02;
    public AudioClip attack03;
    public AudioClip neutral01;
    public AudioClip neutral02;
    public AudioClip neutral03;
    int select = 1;

    // Use this for initialization
    void Awake () {
        //GUI
        GameObject _temp;
        _temp = GameObject.FindGameObjectWithTag("EditorOnly");
        fade = _temp.GetComponent<Fading>();
        //
        anim = GetComponent<Animator>();
        Directions = GameObject.FindGameObjectsWithTag("Front");
        wayPointIndex = 0;
        wayPoints = new GameObject[2];
        nav = GetComponent<NavMeshAgent>();
        

        rotationTime = 3f;

        wayPoints = GameObject.FindGameObjectsWithTag("Point1");
        player = GameObject.FindGameObjectWithTag("Player");
        playerDeath = player.GetComponent<PlayerDeath>();
        time = 0;
        nav.destination = wayPoints[0].transform.position;

        pivotPoint = GameObject.FindGameObjectWithTag("TransformPos");

        //EnemySight
        seenPlayer = false;
        col = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {

        moveSpeed = nav.velocity.sqrMagnitude;
        anim.SetFloat("Speed", moveSpeed * moveSpeed);
        if (seenPlayer && !Roar)
        {
            NoticePlayer();
        }
        if (Roar) {
            if (!trigger2)
            {
                nav.Resume();
                trigger2 = true;
            }
            anim.SetBool("SeenPlayer", true);
            nav.speed = 2f;
            playerPosition = player.transform.position;
            nav.SetDestination(playerPosition);
            
            Attack();
        }
        else if(!seenPlayer || !Roar)
            Move();
    }

    public IEnumerator roars()
    {
        switch (select)
        {
            case 1:
                AudioSource.PlayClipAtPoint(attack01, transform.position, 1.0f);
                break;
            case 2:
                AudioSource.PlayClipAtPoint(neutral01, transform.position, 1.0f);
                
                break;
            case 3:
                AudioSource.PlayClipAtPoint(attack02, transform.position, 1.0f);
                break;
            case 4:
                AudioSource.PlayClipAtPoint(neutral02, transform.position, 1.0f);
                
                break;
            case 5:
                AudioSource.PlayClipAtPoint(attack03, transform.position, 1.0f);
                break;
            case 6:
                AudioSource.PlayClipAtPoint(neutral03, transform.position, 1.0f);
                
                break;
        }
        yield return new WaitForSeconds(4f);
        select++;
        if (select == 7) { select = 1; }
        yield return null;
        StartCoroutine(roars());


    }

    public void OnTriggerStay(Collider other) //EnemySight - should use base keyword in children classes
    {
        if (other.gameObject == player)
        {
            direction = other.transform.position - transform.position;
            angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                
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
        if (moveSpeed * moveSpeed == 0)
        {
            anim.SetBool("RoarFromStance", true);
        }
        else {
            anim.SetBool("roar", true);
        }

        nav.Stop();
        
        
        i += Time.deltaTime;
        if (!trigger && i > 1)
        {
            StartCoroutine(roars());
            trigger = true;
        }
        if (i > 2.5)
        {
            Roar = true;
            anim.SetBool("roar", false);
            anim.SetBool("RoarFromStance", false);
            
        }
    }
    public override IEnumerator KnockBack()
    {
        nav.Stop();
        anim.SetBool("Attack", true);
        //GUI
        fade.Die();
        //
        //GUI
        if(fade.alpha >= 1){
            fade.BeginFade(-1);
        }
        //
        yield return null;
        anim.SetBool("Attack", false);

        yield return new WaitForSeconds(3);
        nav.Resume();
        PlayerKnockedDown = true;
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
            nav.Stop();
            anim.SetBool("Attack", true);
            //GUI
            fade.OnLevelWasLoaded();
            //
            yield return null;
            anim.SetBool("Attack", false);
            yield return new WaitForSeconds(0.5f);
            playerDeath.playerDie = true;
        }
    }
}
