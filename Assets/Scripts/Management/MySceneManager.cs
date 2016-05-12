using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public PlayerDeath player;
    public Fading fadeScript;
    private bool trigger = true;
    public enum SceneState
    {
        Uncanny,
        Marvelous,
        Fantastic,
        FinalRoom
    }
    public SceneState _currentState;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeath>() ;
        fadeScript = GetComponent<Fading>();
        DontDestroyOnLoad(gameObject);
        _currentState = CheckState();

    }
    public IEnumerator _LoadScene(int index, GameAI _controller)
    {
        //TO-DO Write down start timer
        fadeScript.BeginFade(2);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(index);
        OnLevelWasLoaded(index);
        yield return null;
        _controller.player = GameObject.FindGameObjectWithTag("Player");
        _controller.AddTrigger();
    }
    void Update()
    {
        _currentState = CheckState();
        if (player.playerDie && trigger)
        {
            fadeScript.BeginFade(2);
            player.PlayDied();
            trigger = false;
        }
    }
    void OnGUI()
    {
        // fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds
        fadeScript.alpha += fadeScript.fadeDir * fadeScript.fadeSpeed * Time.deltaTime;
        // force (clamp) the number to be between 0 and 1 because GUI.color uses Alpha values between 0 and 1
        fadeScript.alpha = Mathf.Clamp01(fadeScript.alpha);

        // set color of our GUI (in this case our texture). All color values remain the same & the Alpha is set to the alpha variable
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeScript.alpha);
        GUI.depth = fadeScript.drawDepth;                                                              // make the black texture render on top (drawn last)
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeScript.fadeOutTexture);       // draw the texture to fit the entire screen area
    }

    public void _EventTime()
    {
        //TO-DO: Give time for when each 
    }
    public SceneState CheckState()
    {
        SceneState value;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Horror#1":
                value = SceneState.Uncanny;
                break;
            case "Horror#2_2":
                value = SceneState.Marvelous;
                break;
            case "Horror#3":
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
