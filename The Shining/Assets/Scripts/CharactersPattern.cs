using System.Collections;
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
	}

	[System.Serializable]
	public class Insanity
	{
		public int InsanityLevel;
		public Patrol[] PatrolPerInsanityLevel;
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
		CharacterMovement[0].PatrolPerInsanityLevel[0].GetRooms();
		endDestination = CharacterMovement[0].PatrolPerInsanityLevel[0].Room;
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
		if(CharacterMovement[InsanityValue].PatrolPerInsanityLevel.Length > patrolVal)
		{
			if(GameManager.Instance.GameClock //Time to move
				>= CharacterMovement[InsanityValue].PatrolPerInsanityLevel[patrolVal].Time
				)
			{
				endDestination = CharacterMovement[InsanityValue].PatrolPerInsanityLevel[patrolVal].Room;
			}

		}
	}
//--------------------------------------------------------------------------HELPERS:
	
}