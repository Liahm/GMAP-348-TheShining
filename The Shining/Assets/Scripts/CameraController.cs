using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

//---------------------------------------------------------------------------FIELDS:
	public float Offset, Speed;

	public Vector2 MinMaxXPosition, MinMaxYPosition;

	private float screenWidth, screenHeight;
	private Vector3 cameraMove;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		cameraMove.x = transform.position.x;
		cameraMove.y = transform.position.y;
		cameraMove.z = transform.position.z;
		Cursor.lockState = CursorLockMode.Confined;
	}
		
	void Update()
    {
		 //Move camera
        if ((Input.mousePosition.x > screenWidth - Offset) 
				&& transform.position.x < MinMaxXPosition.y)
        {
            cameraMove.x += MoveSpeed();
        }
        if ((Input.mousePosition.x < Offset) 
				&& transform.position.x > MinMaxXPosition.x)
        {
            cameraMove.x -= MoveSpeed();
        }
        if ((Input.mousePosition.y > screenHeight - Offset) 
				&& transform.position.y < MinMaxYPosition.y)
        {
            cameraMove.y += MoveSpeed();
        }
        if ((Input.mousePosition.y < Offset) 
				&& transform.position.y > MinMaxYPosition.x)
        {
            cameraMove.y -= MoveSpeed();
        }
        transform.position = cameraMove;
    }

//--------------------------------------------------------------------------METHODS:
	
//--------------------------------------------------------------------------HELPERS:
	private float MoveSpeed()
	{
		return Speed * Time.deltaTime;
	}
}
