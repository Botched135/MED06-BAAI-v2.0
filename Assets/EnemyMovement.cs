using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
    private NavMeshAgent nav;
    private Vector3 playerPosition, leftTarget, rightTarget, resetTarget;

    private Transform player;
    private float angle;
    private SphereCollider col;
    private float time, patrolTime;

    private GameObject[] Directions = new GameObject[3];
    private Quaternion newRotation;
    private bool left, right, turned, ended;

    private GameObject[] wayPoints;
    private int wayPointIndex, i;

    [SerializeField]
    private float rotationTime;

    public float fieldOfViewAngle = 110f;
    public bool seenPlayer;
    
    void Awake()
    {
        Directions = GameObject.FindGameObjectsWithTag("Front");
        wayPointIndex = 0;
        wayPoints = new GameObject[2];   
        nav = GetComponent<NavMeshAgent>();
        seenPlayer = false;
        rotationTime = 3f;
        wayPoints = GameObject.FindGameObjectsWithTag("Point1");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        time = 0;
        turned = false;
        left = false;
        i = 0;
        ended = false;
        nav.destination = wayPoints[0].transform.position;
        

    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log(resetTarget);
        if (seenPlayer)
        {
            playerPosition = player.position;
            nav.SetDestination(playerPosition);
        }
        else
              Patrol();
       
	}
  

    private void Patrol()
    {
        if(nav.remainingDistance == 0)
        {

            patrolTime += Time.deltaTime;
            if (!turned)
            {
                resetTarget = Directions[0].transform.position;
                leftTarget = Directions[1].transform.position;
                rightTarget = Directions[2].transform.position;
                SetFalse();
            }
            if (!ended)
            {
                StopCoroutine(LookingSequence());
                StartCoroutine(LookingSequence());
            }
            if (patrolTime >= 10f)
            {
                StopCoroutine(LookingSequence());
                if (wayPointIndex == wayPoints.Length - 1)
                {
                    wayPointIndex = 0;
 
                }
                else {
                    wayPointIndex++;
                }
                
                patrolTime = 0f;
                
                
            }
            nav.destination=wayPoints[wayPointIndex].transform.position;
        }
        else
        turned = false;
       
    }

    public IEnumerator LookingSequence()
    {
        yield return new WaitForSeconds(1);

        LookLeft();

        yield return new WaitForSeconds(2);

        LookRight();

        yield return new WaitForSeconds(3);

        ResetPosition();
       
        yield return new WaitForSeconds(3);
        ended = true;

    }
    public void LookLeft()
    {
        
        if (transform.rotation != newRotation && !left) //needs to change at a later point, since it is the first way it turns, and there it doesnt do anything at the second point
        {
            newRotation = Quaternion.LookRotation(leftTarget - transform.position);

            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationTime*Time.deltaTime);
        }


    }
    public void LookRight()
    {
        newRotation = Quaternion.LookRotation(rightTarget - transform.position);
        left = true;
        if (transform.rotation != newRotation && !right)
        {
            
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationTime*Time.deltaTime);
        }

    }
    public void ResetPosition()
    {
        
        newRotation = Quaternion.LookRotation(resetTarget-transform.position);
        right = true;
        if (transform.rotation != newRotation && !ended)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationTime * Time.deltaTime);
        }
    }
    public void SetFalse()
    {
        ended = false;
        left = false;
        right = false;
        turned = true;
    }

 
}

/*  private void SearchForPlayer()
    {

        if (nav.remainingDistance == 0 && time < 4f)
        {
           
            standTime += Time.deltaTime;
            time += Time.deltaTime;

            if (time > 3f && !left)
            {
                Debug.Log("Turning left");
                to = new Vector3(transform.eulerAngles.x,
                        90,
                        transform.eulerAngles.z);
                left = true;
                time = 0;



            }
            else if (time > 3f && left && !right)
            {
                Debug.Log("Turning right");
                to = new Vector3(transform.eulerAngles.x,
                        -180,
                        transform.eulerAngles.z);
                right = true;
                time = 0;

            }

            if (!transform.rotation.Equals(Quaternion.Euler(to)) && (left || right))
            {
                RotateToASide(to);
                
            }

            

        }

    }
    private void RotateToASide(Vector3 to)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(to), rotationTime*Time.deltaTime);
    } */

/*{
        nav.speed = patrolSpeed;

        if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {

                if (wayPointIndex == patrolWayPoints.Length - 1)
                    wayPointIndex = 0;
                else
                    wayPointIndex++;

                
                patrolTimer = 0f;

            }
           // else
             //   patrolTimer = 0f;

            nav.destination = patrolWayPoints[wayPointIndex].position;

        }
    }*/
