using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public Fading fadeScript;
    public enum SceneState
    {
        Elevator,
        Uncanny,
        Marvelous,
        Fantastic,
        FinalRoom
    }
    public SceneState _currentState = SceneState.Elevator;
    // Use this for initialization
    void Start()
    {
        fadeScript = GetComponent<Fading>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void _LoadScene(int index, GameAI _controller)
    {
        fadeScript.BeginFade(1);
        SceneManager.LoadScene(index);
        _controller.AddTrigger();
        fadeScript.OnLevelWasLoaded();

    }
}
