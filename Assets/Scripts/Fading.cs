using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour {

	public Texture2D fadeOutTexture;	// the texture that will overlay the screen. This can be a black image or a loading graphic
	public float fadeSpeed = 0.8f;		// the fading speed

	private int drawDepth = -1000;		// the texture's order in the draw hierarchy: a low number means it renders on top
	private float alpha = 1.0f;			// the texture's alpha value between 0 and 1
	private int fadeDir = -1;			// the direction to fade: in = -1 or out = 1

	void Update(){
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			BeginFade(1);
			Application.LoadLevel (0); //U
			OnLevelWasLoaded();
			
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			BeginFade(1);
			Application.LoadLevel (1); //M
			OnLevelWasLoaded();
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			BeginFade(1);
			Application.LoadLevel (2); //F
			OnLevelWasLoaded();
			
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			BeginFade(1);
			Application.LoadLevel (3); //U
			OnLevelWasLoaded();
			
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			BeginFade(1);
			Application.LoadLevel (4); //M
			OnLevelWasLoaded();
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			BeginFade(1);
			Application.LoadLevel (5); //F
			OnLevelWasLoaded();
			
		}
	}
	void OnGUI()
	{
		// fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		// force (clamp) the number to be between 0 and 1 because GUI.color uses Alpha values between 0 and 1
		alpha = Mathf.Clamp01(alpha);

		// set color of our GUI (in this case our texture). All color values remain the same & the Alpha is set to the alpha variable
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;																// make the black texture render on top (drawn last)
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);		// draw the texture to fit the entire screen area
	}

	// sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
	public float BeginFade (int direction)
	{
		fadeDir = direction;
		return (fadeSpeed);
	}

	// OnLevelWasLoaded is called when a level is loaded. It takes loaded level index (int) as a parameter so you can limit the fade in to certain scenes.
	void OnLevelWasLoaded()
	{
		// alpha = 1;		// use this if the alpha is not set to 1 by default
		BeginFade(-1);		// call the fade in function
	}
}
