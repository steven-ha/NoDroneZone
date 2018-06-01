using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Collections.Generic;

public class SceneController : MonoBehaviour {

	// instance of the scenecontroller used to keep track of the scenes
	public static SceneController Instance;

	// name of the scenes in the game
	string scene1 = "Login";
	string scene2 = "Park";
	string scene3 = "Neighborhood";
	string scene4 = "World";
	string scene5 = "GameOver";

	// bools used by the script to know if a scene needs to be loaded
	public bool loadScene1 = false;
	public bool loadScene2 = false;
	public bool loadScene3 = false;
	public bool loadScene4 = false;
	public bool loadScene5 = false;
	public bool sceneLoaded = false;
	public bool updateStats = false;

	// attributes used to record user data
	public int sceneID = -1; 
	public int userID = 0;
	public string email = "default";
	public string password = "default";
	public string fName = "default";
	public string lName = "default";
	public int hitpoints = -1;
	public int lives = -1;
	public int money = -1;
	public int shotgun = -1;
	public int slingshot = -1;
	public int watergun = -1;

	// attributes of user data prior to starting a level, used for saving mid level
	public int pHitpoints = -1;
	public int pLives = -1;
	public int pMoney = -1;
	public int pShotgun = -1;
	public int pSlingshot = -1;
	public int pWatergun = -1;

	// purchased towers
	public int purSlingshot = 0;
	public int purWater = 0;
	public int purShotgun = 0;

	public int costSlingshot = 100;
	public int costWater = 300;
	public int costShotgun = 500;

	public float usedPercentage = 0.5f;
	public bool gameWon = false;
	public bool gameFinished = false;

	// path to php file to save game
	public string dbPath = "http://people.oregonstate.edu/~has/SaveUser.php";

