using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

//---------------------------------------------------------------------------FIELDS:
	
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{

	}
		
	void Update()
    {
		//Debug stuff
		if(Input.GetKeyUp(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if(Input.GetKeyUp(KeyCode.Space))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
    }

//--------------------------------------------------------------------------METHODS:
	public void Next()
	{
		if(SceneManager.GetActiveScene().name == "GameOver" 
			|| SceneManager.GetActiveScene().name == "Victory")
		{	
			SceneManager.LoadScene("MainMenu");	
		}
		else
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);		
	}

	public void GoBackToMainMenu()
	{
		SceneManager.LoadScene("Main Menu");	
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void Quit()
	{
		Application.Quit();
	}
//--------------------------------------------------------------------------HELPERS:
	
}