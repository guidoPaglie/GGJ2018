using UnityEngine;

public class MainMenuScreen : MonoBehaviour 
{
    public GameController game;

	void OnMouseDown () 
    {
        game.StartGame();
        gameObject.SetActive(false);
	}
}
