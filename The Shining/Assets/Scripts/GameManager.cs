using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

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
	public Animator anim;

	[System.NonSerialized]
	public bool AllowScare, Scared;
	private int today;
	private float tempRealTimePerDay, tempDayStart, tempDayEnds, maxScareMeterValue;
	private bool day;
	private Vector3 mousePos;
	private Ray ray;
	private Image Fill;
	private SceneVars sceneVars;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
        anim = anim.GetComponent<Animator>();
        anim.SetBool("Fade", false);
		Fill = ScareBar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
		Bar.value = ScareMeterValue;
		Bar.maxValue = ScareMeterValue;
		maxScareMeterValue = ScareMeterValue;
		today = GameDay;
		tempDayEnds = DayEnds ;
		tempDayStart = DayStart ;
		RealTimePerDay *= 60;
		tempRealTimePerDay = RealTimePerDay;
		day = true;
		GameClock = DayStart;
		sceneVars = GameObject.FindObjectOfType<SceneVars>();
	}
		
	void Update()
    {
		ScareMeter();
		MouseMovement();

		if(GameClock <= DayEnds)
		{
			GameClock += (Time.deltaTime * ((tempDayEnds - tempDayStart) / (tempRealTimePerDay)));
		}
		else if(GameClock >= DayEnds && today == GameDay)
		{
			//End Day
			sceneVars.AddValues();
			if(GameDay == 3)
			{
				//Check for end of game
				NoWinnerEndGame();
			}
			else
				EndDay();
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

	public void EndGame()	
	{
		anim.SetBool("Fade", true);

		StartCoroutine(EndGameRoutine());
	}
	public void EndDay()	
	{
		anim.SetBool("Fade", true);

		StartCoroutine(EndDayRoutine());
	}
	
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
		if(ScareMeterValue < 50)
		{
			Fill.color = Color.red;
		}
		else
			Fill.color = Color.yellow;
	}
//--------------------------------------------------------------------------HELPERS:
	
	private IEnumerator EndDayRoutine()
	{
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		//endScreen.SetActive(true);
	}

	private IEnumerator EndGameRoutine()
	{
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene("End");
		//endScreen.SetActive(true);
	}

	private IEnumerator NoWinnerRoutine()
	{
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene("Defeat");
		//endScreen.SetActive(true);
	}

	private void NoWinnerEndGame()
	{
		anim.SetBool("Fade", true);

		StartCoroutine(NoWinnerRoutine());
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
				Debug.Log("Clicked: " + hit.collider.name);
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
			Scared = true;
			insane.Active = false;
		}
	}
}