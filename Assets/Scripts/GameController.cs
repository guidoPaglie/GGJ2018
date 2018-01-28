﻿using System.Collections;
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

    public static float TIME_PEOPLE_TALKING = 3.0f;

    public float[] startStressLevel = new float[] { 0, 15, 25, 50 };
    public float[] maxStressLevel = new float[] { 45, 60, 85, 100 };
    public float[] phoneRates = new float[] { 1.50f, 1.00f, 0.75f, 0.50f };
    public float[] timeBetweenRounds = new float[] { 29.0f, 30.0f, 32.0f, 2.0f };

    public Board Board;
    public GameplayScreen GameplayScreen;
    public StressController StressController;

    private TelephoneCentral _telephoneCentral;
    private PhoneUsers _phoneUsers;
    private PhoneCallsHarcode _phoneCallsHarcoded;

    public static int _currentRound = 0;

    private GameState currentGameState = GameState.NOT_PLAYING;

    public List<Sprite> RoundSprites;
    public List<AudioClip> RoundNarrations;
    public List<AudioClip> RoundMusic;
    public SpriteRenderer RoundSprite;
    public AudioSource RoundNarration;
    public AudioSource RoundMusicSource;

    void Start () 
    {
        _phoneUsers = new PhoneUsers();
        _phoneCallsHarcoded = new PhoneCallsHarcode();

        _telephoneCentral = new TelephoneCentral(this, Board, StressController, _phoneUsers);

        //StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(StartRound(0.0f, timeBetweenRounds[_currentRound]));   
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
        GameplayScreen.ShowCallerMessage(caller);
    }

    public void CallCompleted(int callerId, int receiverId)
    {
        var caller = _phoneUsers.users.FirstOrDefault(user => user.Id == callerId);
        var receiver = _phoneUsers.users.FirstOrDefault(user => user.Id == receiverId);
        GameplayScreen.CallCompleted(caller, receiver);
    }

    public void WrongConnection(int receiverId)
    {
        var receiver = _phoneUsers.users.FirstOrDefault(user => user.Id == receiverId);
        GameplayScreen.ShowReceiverMessage(receiver, false);
    }

    public void NotifyEndOfRound()
    {
        //Debug.Log("ROUND FINISH");
        _currentRound++;
        RoundMusicSource.Stop();
        RoundMusicSource.volume = 0;

        currentGameState = GameState.END_OF_ROUND;

        StartCoroutine(StartRound(TIME_PEOPLE_TALKING, timeBetweenRounds[_currentRound]));
    }

    private IEnumerator StartRound(float endOfRound, float difRound)
    {
        yield return new WaitForSeconds(endOfRound);

        GameplayScreen.SetScreenVisibility(false);
        GameplayScreen.ResetData();

        Board.ResetBoard();

        RoundSprite.gameObject.SetActive(true);
        RoundSprite.sprite = RoundSprites[_currentRound];
        RoundNarration.clip = RoundNarrations[_currentRound];
        RoundNarration.PlayDelayed(0.5f);

        yield return new WaitForSeconds(difRound);

        RoundNarration.Stop();
        RoundSprite.gameObject.SetActive(false);
        currentGameState = GameState.PLAYING;

        GameplayScreen.SetScreenVisibility(true);

        _telephoneCentral.InitializeRound(_phoneCallsHarcoded.phoneCalls[_currentRound], phoneRates[_currentRound], _currentRound == _phoneCallsHarcoded.phoneCalls.Count - 1);
        StressController.SetupStresslevels(startStressLevel[_currentRound], maxStressLevel[_currentRound]);
        RoundMusicSource.clip = RoundMusic[_currentRound];
        RoundMusicSource.Play();

        while (RoundMusicSource.volume < 0.2f)
        {
            yield return new WaitForSeconds(0.05f);
            RoundMusicSource.volume += 0.025f;
        }
    }

    public void NotifyGameOver()
    {
        currentGameState = GameState.GAMEOVER;
        Debug.Log("GAME OVER");
    }
}
