using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// bool used to control camera movement
	private bool moveCam = true;

	// speed of panning
	public float panSpeed = 5f; 

	// set a board around the window that will be used to move the screen based on mouse position
	public float panBoardThickness = 20f;

	// mouse scroll speed
	public float scrollSpeed = 5f;
			
	// Update is called once per frame
	void Update () {

		// press esc to move or stop moving
		if (Input.GetKey("c")) {
			moveCam = !moveCam;
		}

		// if the user dones't want to move, stop moving
		if (!moveCam) {
			return;
		}

		// wasd keys used to move around the map
		if (Input.GetKey ("w") || Input.mousePosition.y >= Screen.height - panBoardThickness) {
			transform.Translate (Vector3.up * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey ("s") || Input.mousePosition.y <= panBoardThickness) {
			transform.Translate (Vector3.down * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey ("a") || Input.mousePosition.x <= panBoardThickness) {
			transform.Translate (Vector3.left * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey ("d") || Input.mousePosition.x >= Screen.width - panBoardThickness) {
			transform.Translate (Vector3.right * panSpeed * Time.deltaTime, Space.World);
		}

		// keys to zoom in and out of the map
		if (Input.GetKey ("z")) {
			Camera.main.orthographicSize += 0.1f;
		}
		if (Input.GetKey ("x")){
			Camera.main.orthographicSize -= 0.1f;
		}

		// set scroll 
		float scroll = Input.GetAxis ("Mouse ScrollWheel");

		// get the position of the camera
		Vector3 pos = transform.position;

		// set the orthographicsize to be changed on the mouse wheel
		Camera.main.orthographicSize -= scroll * 10 * scrollSpeed * Time.deltaTime;

		// set the limits of the orthographicsize
		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, 1f, 3f);

		// get the screen dimensions
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = Camera.main.orthographicSize*2;

		// set the scale to assist in defining the bounds the camera can move
		float scaleDim = 1-(Camera.main.orthographicSize- 1f)/(3f-1f);

		// make sure the camera is within the bounds
		pos.x = Mathf.Clamp(pos.x, -screenAspect*cameraHeight * scaleDim, screenAspect*cameraHeight * scaleDim);
		pos.y = Mathf.Clamp(pos.y, -cameraHeight * scaleDim, cameraHeight * scaleDim);

		// set the camera position
		transform.position = pos;
	}
}
