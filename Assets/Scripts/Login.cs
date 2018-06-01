using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Collections.Generic;

public class Login : MonoBehaviour {

	// Use this for initialization

	// path to php file to log into game
	public string dbPath = "http://people.oregonstate.edu/~has/GetUser.php";

	// Text Input for email
	public GameObject email;

	// Text Input for password
	public GameObject password;

	public bool loginError = false;

	// string values for email and password
	private string Email;
	private string Password;

	// characters used to parse string
	char[] delimiterChars = { '|' };

	// store used data from database
	public string[] userData = new string[10];

	// register gameObject
	public Register myRegister;
			
	void Update(){

		//get register game object
		myRegister = (Register)FindObjectOfType(typeof(Register));

		// if the user is in the email or password text fields for login, reset the loginError to false and clear the message
		if (email.GetComponent<InputField> ().isFocused == true || password.GetComponent<InputField> ().isFocused == true) {
			myRegister.registerError = false;
			GameObject.Find ("Message").GetComponent<Text> ().text = "";			
			loginError = false;
		}

		// if user presses tab key, switch between email and password fields
		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (email.GetComponent<InputField> ().isFocused) {
				password.GetComponent<InputField> ().Select ();
			}
		}
			
		// if the login information does not exist in the database, warn the user to check their login information
		if (loginError == true) {
			GameObject.Find ("Message").GetComponent<Text> ().text = "Check login information!";
			GameObject.Find ("Message").GetComponent<Text> ().color = Color.red;
		}

		// set the Email and Password strings from user entered field data
		Email = email.GetComponent<InputField> ().text;
		Password = password.GetComponent<InputField> ().text;
	}
		
	/*
	 * Name: LoginButton
	 * Parameters: N/A
	 * Pre-Conditions: Scene should have a login button with fields for login data
	 * Post-Conditions: Coroutine to log into the database will be executed or message will be printed to console saying check input
	 */
	public void LoginButton(){
		// reset register error flag/message
		myRegister.registerError = false;
		loginError = false;
		GameObject.Find ("Message").GetComponent<Text> ().text = "";

		// check if user input is valid to start login coroutine
		if (Email != "" && Password != "" && Email != "Email" && Password != "Password") {
			StartCoroutine (LoginToDB (Email, Password));
		} else {
			GameObject.Find ("Message").GetComponent<Text> ().text = "Check login information!";
		}
	}

	/*
	 * Name: LoginToDB
	 * Parameters: N/A
	 * Pre-Conditions: Database user table should exist before calling this method
	 * Post-Conditions: Server will send back message stating "Login successful." or "Login information is invalid."
	 */
	IEnumerator	LoginToDB(string email, string password){

		// create new web form
		WWWForm form = new WWWForm ();

		// add email and password fields
		form.AddField ("loginEmail", email);
		form.AddField ("loginPassword", password);

		// access database
		WWW www = new WWW (dbPath, form);
		yield return www;

		// print out server message
		//Debug.Log (www.text);

		// check the response from the server, set up the game stats only if the user successfully logged in
		if ((www.text).CompareTo ("Login information is invalid.") == 0) {
			loginError = true; // incorrect login information
		} else {
			// change scenes
			// get/split the data from the server
			userData = (www.text).Split(delimiterChars);

			// print out the server data
			/*
			for (int i = 0; i < userData.Length; i++) {
				Debug.Log (userData [i]);
			}
			*/

			// set up userID, money, towers, etc
			SceneController.Instance.userID = Convert.ToInt32 (userData[0]);
			SceneController.Instance.money = Convert.ToInt32 (userData[3]);
			SceneController.Instance.shotgun = Convert.ToInt32 (userData[5]);
			SceneController.Instance.slingshot = Convert.ToInt32 (userData[6]);
			SceneController.Instance.watergun = Convert.ToInt32 (userData[7]);
			SceneController.Instance.sceneID = Convert.ToInt32 (userData [4]);
			SceneController.Instance.lives = Convert.ToInt32 (userData [8]);
			SceneController.Instance.hitpoints = Convert.ToInt32 (userData [9]);
			SceneController.Instance.fName = userData [1];
			SceneController.Instance.lName = userData [2];

			// check what scene the user has saved on the server
			// level 1
			if (Convert.ToInt32 (userData [4]) == 1) {
				SceneController.Instance.loadScene2 = true;
			}

			//level 2
			if (Convert.ToInt32 (userData [4]) == 2) {
				SceneController.Instance.loadScene3 = true;
			}

			//level 3
			if (Convert.ToInt32 (userData [4]) == 3) {
				SceneController.Instance.loadScene4 = true;
			}

		}
	}

}
