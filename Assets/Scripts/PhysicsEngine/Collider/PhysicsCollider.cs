using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsCollider : MonoBehaviour
{
	public event EventHandler Collided;

	private void Start()
	{
		Collided += SolveCollision;
	}

	public abstract void CheckCollision(PhysicsSphereCollider collider);

	public virtual void CheckCollision(PhysicsCollider collider)
	{

		if (typeof(PhysicsSphereCollider) == collider.GetType())
		{
			CheckCollision((PhysicsSphereCollider)collider);
		}
	}

	protected abstract void SolveCollision(object colliededWith, EventArgs data);

	protected virtual void OnCollision(PhysicsCollider collidedWith)
	{
		EventHandler handler = Collided;
		handler?.Invoke(collidedWith, EventArgs.Empty);
	}
}
