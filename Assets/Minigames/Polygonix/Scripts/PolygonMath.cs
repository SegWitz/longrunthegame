using UnityEngine;

public static class PolygonMath
{

	/// <summary>
	/// Returns true, if the position is intersecting the points of the polygon
	/// </summary>
	/// <param name="points">Points.</param>
	/// <param name="pointCount">Point count.</param>
	/// <param name="position">Position.</param>
	public static bool Intersect(Vector3[] points, int pointCount, Vector3 position)
	{
		Vector3 p1, p2;
		bool intersect = false;

		if (pointCount < 3)
		{
			return intersect;
		}

		Vector3 oldPoint = points[pointCount - 1];

		for (int i = 0; i < pointCount; i++)
		{
			Vector3 newPoint = points[i];

			if (newPoint.x > oldPoint.x)
			{
				p1 = oldPoint;
				p2 = newPoint;
			}
			else
			{
				p1 = newPoint;
				p2 = oldPoint;
			}

			if ((newPoint.x < position.x) == (position.x <= oldPoint.x) &&
			   ((long)position.y - (long)p1.y) * (long)(p2.x - p1.x) <
				((long)p2.y - (long)p1.y) * (long)(position.x - p1.x))
			{
				intersect = !intersect;
			}

			oldPoint = newPoint;
		}

		return intersect;
	}

	public static bool IntersectNew(Vector3[] points, int pointCount, Vector3 position)
	{
		var j = pointCount - 1;
		bool IsColliding = false;

		for (int i = 0; i < pointCount; j = i++)
		{
			//This if prevents division by zero
			if ((points[j].y - points[i].y) != 0)
			{
				if (((points[i].y > position.y) != (points[j].y > position.y)) && (position.x < (points[j].x - points[i].x) * (position.y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
					IsColliding = !IsColliding;
			}
		}

		return IsColliding;
	}
}