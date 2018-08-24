using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersPattern : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

//---------------------------------------------------------------------------FIELDS:
	[System.Serializable]
	public class Patrol
	{
		public int Day;
		public int Time;
		public GameObject Room;
	}

	[System.Serializable]
	public class Insanity
	{
		public int InsanityLevel;
		public Patrol[] PatrolPerInsanityLevel;
	}
	
	public Insanity[] CharacterMovement;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{

	}
		
	void Update()
    {

    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}