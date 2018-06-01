using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class WaveGenerator : MonoBehaviour {

	// MAX number of waves the level will generate
	const int NUMBERWAVES = 10;

    // testing
    public GameObject enemyBee;
    public GameObject enemyCopter;
    public GameObject enemyPredator;
    public GameObject enemyX45;

	// text to hold wave/level number 
	public Text waveNumber; 
	public Text levelNumber;

	// timer used to control wave generator, enemy generated when countDown is 0
	private float countDown = 5f;

	// records the number of waves currently generated
	private int waveCount = 0;

	// location where enemy will be spawned
	public Transform spawnPoint;

	// number of enemies that will be generated in a wave, randomly calculated
	public int randomNumEnemy = 2;

	// amount of time between the end of one wave and the start of the next
	public float waveInterval = 4.5f;

	// amount of time between the two enemy units
	public float enemyInterval = 0.5f;

	// gameobject used to show countdown of next enemy wave
	public Text CountDownTimer;

	// name of the scene
	public string sceneName;

	// audio source 
    public AudioSource boomer;

	// array used to keep track of the number of enemies on the map
	public GameObject[] listEnemies;

	// flag to keep track if the last enemy has been generated
	public bool lastEnemyGenerated = false;

	// time to wait before calling transition script
	public float waitTime = 5f;

     public bool impactBool = false;


	public string[] waveData;

	private int waveEnemyType;

	private int waveEnemyCount;


	void Start(){
		// get the name of the scene
		sceneName = SceneManager.GetActiveScene().name;
		SceneController.Instance.updateStats = true;

		// access the enemy array text file for the scene
		waveData = readTextFile ((sceneName).ToLower() + "EnemyArray");

	
	}


	void Update(){
		// generate a wave of enemies when the countdown reaches 0 and the number of waves hasn't been reached
		if (countDown <= 0f && waveCount != NUMBERWAVES) {
			StartCoroutine(generateWave (waveEnemyType, waveEnemyCount));
			countDown = waveEnemyType * waveEnemyCount + waveInterval;
		}

		// print out info for debugging
		//print ("RANDOM: " + randomNumEnemy + " COUNTDOWN: " + countDown);

		// decrement the countDown timer
		countDown -= Time.deltaTime;

		// print out countdown when a wave is within 3 seconds of being generated
		if (countDown < 4 && countDown >= 0 && waveCount != NUMBERWAVES) {
			// set the enemy type and count to be used in the generateWave function
			waveEnemyType = Convert.ToInt32 (waveData [waveCount * 2]);
			waveEnemyCount = Convert.ToInt32 (waveData [waveCount * 2 + 1]);
			// update the count down timer text
			CountDownTimer.text = "INCOMING WAVE!\n" + Mathf.Floor (countDown).ToString () + " seconds";
		} else {
			// clear the count down timer text
			CountDownTimer.text = "";
		}
			
		// get the number of enemies that currently are on the map
		listEnemies = GameObject.FindGameObjectsWithTag ("Enemy");

		// transition scenes when the map contains no enemies and the number of enemies has been generated
		if (listEnemies.Length == 0 && lastEnemyGenerated == true && waveCount == NUMBERWAVES) {
		//if (countDown <= -30.0f && waveCount == NUMBERWAVES  && tempBool==true) {
			SceneController.Instance.updateStats = false;

			waitTime -= Time.deltaTime;
			// wait 5 seconds before loading the next scene
			if (waitTime <= 0f) {

				//reset wait time to 5 seconds
				waitTime = 5f;
				Debug.Log (sceneName);

				if (sceneName == "Park") {
					SceneController.Instance.loadScene3 = true;
				}
				if (sceneName == "Neighborhood") {
					SceneController.Instance.loadScene4 = true;
					SceneController.Instance.sceneID = 2;
					GameObject.Find ("LevelNum").GetComponent<Text> ().text = SceneController.Instance.sceneID.ToString ();
				}
				if (sceneName == "World") {
					SceneController.Instance.gameWon = true;
					SceneController.Instance.gameFinished = true;
					SceneController.Instance.loadScene5 = true;
					SceneController.Instance.sceneID = 3;
					GameObject.Find ("LevelNum").GetComponent<Text> ().text = SceneController.Instance.sceneID.ToString ();
				}
			}
		}
			
		// set the current waveCount
		waveNumber.text = waveCount.ToString();
	}

	/*
	 * Name: readTextFile
	 * Parameters: N/A
	 * Pre-Conditions: text file should be located in resoures file and contain enemytype and enemycount values
	 * Post-Conditions: array containing wave data will be read and returned
	 */
	string[] readTextFile(string fileName)
	{
		// get the text file from the resources folder
		TextAsset wavefile = (TextAsset)Resources.Load(fileName);

		char[] delimiterChars = { ' ', '\n' };

		// parse the data
		string[] values = wavefile.text.Split(delimiterChars);

		// return the string array
		return values;
	}

	/*
	 * Name: generateWave
	 * Parameters: int enemyType, int numEnemies
	 * Pre-Conditions: Level should have waypoints and starting point
	 * Post-Conditions: A random number of enemy units will be generated and navigate the waypoints
	 */
	IEnumerator generateWave(int enemyType, int numEnemies){
		// increment wave count
		waveCount++;

		// generate the random number of enemies
		for (int i = 0; i < numEnemies; i++) {

			if (enemyType == 1)
			{
				generateBee();
			}
			if (enemyType == 2)
			{
				generateCopter();
			}
			if (enemyType == 3)
			{
				generatePredator();
			}
			if (enemyType == 4)
			{
				generateX45();
			}

			//Set how fast they spawn. Smaller ones spawn faster
			enemyInterval = enemyType;
			// wait some amount of time before spawning another enemy
			yield return new WaitForSeconds (enemyInterval);			
		}

		// if the last wave has been generated, the last enemey has also been generated by this point. set the lastenemygenerated flag to true
		if (waveCount == NUMBERWAVES) {
			lastEnemyGenerated = true;
		}
	}

	/*
	 * Name: generateBee, Copter, Predator, X45
	 * Parameters: N/A
	 * Pre-Conditions: Level should have waypoints and starting point
	 * Post-Conditions: Enemy will be generated with default gameobject
	 */
	void generateBee(){
          Instantiate(enemyBee, spawnPoint.position, spawnPoint.rotation);
	}
     void generateCopter()
     {
          Instantiate(enemyCopter, spawnPoint.position, spawnPoint.rotation);
     }
     void generatePredator()
     {
          Instantiate(enemyPredator, spawnPoint.position, spawnPoint.rotation);
     }
     void generateX45()
     {
          Instantiate(enemyX45, spawnPoint.position, spawnPoint.rotation);
     }
}