using UnityEngine;
using System.Collections;

public class MonsterF : Monster {
    private bool Roar;
	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animator>();
        Directions = GameObject.FindGameObjectsWithTag("Front");
        wayPointIndex = 0;
        wayPoints = new GameObject[2];
        nav = GetComponent<NavMeshAgent>();
        

        rotationTime = 3f;

        wayPoints = GameObject.FindGameObjectsWithTag("Point1");
        player = GameObject.FindGameObjectWithTag("Player");
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
            anim.SetBool("SeenPlayer", true);
            nav.speed = 2f;
            playerPosition = player.transform.position;
            nav.SetDestination(playerPosition);
            nav.Resume();
            Attack();
        }
        else if(!seenPlayer || !Roar)
            Move();
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
                if (Physics.Raycast(transform.position, direction, out hit, col.radius))
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
            anim.SetBool("RoarFromStance", true);
        //play soundfile for beast roar
        else
            anim.SetBool("roar", true);
        
        nav.Stop();
        i += Time.deltaTime;
        if (i > 2.5)
        {
            Roar = true;
            anim.SetBool("roar", false);
            anim.SetBool("RoarFromStance", false);
            
        }
    }
}
