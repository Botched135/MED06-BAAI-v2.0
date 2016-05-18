using UnityEngine;
using System.Collections;

public class MannequineScript : MonoBehaviour {
    private PlayerSight PlayerSight;
    private GameObject Player;
    public AudioClip teleportSound;
    public int numberMan;
    private Vector3 []direction = new Vector3[4];
    private Vector3 tempVec;
    public GameObject[] Mannequnies = new GameObject[4];
    private Vector3[,] teleportPoints = new Vector3[4,5];
    private GameObject[] TPGameObject = new GameObject[5];
    public BoxCollider[] TPCoorsCollider = new BoxCollider[5];
    private Vector3[] temp = new Vector3[4];
    public float maxDistance;
    private Vector3 TPDirections;
	void Start(){
        Player = GameObject.FindGameObjectWithTag("MainCamera");
        PlayerSight = Player.GetComponent<PlayerSight>();
        maxDistance = 12f;
    }
	void OnEnable () {
        TPGameObject = GameObject.FindGameObjectsWithTag("TPCoors"+numberMan);
        for (int i = 0; i < TPCoorsCollider.Length; i++)
        {
            TPCoorsCollider[i] = TPGameObject[i].GetComponent<BoxCollider>();
        }
        Player = GameObject.FindGameObjectWithTag("MainCamera");
        PlayerSight = Player.GetComponent<PlayerSight>();
        
        for (int i = 0; i < Mannequnies.Length; i++)
        {
            TPGameObject = GameObject.FindGameObjectsWithTag("TPSet"+(i+1));
            for (int j = 0; j < TPGameObject.Length; j++)
            {
                teleportPoints[i, j] = TPGameObject[j].transform.position;
            }
        }
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = teleportPoints[i, 0];
        }
        
        for (int i = 0; i < Mannequnies.Length; i++)
        {
            direction[i] = Player.transform.position - Mannequnies[i].transform.position;
        }
        tempVec = direction[0];

        TPDirections =  TPCoorsCollider[0].gameObject.transform.position -Player.transform.position;


    }
	
	void Update () {

        for (int i = 0; i < Mannequnies.Length; i++)
        {
            direction[i] = Player.transform.position - Mannequnies[i].transform.position;
        }
        Evaluate();

        if(!PlayerSight.mannequineSeen && tempVec.sqrMagnitude > maxDistance * maxDistance)
        {
            Teleport();
        }
	}

    void Teleport()
    {


        for (int i = 0; i < Mannequnies.Length; i++)
        {
            for (int j = 0; j < TPGameObject.Length; j++)
            {
                float distance1 = (Player.transform.position - temp[i]).sqrMagnitude;
                float distance2 = (Player.transform.position - teleportPoints[i, j]).sqrMagnitude;
                if (distance1 > distance2)
                {

                    temp[i] = teleportPoints[i, j];

                }
                TPDirections = TPCoorsCollider[i].gameObject.transform.position - Player.transform.position;
                float angle = Vector3.Angle(TPDirections, Vector3.forward);

                if (angle < PlayerSight.fieldOfViewAngle * 0.5)
                {
                    RaycastHit hit;

                    if (Physics.Raycast(Player.transform.position, TPDirections, out hit))
                    {
                        
                        for (int y = 0; y < TPCoorsCollider.Length; y++)
                        {
                            if (hit.collider == TPCoorsCollider[i])
                            {
                                return; 
                            }
                        }


                    }

                }
                    



            }
            Mannequnies[i].transform.position = new Vector3(temp[i].x, 1.5f, temp[i].z);
            Mannequnies[i].transform.LookAt(new Vector3(Player.gameObject.transform.position.x, Mannequnies[i].transform.position.y, Player.gameObject.transform.position.z));

            AudioSource.PlayClipAtPoint(teleportSound, Mannequnies[i].transform.position, 0.7f);

        }
        

    }
    void Evaluate()
    {
        for (int i = 0; i < direction.Length; i++)
        {
            float distance1 = tempVec.sqrMagnitude;
            float distance2 = direction[i].sqrMagnitude;

            if (distance2 < distance1)
                tempVec = direction[i];
            else
                tempVec = direction[0];
        }

    }
}