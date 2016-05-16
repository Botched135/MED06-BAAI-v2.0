using UnityEngine;
using System.Collections;

public class BaselineGUI : MonoBehaviour {
    public float time;
    private int Timer;
    public float Xsize;
    public float YSize;
	// Use this for initialization
	void Start () {
        Xsize = 500;
        YSize = Xsize;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        Timer = (int)time;
	}
    void OnGUI()
    {
        GUI.Box(new Rect((Screen.width/2)-Xsize/2, 0, Xsize, YSize), "\n \n \n \n \n \n \n This is initial measurement. Sit back, relax, and enjoy the music. \n \n The game will start in "+(300 -Timer).ToString()+" seconds");
    }
}
