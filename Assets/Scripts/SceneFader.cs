using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour {

	public Image img; // UI img 
	public AnimationCurve curve; // curve used to control fading

	void Start ()
	{
		StartCoroutine(FadeIn());
	}

	public void FadeTo (string scene)
	{
		StartCoroutine(FadeOut(scene));
	}

	IEnumerator FadeIn ()
	{
		// duration of fading in
		float fadeTime = 2f;

		// if there is still fadeTime, change the color of the UI img
		while (fadeTime > 0f)
		{
			// decrement fadeTime
			fadeTime -= Time.deltaTime;

			// find the point on the curve that will be used to augment the img color
			float curvePoint = curve.Evaluate(fadeTime);

			// update the img color using the calculated curve point
			img.color = new Color (0f, 0f, 0f, curvePoint);

			// return
			yield return 0;
		}
	}

	IEnumerator FadeOut(string sceneName)
	{
		// duraction of fading in
		float fadeTime = 0f;

		// fade out while fadeTime is less than 1sec
		while (fadeTime < 2f)
		{
			// add to the fadeTime
			fadeTime += Time.deltaTime;

			// find the point on the curve that will be used to augment the img color
			float curvePoint = curve.Evaluate(fadeTime);

			// update the img color using the calculated curve point
			img.color = new Color(0f, 0f, 0f, curvePoint);

			// return
			yield return 0;
		}

		// load the next scene
		SceneManager.LoadScene(sceneName);
	}

}