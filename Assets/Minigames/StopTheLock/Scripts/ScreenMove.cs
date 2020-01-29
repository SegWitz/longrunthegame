
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/


using UnityEngine;
using System.Collections;

namespace AppAdvisory.StopTheLock
{
	public class ScreenMove : MonoBehaviourHelper
	{
		public bool isMoving = false;

		public IEnumerator Move(RectTransform t, bool startAnim)
		{
			isMoving = true;

			float p0;
			float p1;

			if (startAnim)
			{
				p0 = (gameManager.rectTransform.sizeDelta.x * 0.5f) + 210.0f;
				p1 = 0;
			}
			else
			{
				p0 = 0;
				p1 = (-gameManager.rectTransform.sizeDelta.x * 0.5f) - 210.0f;
			}

			t.anchoredPosition = new Vector2(p0, t.anchoredPosition.y);

			float timer = 0;

			float time = 0.3f;

			while (timer <= time)
			{
				timer += Time.deltaTime;

				float f = Mathf.Lerp(p0, p1, timer / time);

				t.anchoredPosition = new Vector2(f, t.anchoredPosition.y);

				yield return 0;
			}

			yield return new WaitForSeconds(0.1f);

			isMoving = false;
		}
	}
}