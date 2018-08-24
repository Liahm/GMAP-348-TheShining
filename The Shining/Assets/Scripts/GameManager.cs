using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;
public class GameManager : Singleton<GameManager> 
{
//------------------------------------------------------------------------CONSTANTS:

//---------------------------------------------------------------------------FIELDS:

	public float GameClock, RealTimePerDay;
	public float DayStart =8f, DayEnds = 24f;
	public int GameDay;

	private float tempRealTimePerDay, tempDayStart, tempDayEnds;
	private bool day;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		tempDayEnds = DayEnds ;
		tempDayStart = DayStart ;
		RealTimePerDay *= 60;
		tempRealTimePerDay = RealTimePerDay;
		day = true;
		GameClock = DayStart;
	}
		
	void Update()
    {
		if(GameClock <= DayEnds)
		{
			GameClock += (Time.deltaTime * ((tempDayEnds - tempDayStart) / (tempRealTimePerDay)));
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
	
//--------------------------------------------------------------------------HELPERS:
	private void ClockSystem()
	{
		
	}
}