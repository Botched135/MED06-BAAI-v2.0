using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public Fading fadeScript;
    private bool trigger = true;
    public enum SceneState
    {
        BaselineRoom,
        Uncanny,
        Marvelous,
        Fantastic,
        FinalRoom
    }
    public SceneState _currentState = SceneState.BaselineRoom;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        fadeScript = GetComponent<Fading>();
        
        _currentState = CheckState();

    }
    public IEnumerator _LoadScene(float timer1, float timer2, int index, GameAI _controller)
    {
        yield return new WaitForSeconds(timer1);
        fadeScript.OnLevelWasLoaded();
        yield return new WaitForSeconds(timer2);
        SceneManager.LoadScene(index);
        yield return new WaitForSeconds(2f);
        OnLevelWasLoaded(index);
        _controller.player = GameObject.FindGameObjectWithTag("Player");
        _controller.AddTrigger();
    }
    void Update()
    {
        _currentState = CheckState();
    }
    void OnGUI()
    {
        /*
        // fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds
        fadeScript.alpha += fadeScript.fadeDir * fadeScript.fadeSpeed * Time.deltaTime;
        // force (clamp) the number to be between 0 and 1 because GUI.color uses Alpha values between 0 and 1
        fadeScript.alpha = Mathf.Clamp01(fadeScript.alpha);

        // set color of our GUI (in this case our texture). All color values remain the same & the Alpha is set to the alpha variable
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeScript.alpha);
        GUI.depth = fadeScript.drawDepth;                                                              // make the black texture render on top (drawn last)
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeScript.fadeOutTexture);       // draw the texture to fit the entire screen area
    */
    }
    public SceneState CheckState()
    {
        SceneState value;
        switch (SceneManager.GetActiveScene().name)
        {
            case "BaselineRoom":
                value = SceneState.BaselineRoom;
                break;
            case "Horror#1MAJA":
                value = SceneState.Uncanny;
                break;
            case "Horror#2_2MAJA":
                value = SceneState.Marvelous;
                break;
            case "Horror#3MAJA":
                value = SceneState.Fantastic;
                break;
           default:
                value = SceneState.FinalRoom;
                break;
        }
        return value;
    }
    public void OnLevelWasLoaded(int index) {
        CheckState();
        fadeScript.BeginFade(-1);
    }
}
