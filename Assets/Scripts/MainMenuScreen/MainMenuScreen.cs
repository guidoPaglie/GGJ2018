using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour 
{
    public Button StartGameBtn;

	void Start () 
    {
        StartGameBtn.onClick.AddListener(StartGame);		
	}

	void StartGame () 
    {
        SceneManager.LoadScene("Gameplay",LoadSceneMode.Single);	
	}
}
