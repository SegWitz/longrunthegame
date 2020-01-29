using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBehaviour : MonoBehaviour 
{
	private const float height = 0.9f;
	private const float y = 0.4f;
	private CapsuleCollider col;
	private Vector3 originalsize;
	private float heightOriginal;

	public void StartSlide()
	{
		if (col == null) 
		{
			col = GetComponent<CapsuleCollider> ();
		}

		originalsize = col.center;
		heightOriginal = col.height;

		Vector3 temp = originalsize;
		temp.y = y;

		col.center = temp;
		col.height = height;
	}

	public void EndSlide()
	{
		col.center = originalsize;
		col.height = heightOriginal;
	}

}
