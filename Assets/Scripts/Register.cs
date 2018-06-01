using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Collections.Generic;

public class Register : MonoBehaviour {

	// Use this for initialization

	// path to php file to register a user
	public string dbPath = "http://people.oregonstate.edu/~has/RegisterUser.php";

	// Text input values for user data
	public GameObject firstName;
	public GameObject lastName;
	public GameObject email;
	public GameObject password;
	public GameObject confirmEmail;
	public GameObject confirmPassword;

	// string values of user data
	private string FirstName;
	private string LastName;
	private string Email;
	private string Password;
	private string ConfirmEmail;
	private string ConfirmPassword;

	// characters used to parse database info
	char[] delimiterChars = { '|' };

	// array used to store database info
	public string[] userData = new string[10];

	// flag used to denote an error during registering
	public bool registerError = false;

	// get instance of login 
	public Login myLogin;

	void Update(){

		//get login game object
		myLogin = (Login)FindObjectOfType(typeof(Login));

		// if the user is in the text fields for register, reset the registerError to false and clear the message
		if (email.GetComponent<InputField> ().isFocused == true || password.GetComponent<InputField> ().isFocused == true || firstName.GetComponent<InputField> ().isFocused == true || lastName.GetComponent<InputField> ().isFocused == true || confirmEmail.GetComponent<InputField> ().isFocused == true || confirmPassword.GetComponent<InputField> ().isFocused == true) {
			registerError = false;
			myLogin.loginError = false;
			GameObject.Find ("Message").GetComponent<Text> ().text = "";
		}

		// if user presses tab key, switch between firstname, lastname, email and password fields
		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (firstName.GetComponent<InputField> ().isFocused) {
				lastName.GetComponent<InputField> ().Select ();
			}
			else if(lastName.GetComponent<InputField> ().isFocused) {
				email.GetComponent<InputField> ().Select ();
			}
			else if(email.GetComponent<InputField> ().isFocused) {
				confirmEmail.GetComponent<InputField> ().Select ();
			}
			else if(confirmEmail.GetComponent<InputField> ().isFocused) {
				password.GetComponent<InputField> ().Select ();
			}
			else if(password.GetComponent<InputField> ().isFocused) {
				confirmPassword.GetComponent<InputField> ().Select ();
			}
		}
			
		// if the register information does exist in the database, warn the user the email address already exists
		if (registerError == true) {
			GameObject.Find ("Message").GetComponent<Text> ().text = "Email address is already registered!";
			GameObject.Find ("Message").GetComponent<Text> ().color = Color.red;
		}

		// set the FirstName, LastName, Email and Password strings from user entered field data
		FirstName = firstName.GetComponent<InputField> ().text;
		LastName = lastName.GetComponent<InputField> ().text;
		Email = email.GetComponent<InputField> ().text;
		Password = password.GetComponent<InputField> ().text;
		ConfirmEmail = confirmEmail.GetComponent<InputField> ().text;
		ConfirmPassword = confirmPassword.GetComponent<InputField> ().text;

	}

	/*
	 * Name: RegisterButton
	 * Parameters: N/A
	 * Pre-Conditions: Scene should have a register button with fields for register data
	 * Post-Conditions: Coroutine to register user will be executed or message will be printed to console saying check input
	 */
	public void RegisterButton(){
		// reset message and error flags for login
		myLogin.loginError = false;
		GameObject.Find ("Message").GetComponent<Text> ().text = "";

		// user data should not be blank or default values
		if (FirstName != "" && LastName != "" && FirstName != "First Name" && LastName != "Last Name" && Email != "" && Password != "" && Email != "Email" && Password != "Password" && ConfirmEmail != "" && ConfirmPassword != "" && ConfirmEmail != "Confirm Email" && ConfirmPassword != "Confirm Password") {
			// email and confirm email should match
			if (Email.CompareTo (ConfirmEmail) == 0) {
				// password and confirm password should match
				if (Password.CompareTo (ConfirmPassword) == 0) {
					StartCoroutine (InsertToDB (FirstName, LastName, Email, Password));
				} else {
					Debug.Log ("Passwords do not match.");
					GameObject.Find ("Message").GetComponent<Text> ().text = "Passwords do not match!";
					GameObject.Find ("Message").GetComponent<Text> ().color = Color.red;
				}
			} else {
				Debug.Log ("Emails do not match.");
				GameObject.Find ("Message").GetComponent<Text> ().text = "Emails do not match!";
				GameObject.Find ("Message").GetComponent<Text> ().color = Color.red;
			}
		} else {
			Debug.Log ("Check Registration Input");
			GameObject.Find ("Message").GetComponent<Text> ().text = "Check registration input!";
			GameObject.Find ("Message").GetComponent<Text> ().color = Color.red;
		}
	}

	/*
	 * Name: InsertToDB
	 * Parameters: N/A
	 * Pre-Conditions: Database user table should exist before calling this method
	 * Post-Conditions: Server will send back message stating "User account created successfully" or "User account already exists."
	 */
	IEnumerator	InsertToDB(string firstName, string lastName, string email, string password){

		// create new web form
		WWWForm form = new WWWForm ();

		// add firstname, lastname, email and password fields
		form.AddField ("registerFirstName", firstName);
		form.AddField ("registerLastName", lastName);
		form.AddField ("registerEmail", email);
		form.AddField ("registerPassword", password);

		// access database
		WWW www = new WWW (dbPath, form);
		yield return www;

		// print out server message
		//Debug.Log (www.text);

		// Check if the account was successfully added to the database, set up game data if true
		if (((www.text).CompareTo ("User account already exists.") == 0)) {
			registerError = true; // incorrect login information
		} 
		else{
			userData = (www.text).Split (delimiterChars);

			// parse the server data
			/*
			for (int i = 0; i < userData.Length; i++) {
				Debug.Log (userData [i]);
			}
			*/

			// set up userID, money, towers
			SceneController.Instance.userID = Convert.ToInt32 (userData [0]);
			SceneController.Instance.money = Convert.ToInt32 (userData [3]);
			SceneController.Instance.shotgun = Convert.ToInt32 (userData [5]);
			SceneController.Instance.slingshot = Convert.ToInt32 (userData [6]);
			SceneController.Instance.watergun = Convert.ToInt32 (userData [7]);
			SceneController.Instance.sceneID = Convert.ToInt32 (userData [4]);
			SceneController.Instance.lives = Convert.ToInt32 (userData [8]);
			SceneController.Instance.hitpoints = Convert.ToInt32 (userData [9]);

			// start the user at level1
			SceneController.Instance.loadScene2 = true;
	
		}

	}
}
