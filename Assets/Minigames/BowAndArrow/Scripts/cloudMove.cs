using UnityEngine;

namespace BowAndArrow
{
	public class cloudMove : MonoBehaviour
	{
		[SerializeField]
		float speed;

		void Update()
		{
			Vector3 position = transform.position;
			position.x += speed * Time.deltaTime;
			if (position.x > 12f) position.x = -12f;
			transform.position = position;
		}
	}
}