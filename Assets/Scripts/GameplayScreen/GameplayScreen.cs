using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameplayScreen : MonoBehaviour
{
    public Button BackBtn;

    public List<PhoneUserView> CallersContainer;
    public List<PhoneUserView> ReceiversContainer;

    private int callersCount;

    public void Start()
    {
        BackBtn.onClick.AddListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void PositionateUserWithSprite(Sprite sprite)
    {
        CallersContainer[callersCount].Initialize(sprite);
    }
}
