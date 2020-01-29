using System;
using System.Globalization;
using UnityEngine;

public static class Globals
{
	public static string GetFormattedCurrency(long Value, bool AddJGD)
	{
		if (AddJGD)
			return Value.ToString("N0") + " JGD";
		else
			return Value.ToString("N0");
	}

	public static long AddLongChecked(long Value1, long Value2)
	{
		long Result;
		try
		{
			Result = checked(Value1 + Value2);
		}
		catch (System.OverflowException)
		{
			Result = long.MaxValue;
		}

		return Result;
	}

	public static long SubtractLongChecked(long Value1, long Value2)
	{
		long Result;
		try
		{
			Result = checked(Value1 - Value2);
		}
		catch (System.OverflowException)
		{
			Result = long.MinValue;
		}

		return Result;
	}

	public static ulong AddULongChecked(ulong Value1, ulong Value2)
	{
		ulong Result;
		try
		{
			Result = checked(Value1 + Value2);
		}
		catch (System.OverflowException)
		{
			Result = ulong.MaxValue;
		}

		return Result;
	}

	public static string DateTimeToString(DateTime dateTime)
	{
		return dateTime.ToUniversalTime().ToString("s", new CultureInfo("en-US"));
	}

	public static DateTime StringToDateTime(string dateTimeString)
	{
		return DateTime.Parse(dateTimeString, new CultureInfo("en-US")).ToLocalTime();
	}

	public static void ExitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}