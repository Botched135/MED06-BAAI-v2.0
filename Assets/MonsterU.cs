using UnityEngine;
using System.Collections;

public class MonsterU : Monster {
    private Vector3 TeleportVector;
    private bool trigger;
	// Use this for initialization
	void Start () {
        trigger = false;
        anim = GetComponent<Animator>();
        Directions = GameObject.FindGameObjectsWithTag("Front");
        wayPointIndex = 0;
        wayPoints = new GameObject[2];
        nav = GetComponent<NavMeshAgent>();
        nav.speed = 0.75f;


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
	
	// Update is called once per frame
	void Update () {

        moveSpeed = nav.velocity.sqrMagnitude;
        anim.SetFloat("Speed", moveSpeed * moveSpeed);
        if (seenPlayer)
        {
            playerPosition = player.transform.position;
            TeleportVector = playerPosition - transform.position;
            if (!trigger)
            {
                StartCoroutine(Teleport());
                trigger = true;
            }
            anim.SetBool("SeenPlayer", true);
            nav.speed = 1.25f;
            NoticePlayer();
            
            
            nav.SetDestination(playerPosition);
            nav.Resume();
            Attack();
        }
        else if (!seenPlayer)
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
                Debug.DrawRay(transform.position + direction.normalized, direction, Color.red);
                RaycastHit hit;
                if (Physics.Raycast(transform.position+direction.normalized, direction, out hit, col.radius))
                {
                    if (hit.collider.gameObject == player)
                    {
                        Debug.Log("NOTICE ME SENPAI!");
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
    }
    private IEnumerator Teleport()
    {
        
        yield return new WaitForSeconds(5f);
        //soundfile play for teleport
        transform.position = new Vector3(playerPosition.x-(TeleportVector.x/2), transform.position.y, playerPosition.z-(TeleportVector.z / 2));
        StartCoroutine(Teleport());
    }
}
