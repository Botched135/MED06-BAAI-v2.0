using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#


public class Rumble : MonoBehaviour {

    
    public float BPM, preBPM;
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    float x;

    //
    public float timer = 0;
    float maxTime;

    void Update()
    {
        maxTime = 60/BPM;
        timer += Time.deltaTime;
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
        if(timer>maxTime){
            Debug.Log(maxTime+" "+BPM);
            transform.localRotation *= Quaternion.Euler(0.0f, 100.0f, 0.0f);
            x = 0.10f;
            timer = maxTime*-1;
            Debug.Log("SHAKE!");
         } 
        if(timer<maxTime && timer>0){
            x = 0.0f;
        }
    }
}