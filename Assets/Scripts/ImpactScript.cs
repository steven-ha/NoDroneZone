using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactScript : MonoBehaviour 
{

     Animator animator;
     AudioSource boomSound;
     float timer;
     float timer2;

     // Use this for initialization
     void Start()
     {
          animator = GetComponent<Animator>();
          boomSound = GetComponent<AudioSource>();
          Debug.Log("got em");
          Debug.Log("Heres the anim", animator);
     }

     // Update is called once per frame
     void Update()
     {
       //   animator.SetBool("impactBool", true);
          if (GameManager.Instance.impactCheck == true)
          {
               timer = Time.time;
               Debug.Log("its a hit");
               animator.SetTrigger("impactTrigger");
               boomSound.Play();

               //Now stop from looping
               GameManager.Instance.impactCheck = false;

  //             stopAnim();
 //              animator.ResetTrigger("impactTrigger");
          }
//          timer2 = Time.time;
//          if ((timer2-timer) > 1)
//          {
   //            animator.ResetTrigger("impactTrigger");
//          }
     }

//     IEnumerator stopAnim()
//     {
//          yield return new WaitForSeconds(2);
//          animator.ResetTrigger("impactTrigger");
//     }





 /*    void OnCollisionEnter(Collision col)
     {
         // if (col.gameObject.CompareTag("Enemy"))
          if (GameManager.Instance.impactCheck == true)
          {
       //        Debug.Log("its a hit");
     //          animator.SetTrigger("impactTrigger");
   //            new WaitForSeconds(1);
 //              animator.ResetTrigger("impactTrigger");
          }
     }
  * 
  * /




     /*
     Animator bAnim;

     void Start()
     {
          bAnim = this.GetComponent<Animator>();
          bool impactBool = true;
          
     }
   //  public bool impactBool = true;
     public int randomNum = 0;
     void Update()
     {
          //Check if impact
          //bool impactBool = false;
     //     impactBool = GameManager.Instance.impactCheck;
          randomNum = Random.Range(1, 200);
//          if (randomNum == 50)
  //        {
    //           impactBool = true;
      //    }
          //If true, animate impact


          //if true, boom noise
     }




  //   public GameObject boomObj;

   //  public void boomerFunc()
   //  {
   //       if (Enemy.impactBool == true)
  //        {

   //       }
 //    }

       //   impactA.SetBool("impactBool", true);

          //call boom sound
       //   AudioSource kaboom = GetComponent<AudioSource>();
      * 
      * 
      * */
}

