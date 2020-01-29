using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class PolygonPainter : MonoBehaviour
{

	public delegate void PolygonPainterDelegate(PolygonPainterEventArgs e);

	/// <summary>
	/// Gets fired when a polygon paint is finished
	/// </summary>
	public event PolygonPainterDelegate PolygonPainted;

	/// <summary>
	/// the "detail" of the line.
	/// the distance between single line parts.
	/// </summary>
	public float dotDistance = 1f;

	/// <summary>
	/// The max number of paintable points.
	/// </summary>
	public int maxPaintablePoints = 5000;

	/// <summary>
	/// true if the player is painting at the moment.
	/// </summary>
	private bool isPainting;

	/// <summary>
	/// while painting the position of the last drawn point of the line.
	/// </summary>
	private Vector2 lastPointPosition;

	/// <summary>
	/// The line renderer.
	/// </summary>
	private LineRenderer lineRenderer;

	/// <summary>
	/// The current point count of the line.
	/// </summary>
	private int pointCount;

	/// <summary>
	/// The point positions of the line.
	/// </summary>
	private Vector3[] points;

	Camera CameraReference;

	Vector3 CurrentMouseWorldPosition;

	void OnEnable()
	{
		isPainting = false;
		lineRenderer = GetComponent<LineRenderer>();

		CameraReference = Camera.main;

		pointCount = 0;
		points = new Vector3[maxPaintablePoints];
	}

	/// <summary>
	/// Update the painter
	/// </summary>
	void Update()
	{
		CurrentMouseWorldPosition = Get2DMousePosition();

		if (Input.GetMouseButtonDown(0))
		{
			if (!isPainting)
			{
				InitPaintMode();
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			ClosePolygon();

			if (pointCount >= 3)
			{
				if (PolygonPainted != null)
				{
					PolygonPainted(new PolygonPainterEventArgs(points, pointCount));
				}
			}

			Reset();
		}

		if (CanPaint())
		{
			Paint();
		}
	}

	/// <summary>
	/// Reset all parameters.
	/// </summary>
	void Reset()
	{
		pointCount = 0;
		lineRenderer.positionCount = pointCount;
		isPainting = false;
	}

	/// <summary>
	/// check if the max point count is reached.
	/// </summary>
	/// <returns><c>true</c> if this instance can paint; otherwise, <c>false</c>.</returns>
	bool CanPaint()
	{
		return pointCount < maxPaintablePoints - 1;
	}

	/// <summary>
	/// Initialize the paint mode
	/// </summary>
	void InitPaintMode()
	{
		pointCount = 1;
		points[pointCount - 1] = CurrentMouseWorldPosition;
		lineRenderer.positionCount = pointCount;
		lineRenderer.SetPosition(0, CurrentMouseWorldPosition);

		lastPointPosition = CurrentMouseWorldPosition;
		isPainting = true;
	}

	/// <summary>
	/// Add new points to the line
	/// </summary>
	void Paint()
	{
		if (!isPainting)
		{
			return;
		}

		if (Vector2.Distance(CurrentMouseWorldPosition, lastPointPosition) >= dotDistance)
		{
			pointCount++;
			points[pointCount - 1] = CurrentMouseWorldPosition;
			lineRenderer.positionCount = pointCount;
			lineRenderer.SetPosition(pointCount - 1, CurrentMouseWorldPosition);

			lastPointPosition = CurrentMouseWorldPosition;
		}
	}

	/// <summary>
	/// Closes the polygon.
	/// </summary>
	void ClosePolygon()
	{
		if (pointCount >= 2)
		{
			pointCount++;
			points[pointCount - 1] = points[0];
			lineRenderer.positionCount = pointCount;
			lineRenderer.SetPosition(pointCount - 1, points[0]);

			lastPointPosition = CurrentMouseWorldPosition;
		}
	}

	/// <summary>
	/// Get the 2D position of the mouse or touch
	/// </summary>
	/// <returns>The D mouse position.</returns>
	Vector3 Get2DMousePosition()
	{
		var result = CameraReference.ScreenToWorldPoint(Input.mousePosition);
		result.z = 0f;
		return result;
	}
}