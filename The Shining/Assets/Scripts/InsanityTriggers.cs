using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsanityTriggers : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

//---------------------------------------------------------------------------FIELDS:
	public Sprite HoverSprite;
	public GameObject HoverTemp;
	public bool Active;
	
	[System.NonSerialized]
	public GameObject CharacterInRoom;
	private Sprite initialSprite, currentSprite;
	private float charactersInRoom;
	
//---------------------------------------------------------------------MONO METHODS:

	void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
		
		//currentSprite = HoverSprite;
		HoverTemp.SetActive(true);
		Debug.Log("Mouse is over: " + gameObject.name);
		
    }

	void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
		//currentSprite = initialSprite;
		HoverTemp.SetActive(false);
        Debug.Log("Mouse is no longer on GameObject.");
    }

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Characters")
		{
			charactersInRoom++;
			Debug.Log(col.name);
			if(charactersInRoom <= 1)
			{
				CharacterInRoom = col.gameObject;
				Active = true;
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.tag == "Characters")
		{
			charactersInRoom--;
		}
	}

	void OnTriggerStay(Collider col)
	{
		if(col.tag == "Characters")
		{
			if(charactersInRoom > 1)
			{
				Active = false;
				//Mouse over is false and can't be clicked
			}
			else
			{
				Active = true;
				//Can be clicked
				if(GameManager.Instance.Scared)
				{
					CharactersPattern cPat = col.GetComponent<CharactersPattern>();
					cPat.Scared = true;
					GameManager.Instance.Scared = false;
				}
			}
			
		}
	}

	void Start()
	{
		charactersInRoom=0;
		
		initialSprite = gameObject.GetComponent<Sprite>();
		currentSprite = initialSprite;
	}

	void Update()
	{

	}
//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}