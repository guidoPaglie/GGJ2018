using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState
{
    NOT_PLAYING,
    PLAYING,
    END_OF_ROUND,
    GAMEOVER
}

public class GameController : MonoBehaviour {

    public static float TIME_PEOPLE_TALKING = 1.0f;
    public static float TIME_BETWEEN_ROUNDS = 1.0f;

    public Board Board;
    public GameplayScreen GameplayScreen;
    public StressController StressController;

    private TelephoneCentral _telephoneCentral;
    private PhoneUsers _phoneUsers;
    private PhoneCallsHarcode _phoneCallsHarcoded;

    private int _currentRound;

    private GameState currentGameState = GameState.NOT_PLAYING;

    public List<Sprite> RoundSprites;
    public SpriteRenderer RoundSprite;

    void Start () 
    {
        _phoneUsers = new PhoneUsers();
        _phoneCallsHarcoded = new PhoneCallsHarcode();

        _telephoneCentral = new TelephoneCentral(this, Board, StressController, _phoneUsers);

        StartCoroutine(StartRound(0.0f, TIME_BETWEEN_ROUNDS));
    }

    private void Update()
    {
        if (currentGameState == GameState.PLAYING)
        {
            _telephoneCentral.OnUpdate();
        }
    }

    public void NotifyShowCaller(int currentCaller)
    {
        var caller = _phoneUsers.users.FirstOrDefault(user => user.Id == currentCaller);
        GameplayScreen.PositionateUserWithSprite(caller, currentCaller);
    }

    public void CallCompleted(int callerId, int receiverId)
    {
        var caller = _phoneUsers.users.FirstOrDefault(user => user.Id == callerId);
        var receiver = _phoneUsers.users.FirstOrDefault(user => user.Id == receiverId);
        GameplayScreen.CallCompleted(caller, receiver);
    }

    public void NotifyEndOfRound()
    {
        Debug.Log("ROUND FINISH");
        _currentRound++;

        currentGameState = GameState.END_OF_ROUND;

        StartCoroutine(StartRound(TIME_PEOPLE_TALKING, TIME_BETWEEN_ROUNDS));
    }

    private IEnumerator StartRound(float endOfRound, float difRound)
    {
        yield return new WaitForSeconds(endOfRound);

        GameplayScreen.StartRound(false);

        RoundSprite.gameObject.SetActive(true);
        RoundSprite.sprite = RoundSprites[_currentRound];

        yield return new WaitForSeconds(difRound);

        RoundSprite.gameObject.SetActive(false);
        currentGameState = GameState.PLAYING;

        GameplayScreen.StartRound(true);
        _telephoneCentral.InitializeRound(_phoneCallsHarcoded.phoneCalls[_currentRound], 0.5f, false);
    }

    public void NotifyGameOver()
    {
        currentGameState = GameState.GAMEOVER;
        Debug.Log("GAME OVER");
    }
}
