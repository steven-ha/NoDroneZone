using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    //Type of drone
    [SerializeField]
    private string droneType;

     //Health points. Amount depends on the type of drone
    public float health;

	// controls the speed of the enemy. Varies depending on type of drone
	private float speed = 1.0f;

	// the waypoint the enemy is moving towards
	private Transform target;

	// index used to identify the current waypoint
	private int waypointIndex = 0;

    // indicates whether enemy is alive
    public bool isAlive { get; set; }

     //Use this for the boom sound
    public AudioSource smackSource;
    public AudioClip smackClip;

    // Value of monster when killed 
    //[SerializeField]
    private int value;

    private Text enemyData;
    Animator myAnimator;

 //   bool checker;





    //   public AudioClip boomSound = (AudioClip)Resources.Load("Boom.mp3");

    void Awake()
    {
        // load enemy data from tower file
        TextAsset enemyData = Resources.Load(droneType) as TextAsset;
        string[] values = enemyData.text.Split(' ');
        this.health = float.Parse(values[0]);
        this.speed = float.Parse(values[1]);
        this.value = int.Parse(values[2]);        
    }

    // Use this for initialization
    void Start () {
		// set the target to the first waypoint on the path
		target = Waypoints.turnPoints [0];
        isAlive = true;
        smackSource = GetComponent<AudioSource>();
        smackClip = smackSource.clip;
    //     boomSound = (AudioClip)Resources.Load("Boom.mp3");

         //get the boom animation
//        Debug.Log("getting...");
//        myAnimator = GetComponent<Animator>();
//        Debug.Log("Heres the anim", myAnimator);
//         if (myAnimator)
//        {
//              Debug.Log("anim aquired");
//         }

   //     checker = false;
	}
	
	// Update is called once per frame
	void Update () {

          //If Health <= 0, destroy
          if (health <= 0)
          {
                //Do some death animation


               //Smack noise
             //  smack.Play();
               AudioSource.PlayClipAtPoint(smackClip, transform.position);




    //           Destroy(gameObject, 1);
               
                //Then destroy object
                Destroy(gameObject);
                isAlive = false;

                // add money for kill
                GameManager.Instance.Money += this.value;
                
          }


		// set the vector the enemy will be moving along
		Vector3 dir = target.position - transform.position;

		// move the enemy
		transform.Translate (dir.normalized * speed * Time.deltaTime, Space.World);

          //define the angle
          float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;

          //Rotate so drones "look" the correct way, smoothly
          Quaternion smoothRotate = Quaternion.AngleAxis(angle, Vector3.forward);
          transform.rotation = Quaternion.Lerp(transform.rotation, smoothRotate, 1);
          
		// update the waypoint when the enemy gets within 0.05 of the target
		if (Vector3.Distance (transform.position, target.position) <= 0.05f) {
			GetNextWaypoint ();
		}

      //    if (myAnimator)
    //      {
          //     myAnimator.SetBool("impactBool", true);
  //             myAnimator.SetTrigger("impactTrigger");
//               Debug.Log("Hello in trigger");
//          }
        //  Debug.Log("just updatin");
	}


	/*
	 * Name: GetNextWaypoint
	 * Parameters: N/A
	 * Pre-Conditions: Scene needs to contain defined waypoints
	 * Post-Conditions: wavepoint index will increment by 1 until the last waypoint is reached
	 * 					the target will be updated to this waypoint
	 * 					if the last waypoint is reached, the gameObject will be destroyed
	 */

    // public AudioClip boomSound = Resources.Load(Application.dataPath.Assets.Audio.Boom);

  //   public AudioClip boomSound = (AudioClip)Resources.Load("Boom.mp3");

	void GetNextWaypoint(){
		// if the waypointIndex greater than or equal to the last waypoint, destroy the enemy 
		if (waypointIndex >= (Waypoints.turnPoints.Length - 1)) {

            // reduce player health by 1
            //GameManager.Instance.Hp -= this.health
            GameManager.Instance.Hp--;

            //Play Boom sound
            //           Vector3 nullpos;
            //               nullpos = new Vector3(0, 0, 0);
            //          AudioSource.PlayClipAtPoint(boomSound, nullpos, 1.0f);
               
               //impactbool = true for use in impact script
           // impactBool = true;
            GameManager.Instance.impactCheck = true;
//            ImpactScript.boomerFunc();

            //deactive the drone
            gameObject.SetActive(false);

            AudioSource audio = GetComponent<AudioSource>();

            //        audio.Play();  // plays sound when collided

            //Set impact bool to be true 
            //           checker = true;

            // destroy the game object
            this.isAlive = false;
            Destroy(gameObject); 
			return;
		}

		// increment waypointIndex
		waypointIndex++;

		// set the target to the current waypointIndex
		target = Waypoints.turnPoints [waypointIndex];
	}
}