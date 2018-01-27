using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class GameplayScreen : MonoBehaviour
{
    public Board Board;

    public GameObject Callers;
    public GameObject Receivers;
    public GameObject StressGauge;

    public Text MessageText;

    public List<PhoneUserView> CallersContainer;
    public List<PhoneUserView> ReceiversContainer;

    public void SetScreenVisibility(bool visible)
    {
        Callers.SetActive(visible);    
        Receivers.SetActive(visible);
        StressGauge.SetActive(visible);
        MessageText.gameObject.SetActive(visible);
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

        yield return new WaitForSeconds(GameController.TIME_PEOPLE_TALKING);

        CallersContainer.FirstOrDefault(user => user.id == caller.Id).Reset();
        ReceiversContainer.FirstOrDefault(user => user.id == receiver.Id).Reset();

        Board.CallCompleted(caller.Id, receiver.Id);
    }
}