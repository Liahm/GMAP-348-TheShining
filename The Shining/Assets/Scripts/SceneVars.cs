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
		try
		{
			characters = new GameObject[numOfCharacters];
			characters = GameObject.FindGameObjectsWithTag("Characters");
			ShinningCharacters = new CharacterValues[numOfCharacters];
			foreach(GameObject character in characters)
			{
				ShinningCharacters[i].Character = character.name;
				ShinningCharacters[i].InsanityValue = character.GetComponent<CharactersPattern>().InsanityValue;
				i++;
			}
		}
		catch(Exception e)
		{
			Debug.Log(e);
		}
	}
//--------------------------------------------------------------------------HELPERS:
	private IEnumerator test()
	{
		yield return new WaitForSeconds(5);
		AddValues();
	}
}
