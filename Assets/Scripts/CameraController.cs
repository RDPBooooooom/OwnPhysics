using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	[SerializeField]
	private float speed;

	[SerializeField]
	private float sensitivity;

	private Vector3 lastMouse;
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		Movement();
		LookAround();
	}

	private void LookAround() {
		lastMouse = Input.mousePosition - lastMouse;
		lastMouse = new Vector3(-lastMouse.y * sensitivity, lastMouse.x * sensitivity, 0);
		lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
		transform.eulerAngles = lastMouse;
		lastMouse = Input.mousePosition;
	}


	private void Movement() {
		if (Input.GetKey(KeyCode.W)) {
			transform.position += GetMoveDistance(transform.forward);
		}
		if (Input.GetKey(KeyCode.S))
		{
			transform.position += GetMoveDistance(-transform.forward);
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.position += GetMoveDistance(-transform.right);
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.position += GetMoveDistance(transform.right);
		}
	}

	private Vector3 GetMoveDistance(Vector3 direction) { 
		return direction * speed * Time.deltaTime;
	}

}
