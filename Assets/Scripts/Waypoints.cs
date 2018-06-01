using UnityEngine;

public class Waypoints : MonoBehaviour {
	// array contains the waypoints on the scene
	public static Transform[] turnPoints;

	void Awake () {
		// create the array that will be used store the waypoints
		turnPoints = new Transform[transform.childCount];

		// for loop will fill the turnPoints array
		for (int i = 0; i < turnPoints.Length; i++) {
			// store the waypoint
			turnPoints[i] = transform.GetChild (i);
		}
	}
}