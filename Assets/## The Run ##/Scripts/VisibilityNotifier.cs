using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class VisibilityNotifier : MonoBehaviour
{
	public event Action<bool> BecameVisible;

	Renderer RendererComponent;

	void Awake()
	{
		RendererComponent = GetComponent<Renderer>();
	}

	void Start()
	{
		if (!RendererComponent.isVisible)
		{
			if (BecameVisible != null) BecameVisible(false);
		}
	}

	void OnBecameVisible()
	{
		if (BecameVisible != null) BecameVisible(true);
	}

	void OnBecameInvisible()
	{
		if (BecameVisible != null) BecameVisible(false);
	}
}