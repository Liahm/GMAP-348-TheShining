using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	 private const float
        hoursToDegrees = 360f / 12f,
        minutesToDegrees = 360f / 60f;
        //secondsToDegrees = 360f / 60f;
//---------------------------------------------------------------------------FIELDS:
	public Transform Hours, Minutes;//, Seconds;

	private float time;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{

	}
		
	void Update()
    {
		 
		time = GameManager.Instance.GameClock;
		Hours.localRotation = Quaternion.Euler(0f, 0f, time * -hoursToDegrees);
        Minutes.localRotation = Quaternion.Euler(0f, 0f, time*60 * -minutesToDegrees);
        //Seconds.localRotation = Quaternion.Euler(0f, 0f, time*3600 * -secondsToDegrees);
    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}