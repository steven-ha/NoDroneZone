using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	// path to save the game
	public string dbPath = "http://people.oregonstate.edu/~has/SaveUser.php";

	void Start(){
		if (SceneController.Instance.gameWon == true){
			GameObject.Find ("GameStatus").GetComponent<Text> ().text = "You Won!";
		}
		else{
			GameObject.Find ("GameStatus").GetComponent<Text> ().text = "You Lose!";
		}

		// update the game data
		if (SceneController.Instance.gameFinished == true) {
			GameObject.Find ("FinalScore").GetComponent<Text> ().text = "Score: " + SceneController.Instance.money.ToString ();
		}
	}


	// Update is called once per frame
	void Update () {

	}

	/*
      * Name: ReplayButton
      * Parameters: N/A
      * Pre-Conditions: Used after user has completed the game or lost
      * Post-Conditions: Coroutine to save user game data to the database will be executed. Initial game stats will be used to update the database
      */
	public void ReplayButton()
	{
          //Changed money to 1050-Tom
		StartCoroutine(SaveToDB(SceneController.Instance.userID, 500, 1, 2, 2, 2, 3, 5));
		SceneController.Instance.money = 500;
		SceneController.Instance.shotgun = 2;
		SceneController.Instance.slingshot = 2;
		SceneController.Instance.watergun = 2;
		SceneController.Instance.sceneID = 2;
		SceneController.Instance.lives = 3;
		SceneController.Instance.hitpoints = 5;
		SceneController.Instance.loadScene2 = true;
		SceneController.Instance.Update ();
	}

	/*
      * Name: QuitButton
      * Parameters: N/A
      * Pre-Conditions: N/A
      * Post-Conditions: Resets the scenecontroller back to the login screen
      */
	public void QuitButton()
	{
          //Reset everything
          StartCoroutine(SaveToDB(SceneController.Instance.userID, 500, 1, 2, 2, 2, 3, 5));
          SceneController.Instance.money = 500;
          SceneController.Instance.shotgun = 2;
          SceneController.Instance.slingshot = 2;
          SceneController.Instance.watergun = 2;
          SceneController.Instance.sceneID = 2;
          SceneController.Instance.lives = 3;
          SceneController.Instance.hitpoints = 5;
          //comment these lines out so we go to login instead  
          //SceneController.Instance.loadScene2 = true;
          //SceneController.Instance.Update();

          //Now send back to login screen
		SceneController.Instance.loadScene1 = true;
		SceneController.Instance.Update ();
	}

	IEnumerator SaveToDB(int id, int money, int scene, int shotgunCount, int slingshotCount, int watergunCount, int numLives, int numHitpoints)
	{

		// create new web form
		WWWForm form = new WWWForm();

		// add email and password fields
		form.AddField("userID", id);
		form.AddField("userMoney", money);
		form.AddField("userScene", scene);
		form.AddField("userShotgun", shotgunCount);
		form.AddField("userSlingshot", slingshotCount);
		form.AddField("userWatergun", watergunCount);
		form.AddField("userLives", numLives);
		form.AddField("userHitpoints", numHitpoints);

		// access database
		WWW www = new WWW(dbPath, form);
		yield return www;

		// print out server message
		//Debug.Log(www.text);

	}
}
