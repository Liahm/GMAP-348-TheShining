﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharactersPattern : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

//---------------------------------------------------------------------------FIELDS:
	[System.Serializable]
	public class Patrol
	{
		public int Day;
		public float Time;
		public GameObject Room;
		
		private GameObject[] rooms;

		public void GetRooms()
		{
			GameObject[] allRooms = GameObject.FindGameObjectsWithTag("Location");
			rooms = new GameObject[allRooms.Length];
			for ( int i = 0; i < allRooms.Length; ++i )
			{
				rooms[i] = allRooms[i];
				//Debug.Log(rooms[i].name);
			}	
		}
		public GameObject RandomRoom()
		{
			GetRooms();
			Debug.Log(Room);
			if (Room == null)
			{
				Room = rooms[Random.Range(0, rooms.Length)];
				return Room;
			}
			else
				return Room = rooms[0];
		}
	}

	[System.Serializable]
	public class Insanity
	{
		public Patrol[] PatrolOnThisInsanityLevel;
	}
	
	public float CharacterSpeed;
	public int InsanityValue, InsanityTreshHolds;
	public Insanity[] CharacterMovement;
	[System.NonSerialized]
	public bool Scared;

	private GameObject endDestination;
	private int insanityHits, patrolVal = 0;
	private NavMeshAgent Agent;
	private bool add, moving;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		add = false;
		moving = true;
		insanityHits = InsanityTreshHolds;
		Agent = GetComponent<NavMeshAgent>();
		Agent.speed = CharacterSpeed;

		//Hardcoding 0 because this is the start of the level.
		CharacterMovement[0].PatrolOnThisInsanityLevel[0].GetRooms();
		endDestination = CharacterMovement[0].PatrolOnThisInsanityLevel[0].Room;
	}
		
	void Update()
    {
		ChangeDestination();
		if(Vector3.Distance(transform.position, endDestination.transform.position)
					<= 1f && moving)
		{
			patrolVal++;
			moving = false;
		}
		else if (Vector3.Distance(transform.position, endDestination.transform.position)
					>= 1f)
		{
			Agent.SetDestination(endDestination.transform.position);
			moving = true;
		}

		if(Scared) //Only enter once per scare
		{
			insanityHits--;
			if(insanityHits == 0)
			{
				InsanityValue++;
				insanityHits = InsanityTreshHolds;
			}
			Scared = false;
		}
    }

//--------------------------------------------------------------------------METHODS:
	public void ChangeDestination()
	{
		//Check if it's not last patrol
		if(CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel.Length > patrolVal)
		{
			if(GameManager.Instance.GameClock //Time to move
				>= CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel[patrolVal].Time
				)
			{
				if(CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel[patrolVal].Room != null)
				{
					endDestination = CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel[patrolVal].Room;
				}
				else
				{
					endDestination = CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel[patrolVal].RandomRoom();
				}
			}

		}
	}
//--------------------------------------------------------------------------HELPERS:
	
}