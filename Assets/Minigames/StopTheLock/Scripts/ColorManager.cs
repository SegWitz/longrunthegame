
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/



using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AppAdvisory.StopTheLock
{
	public class ColorManager : MonoBehaviour
	{
		public Color[] colors;

		public Image m_background;
		public Image m_clock;
		public Image m_lock;

		Color color;

		public float timeChangeColor = 10;

		void OnEnable()
		{
			m_background.color = colors[0];
			UpdateCircleColor();
		}

		void OnDisable()
		{
			StopAllCoroutines();
		}

		public void ChangeColor()
		{
			Color colorTemp = colors[Random.Range(0, colors.Length)];

			StartCoroutine(DoLerp(m_background.color, colorTemp, 1f));
		}

		public IEnumerator DoLerp(Color from, Color to, float time)
		{
			float timer = 0;
			while (timer <= time)
			{
				timer += Time.deltaTime;
				m_background.color = Color.Lerp(from, to, timer / time);
				UpdateCircleColor();
				yield return null;
			}
			m_background.color = to;
			UpdateCircleColor();
		}

		void UpdateCircleColor()
		{
			Color c = m_background.color;

			Color temp = new Color(c.r / 2f, c.g / 2f, c.b / 2f, 1f);
			Color temp2 = new Color(c.r / 2f, c.g / 2f, c.b / 2f, 0.6f);

			m_clock.color = temp;
			m_lock.color = temp2;
		}
	}
}