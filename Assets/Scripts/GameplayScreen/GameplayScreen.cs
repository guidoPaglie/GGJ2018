using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class GameplayScreen : MonoBehaviour
{
    public Board Board;

    public GameObject Panel;

    public Text CallerMsg;
    public Text ReceiverMsg;

    public Image CallerMsgContainer;
    public Image ReceiverMsgContainer;

    public List<PhoneUserView> CallersContainer;
    public List<PhoneUserView> ReceiversContainer;

    public List<Sprite> MsgContainerSprites;

    public List<Sprite> BackSprites;
    public SpriteRenderer BackSpriteRenderer;

    public void SetScreenVisibility(bool visible)
    {
        Panel.SetActive(visible);
    }

    public void ResetData()
    {
        CallerMsg.text = "";
        ReceiverMsg.text = "";
        if (GameController._currentRound < 3)
            BackSpriteRenderer.sprite = BackSprites[GameController._currentRound];
    }

    public void PositionateUserWithSprite(PhoneUser caller, int id)
    {
        CallersContainer.FirstOrDefault(user => !user.inUse).SetUser(caller.CharacterSprite, id);
    }

    public void CallCompleted(PhoneUser caller, PhoneUser receiver)
    {
        ShowReceiverMessage(receiver, true, caller.Id);
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

    public void ShowCallerMessage(PhoneUser caller)
    {
        CallerMsg.text = caller.roundTexts[GameController._currentRound]["inicio"].str;

        CallerMsgContainer.sprite = GetSprite(caller.Id);
    }

    public void ShowReceiverMessage(PhoneUser receiver, bool isRight, int callerId)
    {
        var strKey = isRight ? "r-positivo" : "r-neutral";
        if (isRight)
        {
            if (receiver.roundTexts[GameController._currentRound][strKey].Count > 0)
                ReceiverMsg.text = receiver.roundTexts[GameController._currentRound][strKey][callerId.ToString()].str;
            else
                ReceiverMsg.text = receiver.roundTexts[GameController._currentRound][strKey].str;
        }
        else
        {
            ReceiverMsg.text = receiver.roundTexts[GameController._currentRound][strKey].str;
        }


        ReceiverMsgContainer.sprite = GetSprite(receiver.Id);
    }

    private Sprite GetSprite(int Id)
    {
        if (Id == 0 || Id == 9 || Id == 11 || Id == 12)
            return MsgContainerSprites[0];
        if (Id == 5 || Id == 6 || Id == 13 || Id == 15)
            return MsgContainerSprites[1];
        if (Id == 2 || Id == 4 || Id == 8 || Id == 10)
            return MsgContainerSprites[2];
        if (Id == 1 || Id == 3 || Id == 7 || Id == 14)
            return MsgContainerSprites[3];

        return MsgContainerSprites[0];
    }
}