using UnityEngine;

[CreateAssetMenu(fileName = "Quotes Data", menuName = "Quotes Data")]
public class QuotesData : ScriptableObject
{
	[SerializeField]
	Quote[] quotes = null;

	public Quote[] Quotes { get { return quotes; } }
}

[System.Serializable]
public class Quote
{
	public string AuthorName;
	[TextArea(1, 4)]
	public string QuoteText;
}