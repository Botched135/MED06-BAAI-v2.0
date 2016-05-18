using UnityEngine;
using System.Collections;

public class BaselineGUI : MonoBehaviour {
    public float time;
    private int Timer;
    public bool mode;
    private int mode1, mode2;
    public float Xsize;
    public float YSize;
    public AudioSource source;
    public GameAI GameAI;
    public GameObject temp;
   
    
	// Use this for initialization
	void Start () {
        Xsize = 500;
        YSize = Xsize;
        source.volume = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
    
        if (!source.isPlaying)
        {
            source.Play();
        }
        time += Time.deltaTime;
        Timer = (int)time;
        if (Timer == 10) //300 for 5 min
        {
            GameAI.intialReads();
        }
    }
    void OnGUI()
    {
        GUI.Box(new Rect((Screen.width/2)-250, 0, 500, 500), "\n \n \n \n \n \n \n This is initial measurement. Sit back, relax, and enjoy the music. \n \n The game will start in "+(300 -Timer).ToString()+" seconds");
        
        if (GUI.Button(new Rect((Screen.width/2)-50,(Screen.height/2)+25,100, 50),"Mode: "+mode2)){
            mode1++;
            mode = !mode;
            mode2 = mode1%2;
        }
    }
}
