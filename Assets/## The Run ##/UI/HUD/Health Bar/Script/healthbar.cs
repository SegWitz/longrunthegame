using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
	private float fillAmount;

	[SerializeField]
	private float lerpSpeed;

	[SerializeField]
	private Image content;

	[SerializeField]
	private Text valueText;

	[SerializeField]
	private Color fullColor;

	[SerializeField]
	private Color lowColor;

	[SerializeField]
	private bool lerpColors;

	public float MaxValue { get; set; }

	public float Value
	{
		set
		{
			valueText.text = value + "%"; ;
			fillAmount = Map(value, 0, MaxValue, 0, 1);
		}
	}

	void Start()
	{
		if (lerpColors)
		{
			content.color = fullColor;
		}
	}

	void Update()
	{
		Handlebar();
	}

	private void Handlebar()
	{
		if (fillAmount != content.fillAmount)
		{
			content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
		}

		if (lerpColors)
		{
			content.color = Color.Lerp(lowColor, fullColor, fillAmount);
			valueText.color = Color.Lerp(lowColor, fullColor, fillAmount);
		}
	}

	private float Map(float value, float inMin, float inMax, float outMin, float outMax)
	{
		return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}