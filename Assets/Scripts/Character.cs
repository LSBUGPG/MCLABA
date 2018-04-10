using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
	public string name;
	public int affection;

	public Character(Character original)
	{
		name = original.name;
		affection = original.affection;
	}

	public Character(string name)
	{
		this.name = name;
		affection = 0;
	}
}
