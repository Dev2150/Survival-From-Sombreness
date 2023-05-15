using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector2 mouseLook; // how much movement the mouse has made in the current frame, is going to change the camera & player
    Vector2 smoothV; // smooth down the movement

    public float sensitivity = 20.0f; // how less to move the mouse to create camera rotation
    public float smoothing = 5.0f;
	float gameStartTime = 0.0f;

    GameObject o_parent;

	Vector3 finalPos = new Vector3 (0f, 0.8f, -0.25f), perFrame;
	Transform this_transform;
	float speed = 2f;

	// Use this for initialization
	void Start () {
        o_parent = transform.parent.gameObject; // character = the camera's parent
		this_transform = transform;
		perFrame = -(this_transform.position - finalPos) / speed;
		gameStartTime = TimeSystem.time;
	}

	void LateUpdate() {
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        //Debug.Log(mouseDelta);
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

        smoothV = new Vector2(Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing),
                              Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing));
        mouseLook += smoothV;

        transform.localRotation = Quaternion.AngleAxis(-5 + -mouseLook.y, Vector3.right);

        o_parent.transform.localRotation = Quaternion.AngleAxis(75 + mouseLook.x, o_parent.transform.up);


    }
}
