using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
	public Vector3 Position { get => transform.position; set => transform.position = value; }

	#region Physics relevant
	[SerializeField]
	private Vector3 velocity;
	public Vector3 Velocity { get => velocity; set => velocity = value; }

	private Vector3 force;
	public Vector3 Force { get => force; set => force = value; }

	[SerializeField]
	private float mass;
	public float Mass { get => mass; private set => mass = value; }
	#endregion

	#region Rotation
	[SerializeField]
	private Vector3 rotationSpeed;
	public Vector3 RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }

	private Vector3 currentEulerAngles;
	public Vector3 CurrentEulerAngles { get => currentEulerAngles; set => currentEulerAngles = value; }

	public Quaternion CurrentRotation { get => transform.rotation; set => transform.rotation = value; }
	#endregion

	private PhysicsCollider physicsCollider;
	public PhysicsCollider Collider { get => physicsCollider; set => physicsCollider = value; }

	public void Start()
	{
		Force = Vector3.zero;

		PhysicsWorld.Instance.AddObject(this);

		currentEulerAngles = CurrentRotation.eulerAngles;
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		PhysicsWorld.Instance.RemoveObject(this);
	}
}
