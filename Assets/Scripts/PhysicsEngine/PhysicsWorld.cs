using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PhysicsWorld : MonoBehaviour
{

	public static PhysicsWorld Instance;

	[SerializeField]
	private bool CalculateCenterOfMass;

	[SerializeField]
	[Range(0, 80)]
	private float timeScale = 1;

	[SerializeField]
	private Vector3 gravity;
	public Vector3 Gravity { get => gravity; private set => gravity = value; }

	private const float G = 6674f;

	[SerializeField]
	private Vector3 centerOfMassPosition;

	// List of all Physic Objects
	private List<PhysicsObject> objects;

	public void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		objects = new List<PhysicsObject>();
	}

	public void Start()
	{
	}

	/// <summary>
	/// Start applying physics to PhysicsObject
	/// </summary>
	/// <param name="toAdd"></param>
	public void AddObject(PhysicsObject toAdd)
	{
		objects.Add(toAdd);
	}

	/// <summary>
	/// Stop applying physics to PhysicsObject
	/// </summary>
	/// <param name="toAdd"></param>
	public void RemoveObject(PhysicsObject toRemove)
	{
		if (toRemove == null) return;
		if (!objects.Contains(toRemove)) return;
		objects.Remove(toRemove);
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		Time.timeScale = timeScale;
		/*UpdateCenterOfMass();
		transform.position = centerOfMassPosition;
		*/
		Step();
		CollisionHandling();
	}

	private void Step()
	{
		float dt = Time.fixedDeltaTime;

		foreach (PhysicsObject phyObject in objects)
		{
			phyObject.Force += GetForceToApplyToPhysicsObject(phyObject);

			//Calculate Velocity with force and mass | dt -> delta Time  
			phyObject.Velocity += phyObject.Force / phyObject.Mass * dt;

			// Update Position wiht Velocity | dt -> delta Time  
			phyObject.Position += phyObject.Velocity * dt;

			StepRotation(phyObject);

			phyObject.Force = Vector3.zero;
		}
	}

	private void StepRotation(PhysicsObject phyObject)
	{
		//modifying the Vector3, based on input multiplied by speed and time
		phyObject.CurrentEulerAngles += phyObject.RotationSpeed * Time.deltaTime;

		//moving the value of the Vector3 into Quanternion.eulerAngle format
		Quaternion currentRotation = phyObject.CurrentRotation;
		currentRotation.eulerAngles = phyObject.CurrentEulerAngles;
		phyObject.CurrentRotation = currentRotation;

		//apply the Quaternion.eulerAngles change to the gameObject
		phyObject.transform.rotation = phyObject.CurrentRotation;
	}
	private void CollisionHandling()
	{
		List<PhysicsObject> objectsWithCollider = objects.Where(x => x.Collider != null).ToList();

		for (int i = 0; i < objectsWithCollider.Count; i++)
		{
			PhysicsObject obj1 = objectsWithCollider[i];
			for (int j = i + 1; j < objectsWithCollider.Count; j++)
			{
				PhysicsObject obj2 = objectsWithCollider[j];
				obj1.Collider.CheckCollision(obj2.Collider);
			}
		}
	}

	/// <summary>
	/// Calculats the center of mass based on all present physics Objects in this System
	/// </summary>
	private void UpdateCenterOfMass()
	{
		float totalMassInSystem = 0;
		Vector3 positionToDevide = Vector3.zero;

		foreach (PhysicsObject phyObject in objects)
		{
			positionToDevide += (phyObject.Position * phyObject.Mass);
			totalMassInSystem += phyObject.Mass;
		}

		centerOfMassPosition = positionToDevide / totalMassInSystem;
	}

	private Vector3 GetForceToApplyToPhysicsObject(PhysicsObject current)
	{
		Vector3 forceToApply = Vector3.zero;

		foreach (PhysicsObject phyObject in objects)
		{
			// Ignore self -> can't gravitate towards itself
			if (phyObject == current) continue;

			Vector3 directionToApply = phyObject.Position - current.Position;

			float distanceBeetweenObjects = directionToApply.magnitude;

			float forceMagnitude = G * (phyObject.Mass * current.Mass) / Mathf.Pow(distanceBeetweenObjects, 2);

			// Normalize to ignore distance
			forceToApply += directionToApply.normalized * forceMagnitude;
		}

		return forceToApply;
	}
}