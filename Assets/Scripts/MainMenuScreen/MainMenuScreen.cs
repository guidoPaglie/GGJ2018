using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour 
{
    public GameController game;

	void OnMouseDown () 
    {
        game.StartGame();
        gameObject.SetActive(false);
	}
}
