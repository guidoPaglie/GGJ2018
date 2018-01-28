using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

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

    private CringeTexts cringeTexts;
    private int currentCringeText;

    public void Awake()
    {
        cringeTexts = new CringeTexts();
    }

    public void SetScreenVisibility(bool visible)
    {
        Panel.SetActive(visible);
    }

    public void ResetData()
    {
        CallerMsg.text = "";
        ReceiverMsg.text = "";

        BackSpriteRenderer.sprite = BackSprites[GameController._currentRound < 3 ? GameController._currentRound : 2];
    }

    public void ChangeBackground(Sprite background)
    {
        BackSpriteRenderer.sprite = background;
    }
    
    public void PositionateUserWithSprite(PhoneUser caller, int id)
    {
        PhoneUserView phoneUserView = CallersContainer.FirstOrDefault(user => !user.inUse);
        if (phoneUserView != null)
            phoneUserView.SetUser(caller.CharacterSprite, id);
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

        // aca se apagan los mensajes
        CallersContainer.FirstOrDefault(user => user.id == caller.Id).Reset();
        ReceiversContainer.FirstOrDefault(user => user.id == receiver.Id).Reset();

        Board.CallCompleted(caller.Id, receiver.Id);
    }

    public void ShowCallerMessage(PhoneUser caller)
    {
        if (GameController._currentRound == 3 && TelephoneCentral.HasFinishedRound)
        {
            Debug.Log("caller cringe");
            ShowCringeText(CallerMsg);
        }
        else
        {
            CallerMsg.text = caller.roundTexts[GameController._currentRound]["inicio"].str;
        }

        CallerMsgContainer.sprite = GetSprite(caller.Id);    
    }

    public void ShowReceiverMessage(PhoneUser receiver, bool isRight, int callerId)
    {
        if (GameController._currentRound == 3 && TelephoneCentral.HasFinishedRound)
        {
            Debug.Log("receiver cringe");
            ShowCringeText(ReceiverMsg);
        }
        else
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
        }

        ReceiverMsgContainer.sprite = GetSprite(receiver.Id);
    }

    private void ShowCringeText(Text textContainer)
    {
        textContainer.text = cringeTexts.texts[currentCringeText];
        currentCringeText++;

        if (currentCringeText == 9)
        {
            Board.AllCringeShown();
            Invoke("ChangeScene", 2.0f);
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("Gameover", LoadSceneMode.Single);
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