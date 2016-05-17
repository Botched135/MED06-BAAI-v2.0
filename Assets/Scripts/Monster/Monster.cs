using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {
    public NavMeshAgent nav;
    public SpawnEnemy spawnActivator;
    public Animator anim;

    [Header("EnemyMovement")]
    public GameObject[] Directions = new GameObject[3];
    public GameObject[] wayPoints;
    public int wayPointIndex;
    public Vector3 leftTarget, rightTarget, resetTarget, plusVector;
    public Quaternion newRotation;
    public bool left, right, turned, ended;
    public float time, patrolTime, rotationTime, moveSpeed, i;

    [Header("Enemy Sight Variables")]
    public float fieldOfViewAngle = 110f;
    public bool seenPlayer;
    public float angle;
    public SphereCollider col;
    public Vector3 playerPosition, direction;
    public GameObject player;
    public GameObject pivotPoint;

    [Header("Enemy Attack")]
    public float AttackRange;
    public bool PlayerKnockedDown;
    public Vector3 distanceToPlayer;
    public Fading fade;

    void Start () {
        fade = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<Fading>();
        anim = GetComponent<Animator>();
        Directions = GameObject.FindGameObjectsWithTag("Front");
        wayPointIndex = 0;
        wayPoints = new GameObject[2];
        nav = GetComponent<NavMeshAgent>();
        plusVector = new Vector3(0, 4, 0);
        
        rotationTime = 3f;

        wayPoints = GameObject.FindGameObjectsWithTag("Point1");
        player = GameObject.FindGameObjectWithTag("Player");
        time = 0;
        nav.destination = wayPoints[0].transform.position;

        pivotPoint = GameObject.FindGameObjectWithTag("TransformPos");

        //EnemySight
        seenPlayer = false;
    }
	
	
	void Update () {
        moveSpeed = nav.velocity.sqrMagnitude;
        anim.SetFloat("Speed", moveSpeed*moveSpeed);
        Debug.DrawLine(pivotPoint.transform.position, player.transform.position + plusVector - pivotPoint.transform.position, Color.red);
        if (seenPlayer)
        {
            playerPosition = player.transform.position;
            nav.SetDestination(playerPosition);
            Attack();
        }
        else
            Move();
    }

    //====================================================================================================================================================================
    public virtual void Move()
    {
        if (seenPlayer == true)
            return;
        if (nav.remainingDistance == 0)
        {

            patrolTime += Time.deltaTime;
            if (!turned)
            {
                resetTarget = Directions[0].transform.position;
                leftTarget = Directions[1].transform.position;
                rightTarget = Directions[2].transform.position;
                ResetLookVariables();
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
            nav.destination = wayPoints[wayPointIndex].transform.position;
        }
        else
            turned = false;

    }
    public IEnumerator LookingSequence()
    {
        
        if (seenPlayer)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((player.transform.position+ new Vector3(0, 0.7f, 0)) - pivotPoint.transform.position), 3);
            //yield return new WaitForSeconds(0.5f);
            yield break;
        }
        yield return new WaitForSeconds(1);
        LookLeft();

        
        if (seenPlayer)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((player.transform.position+ new Vector3(0, 0.7f, 0)) - pivotPoint.transform.position), 3);
            yield return new WaitForSeconds(0.5f);
            yield break;
        }
        yield return new WaitForSeconds(2);
        LookRight();

       
        if (seenPlayer)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((player.transform.position+ new Vector3(0, 0.7f, 0)) - pivotPoint.transform.position), 3);
            yield return new WaitForSeconds(1);
            yield break;
        }
        yield return new WaitForSeconds(2);
        ResetLook();

        yield return new WaitForSeconds(1);
        ended = true;
    }
    public void LookLeft()
    {
        if (transform.rotation != newRotation && !left) //needs to change at a later point, since it is the first way it turns, and there it doesnt do anything at the second point
        {
            newRotation = Quaternion.LookRotation(leftTarget - pivotPoint.transform.position);

            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationTime * Time.deltaTime);
        }

    }
    public void LookRight()
    {
        newRotation = Quaternion.LookRotation(rightTarget - pivotPoint.transform.position);
        left = true;
        if (transform.rotation != newRotation && !right)
        {

            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationTime * Time.deltaTime);
        }

    }
    public void ResetLook()
    {
        newRotation = Quaternion.LookRotation(resetTarget - pivotPoint.transform.position);
        right = true;
        if (transform.rotation != newRotation && !ended)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationTime * Time.deltaTime);
        }
    }
    public void ResetLookVariables()
    {
        ended = false;
        left = false;
        right = false;
        turned = true;

    }
    //============================================================================================================================================================
   /* private void OnTriggerStay(Collider other) //EnemySight - should use base keyword in children classes
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
                        this.NoticePlayer();
                        seenPlayer = true;
                    }
                }
            }
        }
    }*/
    //============================================================================================================================================================
    public virtual void Attack()
    {
        distanceToPlayer = player.transform.position - transform.position;
        if (AttackRange * AttackRange > distanceToPlayer.sqrMagnitude)
        {
            if (PlayerKnockedDown)
            {
                StartCoroutine(KnockBack(PlayerKnockedDown));
            }
            else {
                
                StartCoroutine(KnockBack());
            }
        }
        else {
            //StopCoroutine(KnockBack());
        }
    }
    public virtual IEnumerator KnockBack()
    {
        nav.Stop();
        anim.SetBool("Attack", true);
        yield return null;
        anim.SetBool("Attack", false);
        yield return new WaitForSeconds(3);
        nav.Resume();
        PlayerKnockedDown = true;
    }
    public virtual IEnumerator KnockBack(bool killer)
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
            yield return null;
            anim.SetBool("Attack", false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public virtual void NoticePlayer()
    {
    }
}