	// create gameobject that will persist during the duration of the game	
	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}
		
	// Update is called once per frame
	public void Update () {

		// load login scene if a user  if loadScene1 is true
		if (loadScene1 == true){
			// reset loadScene2 to false so game doesn't continually load scene2
			resetController();
			loadScene1 = false;

			//Debug.Log ("load scene 1");
			// fade into scene 2
			FindObjectOfType<SceneFader>().FadeTo(scene1);

		}
			
		// load park scene if a user is loaded and loadScene2 is true
		if (loadScene2 == true){
			// reset loadScene2 to false so game doesn't continually load scene2
			loadScene2 = false;

			// buy back towers
	//		buyBackTowers ();
			resetPurchased ();

			// save previous data
			setPreviousData ();

			sceneID = 1;



			//Debug.Log ("load scene 2");
			// fade into scene 2
			FindObjectOfType<SceneFader>().FadeTo(scene2);
		}

		// load park copy scene if a user is loaded and loadScene3 is true
		if (loadScene3 == true){
			// reset loadScene3 to false so game doesn't continually load scene2
			loadScene3 = false;

			// Change: buy back towers, ONLY if we just came from Park
               if (sceneID == 1)
               {
                    buyBackTowers();
               }

			resetPurchased ();

			// save previous data
			setPreviousData ();

			sceneID = 2;

			//Debug.Log ("load scene 3");


			// fade into scene 3
			FindObjectOfType<SceneFader>().FadeTo(scene3);
		}

		// load world scene if a user is loaded and loadScene4 is true
		if (loadScene4 == true){
			// reset loadScene4 to false so game doesn't continually load scene4
			loadScene4 = false;

               // Change: buy back towers, ONLY if we just came from Neighborhood
               if (sceneID == 2)
               {
                    buyBackTowers();
               }

			resetPurchased ();

			// save previous data
			setPreviousData ();

			sceneID = 3;

			//Debug.Log ("load scene 4");

			// fade into scene 4
			FindObjectOfType<SceneFader>().FadeTo(scene4);
		}

		// load gameover scene if a user is loaded and loadScene4 is true
		if (loadScene5 == true){
			// reset loadScene4 to false so game doesn't continually load scene4
			updateStats = false;
			loadScene5 = false;

			// fade into scene 4
			FindObjectOfType<SceneFader>().FadeTo(scene5);
		}

		// update the game data
		if (updateStats == true) {
			GameObject.Find ("UserScore").GetComponent<Text> ().text = money.ToString ();
			//GameObject.Find ("NumHP").GetComponent<Text> ().text = hitpoints.ToString ();
			GameObject.Find ("LevelNum").GetComponent<Text> ().text = sceneID.ToString ();
		}




	}

	/*
	 * Name: SaveButton
	 * Parameters: N/A
	 * Pre-Conditions: Scene should have a pause screen that has a save button to execute the save
	 * Post-Conditions: Coroutine to save user game data to the database will be executed 
	 */
	public void SaveButton(){
		StartCoroutine(SaveToDB(userID, pMoney, sceneID, pShotgun, pSlingshot, pWatergun, pLives, pHitpoints));

	}

	/*
	 * Name: setPreviousData
	 * Parameters: N/A
	 * Pre-Conditions: should be used prior to starting a level 
	 * Post-Conditions: p attributes will be set to in game attributes
	 */
	public void setPreviousData(){
		pHitpoints = hitpoints;
		pLives = lives;
		pShotgun = shotgun;
		pWatergun = watergun;
		pSlingshot = slingshot;
		pMoney = money;
	}

	/*
	 * Name: buyBackTowers
	 * Parameters: N/A
	 * Pre-Conditions: number of towers purchased need to be accounted for
	 * Post-Conditions: money will be updated to account for the purchased towers
	 */
	public void buyBackTowers(){
		int buyback = Mathf.FloorToInt((purSlingshot * costSlingshot + purWater * costWater + purShotgun * costShotgun) * usedPercentage);

		money += buyback;
	}

	/*
	 * Name: resetPurchased
	 * Parameters: N/A
	 * Pre-Conditions: number of towers purchased need to be accounted for
	 * Post-Conditions: towers purchased are reset to 0
	 */
	public void resetPurchased(){
		purSlingshot = 0;
		purWater = 0;
		purShotgun = 0;
	}

	/*
	 * Name: SaveToDB
	 * Parameters: N/A
	 * Pre-Conditions: Database userGameData table should exist before calling this method
	 * Post-Conditions: Server will send back message stating "Save successful." or "Save not successful."
	 */
	public IEnumerator	SaveToDB(int id, int money, int scene, int shotgunCount, int slingshotCount, int watergunCount, int numLives, int numHitpoints){

		// create new web form
		WWWForm form = new WWWForm ();

		// add email and password fields
		form.AddField ("userID", id);
		form.AddField ("userMoney", money);  
		form.AddField ("userScene", scene);
		form.AddField ("userShotgun", shotgunCount);
		form.AddField ("userSlingshot", slingshotCount);
		form.AddField ("userWatergun", watergunCount);
		form.AddField ("userLives", numLives);
		form.AddField ("userHitpoints", numHitpoints);

		// access database
		WWW www = new WWW (dbPath, form);
		yield return www;

		// print out server message
		//Debug.Log (www.text);


	}

	/*
		 * Name: resetController
		 * Parameters: N/A
		 * Pre-Conditions: should only be used when the user quits the game and returns to the login screen
		 * Post-Conditions: attributes will be reset.
		 */
	public void resetController(){
		// bools used by the script to know if a scene needs to be loaded
		loadScene1 = false;
		loadScene2 = false;
		loadScene3 = false;
		loadScene4 = false;
		loadScene5 = false;
		sceneLoaded = false;
		updateStats = false;

		// attributes used to record user data
		sceneID = -1; 
		userID = 0;
		email = "default";
		password = "default";
		fName = "default";
		lName = "default";
		hitpoints = -1;
		lives = -1;
		money = -1;
		shotgun = -1;
		slingshot = -1;
		watergun = -1;

		// attributes of user data prior to starting a level, used for saving mid level
		pHitpoints = -1;
		pLives = -1;
		pMoney = -1;
		pShotgun = -1;
		pSlingshot = -1;
		pWatergun = -1;

		// purchased towers
		purSlingshot = 0;
		purWater = 0;
		purShotgun = 0;

		costSlingshot = 100;
		costWater = 300;
		costShotgun = 500;

		usedPercentage = 0.5f;
		gameWon = false;
		gameFinished = false;
	}

}