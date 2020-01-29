using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class DebugShadedCollider : MonoBehaviour
{
	Collider colliderComponent;

	void OnEnable()
	{
		colliderComponent = GetComponent<Collider>();
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		Gizmos.matrix = transform.localToWorldMatrix;

		if (colliderComponent is BoxCollider)
		{
			BoxCollider box = (BoxCollider)colliderComponent;
			Gizmos.DrawCube(box.center, box.size);
		}
		else if (colliderComponent is SphereCollider)
		{
			SphereCollider sphere = (SphereCollider)colliderComponent;
			Gizmos.DrawSphere(sphere.center, sphere.radius);
		}

		Gizmos.matrix = Matrix4x4.identity;
	}
}