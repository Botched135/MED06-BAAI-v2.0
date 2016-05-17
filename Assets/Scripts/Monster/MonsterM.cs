 using UnityEngine;
using System.Collections;

public class MonsterM : Monster {
    public GameObject Fading;

    private bool trigger = false, trigger2 = false;
    //Soundclips etc
    public AudioClip laugh01;
    public AudioClip scream01;
    public AudioClip scream02;
    public AudioClip jumpScareSound;
    public AudioClip attackSound;
    bool attacked = true;
    public bool firstTimeSeenPlayer = true;
    int select = 1;

    private GameAI GameAI;

	// Use this for initialization
	void Start () {
        //GUI
        nav = GetComponent<NavMeshAgent>();
        
        trigger = false;
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

        GameObject _temp;
        _temp = GameObject.FindGameObjectWithTag("EditorOnly");
        fade = _temp.GetComponent<Fading>();
        GameAI = _temp.GetComponent<GameAI>();
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
            if (!trigger2)
            {
                nav.Resume();
                trigger2 = true;
            }
            playerPosition = player.transform.position;
            anim.SetBool("SeenPlayer", true);
            nav.speed = 2;
            NoticePlayer();
            nav.SetDestination(playerPosition);
            Attack();

            if (!trigger)
            {
                StartCoroutine(laugh());
                trigger = true;

            }

        }
        else if (!seenPlayer)
            Move();
    }


    public IEnumerator laugh()
    {
       
            yield return new WaitForSeconds(4.0f);
            switch (select)
            {
                case 1:
                    AudioSource.PlayClipAtPoint(scream01, transform.position);
                    break;
                case 2:
                    AudioSource.PlayClipAtPoint(scream02, transform.position);
                    break;
                
            }
            select++;
            if (select == 3) { select = 1; }
            
        
    }
    public override IEnumerator KnockBack()
    {
        nav.Stop();
        anim.SetBool("Attack", true);
        yield return null;
        anim.SetBool("Attack", false);

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
        //
       
        yield return new WaitForSeconds(3);
        nav.Resume();
        attacked = true;
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

            //Sound play
            if (attacked == true)
            {
                AudioSource.PlayClipAtPoint(attackSound, player.transform.position, 1.0F);
                attacked = false;
            }

            //GUI
            fade.OnLevelWasLoaded();
            //
            
            yield return null;
            anim.SetBool("Attack", false);
            yield return new WaitForSeconds(0.5f);
            attacked = true;
        }
    }

    public override void NoticePlayer()
    {
       //play sound
    	if(firstTimeSeenPlayer){
            GameAI.SaveToFile(GameAI.time);
    	 AudioSource.PlayClipAtPoint(laugh01, transform.position);
         AudioSource.PlayClipAtPoint( jumpScareSound, player.transform.position, 1.0F);
    	 firstTimeSeenPlayer=false;
    	}
    }
}
