using System;
using UnityEngine;

public class PhysicsSphereCollider : PhysicsCollider
{
	[SerializeField]
	private float radius;
	public float Radius { get => radius; set => radius = value; }

	void Start()
	{
		PhysicsObject phyObject = gameObject.GetComponent<PhysicsObject>();

		if (phyObject == null)
		{
			Destroy(this);
		}

		phyObject.Collider = this;
	}

	/// <summary>
	/// Check if a Collison between this and the given collider has happened
	/// </summary>
	/// <param name="collider">Collider to check with</param>
	public override void CheckCollision(PhysicsSphereCollider collider)
	{
		float distanceBetweenObjects = (transform.position - collider.transform.position).magnitude;
		float radiusSum = Radius + collider.Radius;

		if (distanceBetweenObjects < radiusSum)
		{
			OnCollision(collider);
		}
	}

	/// <summary>
	/// Solve a collision between this and the given collider
	/// </summary>
	/// <param name="collider">Colliding Object</param>
	protected override void SolveCollision(object collidedWith, EventArgs data)
	{
		print("Solving");
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position, radius);
	}
}

