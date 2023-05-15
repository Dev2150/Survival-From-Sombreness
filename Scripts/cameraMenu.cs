using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMenu : MonoBehaviour {

	Vector2 mouseLook; // how much movement the mouse has made in the current frame, is going to change the camera & player
	Vector2 smoothV; // smooth down the movement

	public float sensitivity = 2.5f; // how less to move the mouse to create camera rotation
	public float smoothing = 2.0f;

	GameObject character;

	Vector2 mouseDelta, b = new Vector2(-0.005f, 0);
		
	void Start () {
		character = transform.gameObject; // character = the camera's parent
		// dis = this.transform;
	}

	void FixedUpdate() {
		
		mouseDelta = b;

		mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

		smoothV = new Vector2(Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing),
			Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing));
		mouseLook += smoothV;

		transform.localRotation = Quaternion.AngleAxis(-5 + -mouseLook.y, Vector3.right);

		character.transform.localRotation = Quaternion.AngleAxis(60 + mouseLook.x, character.transform.up);

	}
}
