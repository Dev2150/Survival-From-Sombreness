using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Load : MonoBehaviour {

	public static int Difficulty = 0;
	public GameObject HelpWindow;

	public void Update() {
		Cursor.visible = true;
	}

	public void gs_e() {
		//SceneManager.LoadScene (SceneManager.GetSceneByName ("Overworld").buildIndex);
		Cursor.visible = false;
		SceneManager.LoadScene ("Overworld", LoadSceneMode.Single);
		Difficulty = -2;
	}

	public void gs_m() {
		//SceneManager.LoadScene (SceneManager.GetSceneByName ("Overworld").buildIndex);
		Cursor.visible = false;
		SceneManager.LoadScene ("Overworld", LoadSceneMode.Single);
		Difficulty = -1;
	}
	
	public void gs_h() {
		//SceneManager.LoadScene (SceneManager.GetSceneByName ("Overworld").buildIndex);
		Cursor.visible = false;
		SceneManager.LoadScene ("Overworld", LoadSceneMode.Single);
		Difficulty = 0;
	}
	public void gs_x() {
		//SceneManager.LoadScene (SceneManager.GetSceneByName ("Overworld").buildIndex);
		Cursor.visible = false;
		SceneManager.LoadScene ("Overworld", LoadSceneMode.Single);
		Difficulty = 2;
	}
	public void gs_r() {
		//SceneManager.LoadScene (SceneManager.GetSceneByName ("Overworld").buildIndex);
		Cursor.visible = false;
		SceneManager.LoadScene ("Overworld", LoadSceneMode.Single);
		Difficulty = 4;
	}
	
	public void Help() {
		//SceneManager.LoadScene (SceneManager.GetSceneByName ("Overworld").buildIndex);
		HelpWindow.SetActive(true);
	}
	public void StopHelp() {
		//SceneManager.LoadScene (SceneManager.GetSceneByName ("Overworld").buildIndex);
		HelpWindow.SetActive(false);
	}
	
	public void GameQuit() {
		Application.Quit ();	
	}
}
