using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;
public class CharactersPattern : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

//---------------------------------------------------------------------------FIELDS:
	[System.Serializable]
	public class Patrol
	{
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
			Room = rooms[UnityEngine.Random.Range(0, rooms.Length)];
			return Room;
			
		}
	}

	[System.Serializable]
	public class Insanity
	{
		public Patrol[] PatrolOnThisInsanityLevel;
	}
	
	public float CharacterSpeed;
	public int InsanityValue, InsanityThresholds, TotalInsanityValue;
	public Insanity[] CharacterMovement;
	[System.NonSerialized]
	public bool Scared;

	private GameObject endDestination;
	
	public int insanityHits, patrolVal = 0;
	private NavMeshAgent Agent;
	private bool add, moving;
    private Animator charAnim;

    //status bar variables
    public GameObject CharBar;
    public Slider CharSlider;
    private Image CharFill;
	private SceneVars sceneVars;

    //---------------------------------------------------------------------MONO METHODS:

    void Start() 
	{
        charAnim = GetComponentInChildren<Animator>();

        add = false;
		moving = true;
		insanityHits = InsanityThresholds;
		Agent = GetComponent<NavMeshAgent>();
		Agent.speed = CharacterSpeed;
		sceneVars = GameObject.FindObjectOfType<SceneVars>();
	
		if(sceneVars.ShinningCharacters != null
			&& UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Hotel")
		{
			Debug.Log("Day two, it came here");
			foreach(var characters in sceneVars.ShinningCharacters)
			{
				if(characters.Character == transform.gameObject.name)
				{
					InsanityValue = characters.InsanityValue;
					endDestination = CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel[0].Room;
				}
			}
		}
		else
		{	
			endDestination = CharacterMovement[0].PatrolOnThisInsanityLevel[0].Room;
		}
		
				

        //set up of character status bars
        CharFill = CharBar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        CharSlider.value = InsanityValue*InsanityThresholds + (InsanityThresholds - insanityHits);
        CharSlider.maxValue = InsanityThresholds*TotalInsanityValue;
        //UpdateStatus();
    }
		
	void Update()
    {
		ChangeDestination();
		if(Vector3.Distance(transform.position, endDestination.transform.position)
					<= 1f && moving)
		{
			patrolVal++;
			moving = false;
            charAnim.SetBool("walking", false);
		}
		else if (Vector3.Distance(transform.position, endDestination.transform.position)
					>= 1f)
		{
			Agent.SetDestination(endDestination.transform.position);
			moving = true;
			if(Vector2.Distance(endDestination.transform.position, new Vector2(23, transform.position.y)) 
				> Vector2.Distance(endDestination.transform.position, new Vector2(68, transform.position.y)) 
				&& transform.rotation.y != 180)
			{
				transform.eulerAngles = transform.eulerAngles + 180 * Vector3.up;
			}
			else
				transform.eulerAngles = transform.eulerAngles - 180 * Vector3.up;
            charAnim.SetBool("walking", true);
        }

		if(Scared) //Only enter once per scare
		{
			insanityHits--;
			if(insanityHits == 0)
			{
				if(CharacterMovement.Length < InsanityValue     //[previously] if(CharacterMovement.Length > InsanityValue && InsanityValue != 0)                                              
                && InsanityValue != 0)                          //[Mark didn't understand what this was checking and it broken when Insanity passed value 1]
				{
					InsanityValue--;
				}
				else
				{
					InsanityValue++;
                    if (InsanityValue < TotalInsanityValue) patrolVal = PatrolValueChange(); //Temporary fix: PatrolValueChange() would glitch when Insanity Value exceeds total
                }
				if(InsanityValue == TotalInsanityValue)
				{
					GameManager.Instance.EndGame();
				}
				insanityHits = InsanityThresholds;
			}
			Scared = false;
			UpdateStatus();            
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

					if(CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel[patrolVal+1].Room != null)
					{
						while (endDestination.transform == CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel[patrolVal+1].Room
							|| endDestination.transform == CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel[patrolVal-1].Room)
						{
							Debug.Log("oho");
							endDestination = CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel[patrolVal].RandomRoom();
						}
					}
				}
			}
		}
	}

	//function for updating UI status of each character (level of insanity, color, etc.)
    public void UpdateStatus()
    {
        int tempInsaneVal = InsanityValue * InsanityThresholds + (InsanityThresholds - insanityHits);
        CharSlider.value = tempInsaneVal;

        if (tempInsaneVal < InsanityThresholds) CharFill.color = Color.green;
        else if (tempInsaneVal >= InsanityThresholds && tempInsaneVal < 2 * InsanityThresholds) CharFill.color = Color.gray;
        else if (tempInsaneVal >= 2 * InsanityThresholds && tempInsaneVal < 3 * InsanityThresholds) CharFill.color = Color.yellow;
        else if (tempInsaneVal >= 3 * InsanityThresholds) CharFill.color = Color.red;
    }
//--------------------------------------------------------------------------HELPERS:
	private int PatrolValueChange()
	{
		//Change patrolVal when insanity level changes.
		float[] allTime = new float[CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel.Length];


		for(int i = 0; i < CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel.Length; ++i)
		{
			allTime[i] = CharacterMovement[InsanityValue].PatrolOnThisInsanityLevel[i].Time;
		}

		var closest = float.MaxValue;
		var minDifference = float.MaxValue;
		foreach(var element in allTime)
		{
			var difference = Mathf.Abs((long)element - GameManager.Instance.GameClock);
			if(minDifference >difference)
			{
				minDifference = (float)difference;
				closest = element;
			}
		}
		int index = Array.IndexOf(allTime, closest);
		//Debug.Log(index);

		return index;
	}

    private IEnumerator LateStart()
	{
		yield return new WaitForSeconds(2);
		foreach(var characters in sceneVars.ShinningCharacters)
		{
			if(characters.Character == transform.gameObject.name)
			{
				endDestination = CharacterMovement[characters.InsanityValue].PatrolOnThisInsanityLevel[0].Room;
			}
		}
	}
}