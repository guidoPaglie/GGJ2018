using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayScreen : MonoBehaviour
{
    public Button BackBtn;

    void Start()
    {
        BackBtn.onClick.AddListener(GoToMainMenu);
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
