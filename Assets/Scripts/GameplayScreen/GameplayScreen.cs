using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class GameplayScreen : MonoBehaviour
{
    public Button BackBtn;

    public List<PhoneUserView> CallersContainer;
    public List<PhoneUserView> ReceiversContainer;

    public float TimerPeopleTalking = 2.0f;

    public void Start()
    {
        BackBtn.onClick.AddListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void PositionateUserWithSprite(PhoneUser caller, int id)
    {
        CallersContainer.FirstOrDefault(user => !user.inUse).SetUser(caller.CharacterSprite, id);
    }

    public void CallCompleted(PhoneUser caller, PhoneUser receiver)
    {
        StartCoroutine(CallerAndReceivingTalking(caller, receiver));
    }

    private IEnumerator CallerAndReceivingTalking(PhoneUser caller, PhoneUser receiver)
    {
        ReceiversContainer.FirstOrDefault(user => !user.inUse).SetUser(receiver.CharacterSprite, receiver.Id);

        yield return new WaitForSeconds(TimerPeopleTalking);

        CallersContainer.FirstOrDefault(user => user.id == caller.Id).Reset();
        ReceiversContainer.FirstOrDefault(user => user.id == receiver.Id).Reset();
    }
}