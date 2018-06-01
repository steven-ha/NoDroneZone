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

public class PauseMenu : MonoBehaviour {

     //Create UI Object
     public GameObject PauseUI;

     //Whether game is paused or not. Starts as false
     private bool pausedState = false;

     //Call this when the game starts
     void Start()
     {
          //Not showing the pause menu
          PauseUI.SetActive(false);
     }

     void Update()
     {
          if(Input.GetButtonDown("Pause"))
          {
               //flip the value of the bool
               pausedState = !pausedState;
          }

          if(pausedState)
          {
               //Show the pause menu
               PauseUI.SetActive(true);
               //Stop/pause the game
               Time.timeScale = 0;
          }
          if(!pausedState)
          {
               //No pause menu
               PauseUI.SetActive(false);
               //Game running at normal speed
               Time.timeScale = 1;
          }
     }

     //NOw make functions for OnClick for all buttons

     public void resumeFunc()
     {
          pausedState = false;
     }

     public void saveFunc()
     {
          //save. Defined below
          SaveButton();
     }

     public void savequitFunc()
     {
          //save
          SaveButton();
          //quit back to lgin screen
          SceneController.Instance.loadScene1 = true;
          SceneController.Instance.Update();
          //Close the pause menu
          pausedState = false;
     }

     public void quitFunc()
     {
          //quit back to lgin screen
          SceneController.Instance.loadScene1 = true;
          SceneController.Instance.Update();
          //Close the pause menu
          pausedState = false;

     }



     // Save stuff



     /*
      * Name: SaveButton
      * Parameters: N/A
      * Pre-Conditions: Scene should have a pause screen that has a save button to execute the save
      * Post-Conditions: Coroutine to save user game data to the database will be executed 
      */
     public void SaveButton()
     {
          StartCoroutine(
          SaveToDB(
          //SceneController.Instance.userID, SceneController.Instance.money, SceneController.Instance.sceneID, SceneController.Instance.pShotgun, SceneController.Instance.pSlingshot, SceneController.Instance.pWatergun, SceneController.Instance.lives, SceneController.Instance.hitpoints));
		SceneController.Instance.userID, SceneController.Instance.pMoney, SceneController.Instance.sceneID, SceneController.Instance.pShotgun, SceneController.Instance.pSlingshot, SceneController.Instance.pWatergun, SceneController.Instance.pLives, SceneController.Instance.pHitpoints));

     }


     public string dbPath = "http://people.oregonstate.edu/~has/SaveUser.php";

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
          Debug.Log(www.text);

     }


}
