using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class GameAI : MonoBehaviour {
    public float UHeartRate, MHeartRate, FHeartRate;
    public float URMSSD, USDNN, MRMSSD, MSDNN, FRMSSD, FSDNN;
    public int UGSRlow, MGSRlow, FGSRlow;
    public int numbersOfRoomsComplete;
    public float UScore, MScore, FScore, FinalScore;
    public List<int> HRV;
    public List<float> heartRate;
    private int winner;
    private MySceneManager _sceneManager;
    private GameObject player;
    private BoxCollider _trigger;
	

	void Start () {
        _sceneManager = GetComponent<MySceneManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        AddTrigger();
	}
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SaveToFile(1, 1, 1, 1, 1, 1);
            
        }

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
       
        if (GetComponent<BoxCollider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
            _trigger = GetComponent<BoxCollider>();
            _trigger.isTrigger = true;
            _trigger.size = new Vector3(4, 2.5f, 4);
        }
       
        switch (_sceneManager._currentState)
            {
                case MySceneManager.SceneState.Uncanny:
                    _trigger.center = new Vector3(-54.7f, 1.42f, 0);
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
        if (!_trigger.enabled)
        {
            _trigger.enabled = true;
        }

    }
    //make something clever to actually give the scores.

    public void TotalScore(float heartRate, int GSRSpikes, float HRV1, float HRV2, float GSRAverage)
    {
        switch (_sceneManager._currentState)
        {
            case MySceneManager.SceneState.Uncanny:
                UScore = ScoreCalc(heartRate, GSRSpikes, HRV1, HRV2, GSRAverage);
                SaveToFile(HRV1, HRV2, heartRate, GSRSpikes, GSRAverage, UScore);
                HRV.Clear();
                break;
            case MySceneManager.SceneState.Marvelous:
                MScore = ScoreCalc(heartRate, GSRSpikes, HRV1, HRV2, GSRAverage);
                SaveToFile(HRV1, HRV2, heartRate, GSRSpikes, GSRAverage, MScore);
                HRV.Clear();
                break;
            case MySceneManager.SceneState.Fantastic:
                FScore = ScoreCalc(heartRate, GSRSpikes, HRV1, HRV2, GSRAverage);
                SaveToFile(HRV1, HRV2, heartRate, GSRSpikes, GSRAverage, FScore);
                HRV.Clear();
                break;
            case MySceneManager.SceneState.FinalRoom:
                FinalScore = ScoreCalc(heartRate, GSRSpikes, HRV1, HRV2, GSRAverage);
                SaveToFile(HRV1, HRV2, heartRate, GSRSpikes, GSRAverage, FinalScore);
                break;
            default:
                break;
        }
    }
    private float ScoreCalc(float heartRate, int GSRSpikes, float HRV1, float HRV2, float GSRAverage)
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
                    TotalScore(UHeartRate, UGSRlow, RMSSD(HRV), SDNN(HRV), 2);
                    break;
                case MySceneManager.SceneState.Marvelous:
                    TotalScore(MHeartRate, MGSRlow, RMSSD(HRV), SDNN(HRV), 2);
                    break;
                case MySceneManager.SceneState.Fantastic:
                    TotalScore(FHeartRate, FGSRlow, RMSSD(HRV), SDNN(HRV), 2);
                    break;
                case MySceneManager.SceneState.FinalRoom:
                    TotalScore(UHeartRate, UGSRlow, RMSSD(HRV), SDNN(HRV), 2);
                    break;
                default:
                    break;
            }
            if (_sceneManager._currentState == MySceneManager.SceneState.FinalRoom)
            {
                _sceneManager.fadeScript.BeginFade(1);
                Application.Quit();
            }
            else if (numbersOfRoomsComplete == 2)
            {
                _trigger.enabled = false;
                winner = Compare(UScore, MScore, FScore);
                StartCoroutine(_sceneManager._LoadScene(winner, this));
            }
            else {
                _trigger.enabled = false;
                numbersOfRoomsComplete++;
                StartCoroutine(_sceneManager._LoadScene(numbersOfRoomsComplete, this));

            }

        }

    }
    public void SaveToFile(float RMSSD, float SDNN, float BPM, int GSRspikes, float GSRAverage, float Score)
    {
        string path = string.Format(@"c:\Data\Test{0}.txt", _sceneManager._currentState);
        Debug.Log("Stuff");
        
        if (!File.Exists(path))
        {
            
            using (StreamWriter writer = File.CreateText(path))
            {
                switch (_sceneManager._currentState)
                {
                    case MySceneManager.SceneState.Uncanny:
                        writer.WriteLine("Uncanny");
                        writer.WriteLine("Total Score: " + Score);
                        writer.WriteLine("BPM: " + BPM);
                        writer.WriteLine("GSR-spikes: " + GSRspikes);
                        writer.WriteLine("GSR-Average: " + GSRAverage);
                        writer.WriteLine("RMSSD: " + RMSSD);
                        writer.WriteLine("SDNN:" + SDNN);
                        break;
                    case MySceneManager.SceneState.Marvelous:
                        writer.WriteLine("Marvelous");
                        writer.WriteLine("Total Score: " + Score);
                        writer.WriteLine("BPM: " + BPM);
                        writer.WriteLine("GSR-spikes: " + GSRspikes);
                        writer.WriteLine("GSR-Average: " + GSRAverage);
                        writer.WriteLine("RMSSD: " + RMSSD);
                        writer.WriteLine("SDNN:" + SDNN);
                        break;
                    case MySceneManager.SceneState.Fantastic:
                        writer.WriteLine("Fantastic");
                        writer.WriteLine("Total Score: " + Score);
                        writer.WriteLine("BPM: " + BPM);
                        writer.WriteLine("GSR-spikes: " + GSRspikes);
                        writer.WriteLine("GSR-Average: " + GSRAverage);
                        writer.WriteLine("RMSSD: " + RMSSD);
                        writer.WriteLine("SDNN:" + SDNN);
                        break;
                    case MySceneManager.SceneState.FinalRoom:
                        writer.WriteLine("FinalRoom");
                        writer.WriteLine("Total Score: " + Score);
                        writer.WriteLine("BPM: " + BPM);
                        writer.WriteLine("GSR-spikes: " + GSRspikes);
                        writer.WriteLine("GSR-Average: " + GSRAverage);
                        writer.WriteLine("RMSSD: " + RMSSD);
                        writer.WriteLine("SDNN:" + SDNN);
                        break;
                    default:
                        break;
                }
            }     
        }


    }
    public void SaveToFile()
    {
        string path = string.Format(@"c:\Data\Test{0}.txt", _sceneManager._currentState);

        if (!File.Exists(path))
        {
            using (StreamWriter writer = File.CreateText(path))
            {
                switch (_sceneManager._currentState)
                {
                    case MySceneManager.SceneState.Uncanny:
                        writer.WriteLine("Uncanny");
                        break;
                    case MySceneManager.SceneState.Marvelous:
                        writer.WriteLine("Marvelous");
                        break;
                    case MySceneManager.SceneState.Fantastic:
                        writer.WriteLine("Fantastic");
                        break;
                    case MySceneManager.SceneState.FinalRoom:
                        writer.WriteLine("FinalRoom");
                        break;
                    default:
                        break;
                }
            }
        }
    }


}
