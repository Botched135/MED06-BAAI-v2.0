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
    void Start()
    {
        fadeScript = GetComponent<Fading>();
        DontDestroyOnLoad(gameObject);
        _currentState = CheckState();

    }
    void Update()
    {
        Debug.Log(_currentState);
    }

    public void _LoadScene(int index, GameAI _controller)
    {
        fadeScript.BeginFade(1);
        SceneManager.LoadScene(index);
        _controller.AddTrigger();
        fadeScript.OnLevelWasLoaded();
        _currentState = CheckState();

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
}
