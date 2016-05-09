using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameAI : MonoBehaviour {
    public float UHeartRate, MHeartRate, FHeartRate;
    public float URMSSD, USDNN, MRMSSD, MSDNN, FRMSSD, FSDNN;
    public int UGSRlow, MGSRlow, FGSRlow;
    public int numbersOfRoomsComplete;
    public float UScore, MScore, FScore;
    public List<int> HRV;
    private int winner;
    private MySceneManager _sceneManager;
    private GameObject player;
    private BoxCollider _trigger;
	

	void Start () {
        _sceneManager = GetComponent<MySceneManager>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
    private int Compare(float score1, float score2, float score3)
    {
        int highestScore = 0; //should be 1, 2, or 3
        if(score1 > score2 && score1 > score3)
        {
            highestScore = 1;
        }
        else if(score2 > score1 && score2 > score3)
        {
            highestScore = 2;
        }
        else if (score3 > score1 && score3 > score2)
        {
            highestScore = 3;
        }
        else
        {
            //compare the lowest GSR values.
        }
        return highestScore+2;
    }
    private float RMSSD(List<int> hrv) //needs to be calculated at the end of every map 
    {
        float results = 0;
        float sumDiv;
        ulong sum = 0;
        for (int i = 0; i < hrv.Count-1; i++)
        {
            sum += (ulong)(Mathf.Pow((hrv[i + 1] - hrv[i]), 2));
        }
        sumDiv = sum / (ulong)hrv.Count-1;
        results = Mathf.Sqrt(sumDiv);
        return results;
    }
    private float SDNN(List<int> hrv)
    {
        float results = 0;
        float sumDiv;
        float average;
        ulong sum = 0;
        for (int i = 0; i < hrv.Count; i++)
        {
            sum += (ulong)(hrv[i]);
        }
        average = sum / (ulong)(hrv.Count);
        for (int i = 0; i < hrv.Count; i++)
        {
            sum += (ulong)(Mathf.Pow(hrv[i] - average, 2));
        }
        sumDiv = sum / (ulong)(hrv.Count);
        results = Mathf.Sqrt(sumDiv);
        return results;
    }

    public void AddTrigger()
    {
        gameObject.AddComponent<BoxCollider>();
        _trigger = GetComponent<BoxCollider>();
        _trigger.isTrigger = true;
        _trigger.size = new Vector3(4, 2.5f, 4);
        switch (_sceneManager._currentState)
        {
            case MySceneManager.SceneState.Uncanny:
                _trigger.center= new Vector3(-54.7f,1.42f,0);
                break;
            case MySceneManager.SceneState.Marvelous:
                _trigger.center = new Vector3(-9.12f, 1.54f, 13.8f);
                break;
            case MySceneManager.SceneState.Fantastic:
                _trigger.center = new Vector3(-8.4f, -1.35f, -12.59f);
                break;
            default:
                _trigger.center = new Vector3(-54.85f, 1.4f, -0.2f);
                break;
        }

    }
    //make something clever to actually give the scores.

    public void TotalScore()
    {
        switch (_sceneManager._currentState)
        {
            case MySceneManager.SceneState.Uncanny:
                UScore = ScoreCalc(UHeartRate, 2, URMSSD, USDNN, UGSRlow);
                break;
            case MySceneManager.SceneState.Marvelous:
                MScore = ScoreCalc(MHeartRate, 2, MRMSSD, MSDNN, MGSRlow);
                break;
            case MySceneManager.SceneState.Fantastic:
                FScore = ScoreCalc(FHeartRate, 2, FRMSSD, FSDNN, FGSRlow);
                break;
            case MySceneManager.SceneState.FinalRoom:
                //something to write to a document
                break;
            default:
                break;
        }
    }
    private float ScoreCalc(float heartRate, int GSRSpikes, float HRV1, float HRV2, int GSRLow)
    {
        float results = 0;
        // do the actual score calculation
        return results;
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            switch (_sceneManager._currentState)
            {
                case MySceneManager.SceneState.Uncanny:
                    URMSSD = RMSSD(HRV);
                    USDNN = SDNN(HRV);
                    TotalScore();
                    break;
                case MySceneManager.SceneState.Marvelous:
                    MRMSSD = RMSSD(HRV);
                    MSDNN = SDNN(HRV);
                    TotalScore();
                    break;
                case MySceneManager.SceneState.Fantastic:
                    FRMSSD = RMSSD(HRV);
                    FSDNN = SDNN(HRV);
                    TotalScore();
                    break;
                case MySceneManager.SceneState.FinalRoom:
                    break;
                default:
                    break;
            }
            if (_sceneManager._currentState == MySceneManager.SceneState.FinalRoom)
            {
                _sceneManager.fadeScript.BeginFade(1);
                Application.Quit();
            }
            else if (numbersOfRoomsComplete == 3)
            {
                Destroy(GetComponent<BoxCollider>());
                winner = Compare(UScore, MScore, FScore);
                _sceneManager._LoadScene(winner, this);
            }
            else {
                numbersOfRoomsComplete++;
                Destroy(GetComponent<BoxCollider>());
                _sceneManager._LoadScene(numbersOfRoomsComplete, this);
            }
            
        }

    }

}
