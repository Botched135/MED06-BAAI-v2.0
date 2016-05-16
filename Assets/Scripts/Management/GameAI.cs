using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class GameAI : MonoBehaviour {
    [Header("Baselines")]
    public float BPMBaseLine;
    public float HRVSDNNBaseline;
    public float HRVRMSSDBaseline;
    public float GSRBaseline; //Do something clever to set in from the initial test

    [Header("In-game variables")]
    public int GSRSpikes;
    public int numbersOfRoomsComplete;  
    public float UScore, MScore, FScore, FinalScore;
    public float UGSRA =0, MGSRA=0, FGSRA=0, FinalGSRA=0; //GSR averages

    [Header("Lists")]
    public List<int> HRV;
    public List<int> GSR;
    public List<float> BPM;

    private bool calc = false;
    private int winner;
    [SerializeField]
    private int weight;
    private MySceneManager _sceneManager;
    private BaselineGUI GUI;
    public GameObject player;
    private BoxCollider _trigger;
	

	void Start () {
        numbersOfRoomsComplete = 1;
        _sceneManager = GetComponent<MySceneManager>();
        if(_sceneManager._currentState == MySceneManager.SceneState.BaselineRoom)
        {
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            GUI = camera.GetComponent<BaselineGUI>();
        }
        player = GameObject.FindGameObjectWithTag("Player");
        AddTrigger();
        weight = 25;
	}
	void Update()
    {
        if(GUI.time > 10 && !calc)
        {
            TotalScore(HeartRateAverage(BPM), 0, RMSSD(HRV), SDNN(HRV), GSRAverage(GSR));
            ClearVariables();
            StartCoroutine(_sceneManager._LoadScene(0,1f,1, this));
            calc = true;
        }
        
    }

    private void ClearVariables()
    {
        HRV.Clear();
        BPM.Clear();
        GSR.Clear();
        GSRSpikes = 0;
    }

    //------------------------------------------------ SCORE CALCULATION -------------------------------------------------------------------------
    private int Compare(float score1, float score2, float score3)
    {
        int highestScore = 0; //should be 1, 2, or 3
        if (score1 > score2 && score1 > score3)
        {
            highestScore = 1;
        }
        else if (score2 > score1 && score2 > score3)
        {
            highestScore = 2;
        }
        else if (score3 > score1 && score3 > score2)
        {
            highestScore = 3;
        }
        else
        {
            return Compare(UGSRA, MGSRA, FGSRA);   
        }
        return highestScore + 2;
    }
    public void TotalScore(float heartRate, int GSRSpikes, float HRV1, float HRV2, float GSRAverage)
    {
        switch (_sceneManager._currentState)
        {
            case MySceneManager.SceneState.BaselineRoom:
                BPMBaseLine = heartRate;
                HRVSDNNBaseline = HRV2;
                HRVRMSSDBaseline = HRV1;
                GSRBaseline = GSRAverage;
                SaveToFile(HRVRMSSDBaseline, HRVSDNNBaseline, BPMBaseLine, 0, GSRBaseline, 0);
                ClearVariables();
                break;
            case MySceneManager.SceneState.Uncanny:
                UScore = ScoreCalc(heartRate, GSRSpikes, HRV1, HRV2,weight);
                UGSRA = GSRAverage;
                SaveToFile(HRV1, HRV2, heartRate, GSRSpikes, UGSRA, UScore);
                ClearVariables();
                break;
            case MySceneManager.SceneState.Marvelous:
                MScore = ScoreCalc(heartRate, GSRSpikes, HRV1, HRV2,weight);
                MGSRA = GSRAverage;
                SaveToFile(HRV1, HRV2, heartRate, GSRSpikes, GSRAverage, MScore);
                ClearVariables();
                break;
            case MySceneManager.SceneState.Fantastic:
                FScore = ScoreCalc(heartRate, GSRSpikes, HRV1, HRV2, weight);
                FGSRA = GSRAverage;
                SaveToFile(HRV1, HRV2, heartRate, GSRSpikes, GSRAverage, FScore);
                ClearVariables();
                break;
            case MySceneManager.SceneState.FinalRoom:
                FinalScore = ScoreCalc(heartRate, GSRSpikes, HRV1, HRV2, weight);
                FinalGSRA = GSRAverage;
                SaveToFile(HRV1, HRV2, heartRate, GSRSpikes, GSRAverage, FinalScore);
                break;
            default:
                break;
        }
    }
    private float ScoreCalc(float heartRate, int GSRSpikes, float HRV1, float HRV2, float scale)
    {
        float results = 0;

        results = Clamping((heartRate - BPMBaseLine)/2, scale) +
                  Clamping(GSRSpikes * 1.5f, scale) + 
                  Clamping((scale / 2) - HRV1, scale / 2) +
                  Clamping((scale / 2) - HRV2, scale / 2);

        return results;
    }

    private float Clamping(float obj, float weight)
    {
        float results = obj;
        if(results > weight)
        {
            results = weight;
        }
        else if(results < 0)
        {
            results = 0;
        }
        return results;
    }

    //------------------------------------------------ AVERAGE CALCULATIONS-----------------------------------------------------------------------
    private float RMSSD(List<int> hrv) //needs to be calculated at the end of every map 
    {
        
        float results = 0;
        
        if (hrv.Count != 0)
        {
            float sumDiv;
            ulong sum = 0;
            for (int i = 0; i < hrv.Count - 1; i++)
            {
                sum += (ulong)(Mathf.Pow((hrv[i + 1] - hrv[i]), 2));
            }
            sumDiv = sum / (ulong)hrv.Count - 1;
            results = Mathf.Sqrt(sumDiv);
            return results;
        }
        return results;
    }
    private float SDNN(List<int> hrv)
    {
        float results = 0;
        if (hrv.Count != 0)
        {
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
        return results;
    }
    private float HeartRateAverage(List<float> bpm)
    {
        float results = 0;
        if (bpm.Count != 0)
        {
            float sum = 0;
            for (int i = 0; i < bpm.Count; i++)
            {
                sum += bpm[i];
            }
            return results = sum / bpm.Count;
        }
        return results;

    }
    private float GSRAverage(List<int> gsr)
    {
        float results = 0;
        if (gsr.Count != 0)
        {
            float sum = 0;
            for (int i = 0; i < gsr.Count; i++)
            {
                sum += gsr[i];
            }
            return results = sum / gsr.Count;
        }
        return results;
    }


    //------------------------------------------------------------------------- TRIGGER AND REACTION -------------------------------------------------------------------------------
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
                    _trigger.center = new Vector3(-9.12f, 1.54f, 12.5f);
                    break;
                case MySceneManager.SceneState.Fantastic:
                    _trigger.center = new Vector3(-8.4f, 1.37f, -12.59f);
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
    
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            switch (_sceneManager._currentState)
            {
                
                case MySceneManager.SceneState.Uncanny:
                   
                    TotalScore(HeartRateAverage(BPM), GSRSpikes , RMSSD(HRV), SDNN(HRV), GSRAverage(GSR));
                    break;
                case MySceneManager.SceneState.Marvelous:
                    TotalScore(HeartRateAverage(BPM), GSRSpikes, RMSSD(HRV), SDNN(HRV), GSRAverage(GSR));
                    break;
                case MySceneManager.SceneState.Fantastic:
                   
                    TotalScore(HeartRateAverage(BPM), GSRSpikes, RMSSD(HRV), SDNN(HRV), GSRAverage(GSR));
                    break;
                case MySceneManager.SceneState.FinalRoom:
                    TotalScore(HeartRateAverage(BPM), GSRSpikes, RMSSD(HRV), SDNN(HRV), GSRAverage(GSR));
                    break;
                default:
                    Debug.Log("Well, shit");
                    break;
            }
            if (_sceneManager._currentState == MySceneManager.SceneState.FinalRoom)
            {
                _sceneManager.fadeScript.BeginFade(1);
                Application.Quit();
            }
            else if (numbersOfRoomsComplete >=   3)
            {
                _trigger.enabled = false;
                winner = Compare(UScore, MScore, FScore);
                StartCoroutine(_sceneManager._LoadScene(1.5f,2f,winner, this));
            }
            else {
                StopCoroutine(_sceneManager._LoadScene(1.5f, 2f,numbersOfRoomsComplete, this));
                _trigger.enabled = false;
                numbersOfRoomsComplete++;
                StartCoroutine(_sceneManager._LoadScene(1.5f, 2f,numbersOfRoomsComplete, this));

            }

        }

    }

    // ---------------------------------------------------------------------------------- SAVE TO TEXT ----------------------------------------------------------------------------------------------
    public void SaveToFile(float RMSSD, float SDNN, float BPM, int GSRspikes, float GSRAverage, float Score)
    {
        string path = string.Format(@"c:\Data\Test{0}.txt", _sceneManager._currentState);
        if (!File.Exists(path))
        {

            using (StreamWriter writer = File.CreateText(path))
            {
                switch (_sceneManager._currentState)
                {
                    case MySceneManager.SceneState.BaselineRoom:
                        writer.WriteLine("Baseline Room");
                        WriteInformation(BPM, RMSSD, SDNN, GSRAverage, writer);
                        break;
                    case MySceneManager.SceneState.Uncanny:
                        writer.WriteLine("Uncanny");
                        WriteInformation(RMSSD, SDNN, BPM, GSRspikes, GSRAverage, Score, writer);
                        break;
                    case MySceneManager.SceneState.Marvelous:
                        writer.WriteLine("Marvelous");
                        WriteInformation(RMSSD, SDNN, BPM, GSRspikes, GSRAverage, Score, writer);
                        break;
                    case MySceneManager.SceneState.Fantastic:
                        writer.WriteLine("Fantastic");
                        WriteInformation(RMSSD, SDNN, BPM, GSRspikes, GSRAverage, Score, writer);
                        break;
                    case MySceneManager.SceneState.FinalRoom:
                        writer.WriteLine("FinalRoom");
                        WriteInformation(RMSSD, SDNN, BPM, GSRspikes, GSRAverage, Score, writer);
                        break;
                    default:
                        break;
                }
            }
        }
        else {
            using (StreamWriter writer = new StreamWriter(path))
            {
                switch (_sceneManager._currentState) { 


                    case MySceneManager.SceneState.BaselineRoom:
                        writer.WriteLine("Baseline Room");
                        WriteInformation(BPM, RMSSD, SDNN, GSRAverage, writer);
                        break;
                    case MySceneManager.SceneState.Uncanny:
                        writer.WriteLine("Uncanny");
                        WriteInformation(RMSSD, SDNN, BPM, GSRspikes, GSRAverage, Score, writer);
                        break;
                    case MySceneManager.SceneState.Marvelous:
                        writer.WriteLine("Marvelous");
                        WriteInformation(RMSSD, SDNN, BPM, GSRspikes, GSRAverage, Score, writer);
                        break;
                    case MySceneManager.SceneState.Fantastic:
                        writer.WriteLine("Fantastic");
                        WriteInformation(RMSSD, SDNN, BPM, GSRspikes, GSRAverage, Score, writer);
                        break;
                    case MySceneManager.SceneState.FinalRoom:
                        writer.WriteLine("FinalRoom");
                        WriteInformation(RMSSD, SDNN, BPM, GSRspikes, GSRAverage, Score, writer);
                        break;
                    default:
                        break;

                }
            }
        }
        

    }
 
    private void WriteInformation(float RMSSD, float SDNN, float BPM, int GSRspikes, float GSRAverage, float Score, StreamWriter writer)
    {
        
        writer.WriteLine("Total Score: " + Score);
        writer.WriteLine("BPM: " + BPM);
        writer.WriteLine("GSR-spikes: " + GSRspikes);
        writer.WriteLine("GSR-Average: " + GSRAverage);
        writer.WriteLine("RMSSD: " + RMSSD);
        writer.WriteLine("SDNN:" + SDNN);

    }
    private void WriteInformation(float BPMBaseline, float HRVBaselineRMSSD, float HRVBaselineSDNN, float GSRBaseline, StreamWriter writer)
    {
        writer.WriteLine("BPMBaseline: "+BPMBaseLine);
        writer.WriteLine("HRVBaseline(RMSSD): "+HRVBaselineRMSSD);
        writer.WriteLine("HRVBaseline(SDNN): " + HRVBaselineSDNN);
        writer.WriteLine("GSRBaseline: "+GSRBaseline);
    }
}
