using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#


public class Rumble : MonoBehaviour {

    bool playerIndexSet = false;
    bool startTimer = false;
    public float HRD;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    float x;

    //
    public int timer = 0;
    int maxTime = 15;

    // Use this for initialization
    void Start()
    {
        // No need to initialize anything for the plugin
    }

    // Update is called once per frame
    void Update()
    {
    	timer++;
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }
        prevState = state;
        state = GamePad.GetState(playerIndex);

        GamePad.SetVibration(playerIndex, 0, x);
        
    if(timer<maxTime && timer>0 && startTimer){
        x = 0.0f;
            startTimer = false;

        

    }
    }
    public void Shake(PlayerIndex index)
    {
        Debug.Log("SHAKE");
        //transform.localRotation *= Quaternion.Euler(0.0f, 100.0f, 0.0f);
        x = 0.10f;
        timer = -5;
        startTimer = true;
    }
}
