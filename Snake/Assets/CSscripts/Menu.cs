using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void play()
	{
		SceneManager.LoadScene("Game");
	}

	public void shop()
	{
		print("Entered Shop!");
	}

	public void quit()
	{
		if(Application.isEditor == true)
			print("Game can't quit inside of Unity Editor");
		else
			print("Quiting game...");
		
		Application.Quit();
	}
}
