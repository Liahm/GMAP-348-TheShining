using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SceneVars : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

//---------------------------------------------------------------------------FIELDS:
	[System.NonSerialized]
	public static SceneVars instance = null;

	[System.Serializable]
	public class CharacterValues
	{
		public string Character;
		public int InsanityValue = 0;

		public CharacterValues(string chara, int ins)
		{
			Character = chara;
			InsanityValue = ins;
		}
	}

	public CharacterValues[] ShinningCharacters;
	private GameObject[] characters;
	private int numOfCharacters;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		if(instance == null)   instance = this;
		else if(instance != this)	Destroy(gameObject);
		
		DontDestroyOnLoad(gameObject);	

		numOfCharacters = GameObject.FindGameObjectsWithTag("Characters").Length;
		characters = new GameObject[numOfCharacters];
		ShinningCharacters = new CharacterValues[numOfCharacters];

		characters = GameObject.FindGameObjectsWithTag("Characters");
		
		StartCoroutine(test());
	}

//--------------------------------------------------------------------------METHODS:
	public void AddValues()
	{
		int i = 0;
		
		foreach(GameObject character in characters)
		{
			ShinningCharacters[i] = new CharacterValues(character.name, character.GetComponent<CharactersPattern>().InsanityValue);
			i++;
		}
		

	}
//--------------------------------------------------------------------------HELPERS:
	private IEnumerator test()
	{
		yield return new WaitForSeconds(5);
		AddValues();
	}
}
