using System.IO;
using UnityEngine;

public class TheRunMobileGameData : TheRunGameData
{
	string GameDataPath = Path.Combine(Application.persistentDataPath, "SaveData.json"); //TODO: Rename to *.bin

	protected override bool CheckGameData()
	{
        if (Application.isEditor)
            Debug.Log("The name of the path where data will be stored : " + GameDataPath);
		return File.Exists(GameDataPath);
	}

	protected override void CreateDefaultGameData()
	{
		//TODO: Re-enable binary serialization. Using JSON for testing purposes.
		//using (FileStream fs = new FileStream(GameDataPath, FileMode.Create, FileAccess.Write))
		//{
		//	var Formatter = new BinaryFormatter();
		//	Data = new GameData();
		//	Formatter.Serialize(fs, Data);
		//}

		_Data = new GameData();
		File.WriteAllText(GameDataPath, JsonUtility.ToJson(Data, true), System.Text.Encoding.UTF8);

		Debug.Log("Game Data created at: " + GameDataPath);
	}

	protected override void LoadGameData()
	{
		//TODO: Re-enable binary serialization. Using JSON for testing purposes.
		//using (FileStream fs = new FileStream(GameDataPath, FileMode.Open, FileAccess.Read))
		//{
		//	BinaryFormatter Formatter = new BinaryFormatter();
		//	Data = (GameData)Formatter.Deserialize(fs);
		//}

		_Data = JsonUtility.FromJson<GameData>(File.ReadAllText(GameDataPath, System.Text.Encoding.UTF8));

		Debug.Log("Game Data loaded from: " + GameDataPath);
	}

	protected override void SaveGameData()
	{
		//TODO: Re-enable binary serialization. Using JSON for testing purposes.
		//using (FileStream fs = new FileStream(GameDataPath, FileMode.Create, FileAccess.Write))
		//{
		//	var Formatter = new BinaryFormatter();
		//	Formatter.Serialize(fs, Data);
		//}

		File.WriteAllText(GameDataPath, JsonUtility.ToJson(Data, true), System.Text.Encoding.UTF8);

		Debug.Log("Game Data saved at: " + GameDataPath);
	}
}