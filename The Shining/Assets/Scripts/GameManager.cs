using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;
using UnityEngine.UI; 

public class GameManager : Singleton<GameManager> 
{
//------------------------------------------------------------------------CONSTANTS:

//---------------------------------------------------------------------------FIELDS:

	public float GameClock, RealTimePerDay, ScareMeterValue, ScareUsage, ReplenishRate;
	public float DayStart =8f, DayEnds = 24f;
	public int GameDay;
	public GameObject ScareBar;
	public Slider Bar;
	public Text text;
	public string DepletedText;

	[System.NonSerialized]
	public bool AllowScare;
	private float tempRealTimePerDay, tempDayStart, tempDayEnds, maxScareMeterValue;
	private bool day;
	private Vector3 mousePos;
	private Ray ray;
	private Image Fill;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		Fill = ScareBar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
		Bar.value = ScareMeterValue;
		Bar.maxValue = ScareMeterValue;
		maxScareMeterValue = ScareMeterValue;
		GameDay = 1;
		tempDayEnds = DayEnds ;
		tempDayStart = DayStart ;
		RealTimePerDay *= 60;
		tempRealTimePerDay = RealTimePerDay;
		day = true;
		GameClock = DayStart;
	}
		
	void Update()
    {
		ScareMeter();
		MouseMovement();

		if(GameDay == 3)
		{
			//Check for end of game
			EndGame();
		}

		if(GameClock <= DayEnds)
		{
			GameClock += (Time.deltaTime * ((tempDayEnds - tempDayStart) / (tempRealTimePerDay)));
		}
		else if(GameClock >= DayEnds)
		{
			//End Day
			//Fade to black, then either change scenes, or just move players. IDK yet
			GameDay++;
		}

		if(RealTimePerDay > 0 && day)
		{
			RealTimePerDay -= Time.deltaTime;
		}
		else
		{
			day = false;
			RealTimePerDay = tempRealTimePerDay;
		}
    }

//--------------------------------------------------------------------------METHODS:
	public void ScareMeter()
	{
		//Debug.Log(ScareMeterValue);
		//Replenish the scaremeter
		if(ScareMeterValue < maxScareMeterValue)
		{
			ScareMeterValue += ReplenishRate*Time.deltaTime;
		}
		Bar.value = ScareMeterValue;
		if(ScareMeterValue >= ScareUsage)
		{
			text.text = "";
			AllowScare = true;
		}
		else
		{
			text.text = DepletedText;
			AllowScare = false;

		}
	}
	public void ChangeSliderColor()
	{
		if(ScareMeterValue <= 0)
		{
			Fill.color = Color.red;
		}
		else
			Fill.color = Color.yellow;
	}
//--------------------------------------------------------------------------HELPERS:
	private void EndGame()
	{
		Debug.Log("IT'S DAY 3");
	}

	private void MouseMovement()
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

			ray = Camera.main.ScreenPointToRay(mousePos);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, 1000f) && hit.collider.tag == "Interactions")
			{
				Debug.Log("Clicked");
				InsanityEffects(hit);
			}
		}
	}

	private void InsanityEffects(RaycastHit hit)
	{
		InsanityTriggers insane = hit.collider.GetComponent<InsanityTriggers>();
		//Debug.Log("AllowScare: " + AllowScare);
		if(insane.Active && AllowScare)
		{
			//Debug.Log("Scare allowed");
			ScareMeterValue -= ScareUsage;
			Bar.value -= ScareUsage;
			insane.Active = false;
		}
	}
}