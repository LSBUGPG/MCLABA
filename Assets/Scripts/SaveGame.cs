using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
	public string position;
	public int time;
	public List<Character> characters = new List<Character>();
}

public class SaveGame : MonoBehaviour {

	public SaveData saveData = new SaveData();

	string GetPath(int slot)
	{
		return Path.Combine (Application.persistentDataPath, string.Format ("SaveFile{0}.json", slot));
	}

	public void SaveToSlot(int slot)
	{
		Debug.Log (GetPath (slot));
		using (var streamWriter = new StreamWriter (GetPath(slot))) {
			streamWriter.Write (JsonUtility.ToJson (saveData));
		}
	}

	public void LoadFromSlot(int slot)
	{
		string path = GetPath (slot);
		if (File.Exists (path)) {
			using (var streamReader = new StreamReader (path)) {
				saveData = JsonUtility.FromJson<SaveData> (streamReader.ReadLine ());
			}
		}
	}
}
