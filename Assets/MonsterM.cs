using UnityEngine;
using System.Collections;

public class MonsterM : Monster {

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        Directions = GameObject.FindGameObjectsWithTag("Front");
        wayPointIndex = 0;
        wayPoints = new GameObject[2];
        nav = GetComponent<NavMeshAgent>();
        nav.speed = 1f;


        rotationTime = 3f;

        wayPoints = GameObject.FindGameObjectsWithTag("Point1");
        player = GameObject.FindGameObjectWithTag("Player");
        time = 0;
        nav.destination = wayPoints[0].transform.position;

        pivotPoint = GameObject.FindGameObjectWithTag("TransformPos");

        //EnemySight
        seenPlayer = false;
        anim.SetBool("SeenPlayer", false);
        col = GetComponent<SphereCollider>();
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
                if (Physics.Raycast(transform.position + direction.normalized, direction, out hit, col.radius))
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
    // Update is called once per frame
    void Update () {
        moveSpeed = nav.velocity.sqrMagnitude;
        anim.SetFloat("Speed", moveSpeed * moveSpeed);
        if (seenPlayer)
        {
            playerPosition = player.transform.position;
            anim.SetBool("SeenPlayer", true);
            nav.speed = 2;
            NoticePlayer();


            nav.SetDestination(playerPosition);
            nav.Resume();
            Attack();
        }
        else if (!seenPlayer)
            Move();
    }
    public override void NoticePlayer()
    {
       //play sound
    }
}
