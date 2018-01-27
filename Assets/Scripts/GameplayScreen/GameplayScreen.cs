using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameplayScreen : MonoBehaviour
{
    public Button BackBtn;

    public List<Transform> CallersContainer;
    public List<Transform> ReceiversContainer;

    public void Start()
    {
        BackBtn.onClick.AddListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void PositionateUser(RectTransform obj)
    {
        obj.transform.SetParent(CallersContainer[0]);
        obj.anchoredPosition = Vector3.zero;
    }
}
