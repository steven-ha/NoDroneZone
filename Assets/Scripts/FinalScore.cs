using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour {

    [SerializeField]
    private Text userFinalScoreText;

	// Use this for initialization
	void Start () {
        userFinalScoreText.text = SceneController.Instance.money.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
