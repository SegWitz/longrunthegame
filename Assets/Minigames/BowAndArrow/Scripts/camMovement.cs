using UnityEngine;

namespace BowAndArrow
{
	public class camMovement : MonoBehaviour
	{
		[SerializeField]
		GameObject arrow;

		public void setArrow(GameObject _arrow)
		{
			arrow = _arrow;
		}

		public void resetCamera()
		{
			transform.position = new Vector3(0, 0, -9.32f);
		}

		void Update()
		{
			if (arrow != null)
			{
				Vector3 position = transform.position;
				float z = position.z;
				position = Vector3.Lerp(transform.position, arrow.transform.position, Time.deltaTime);
				position.z = z;
				transform.position = position;
			}
		}
	}
}