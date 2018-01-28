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
    CRINGE,
    GAMEOVER
}

public class GameController : MonoBehaviour {

    public static float TIME_PEOPLE_TALKING = 3.0f;

    public float[] startStressLevel = new float[] { 0, 15, 25, 50 };
    public float[] maxStressLevel = new float[] { 45, 60, 85, 100 };
    public float[] phoneRates = new float[] { 1.50f, 1.00f, 0.75f, 0.50f };
    public float[] timeBetweenRounds = new float[] { 0, 0, 0, 0 };

    public List<Sprite> cringeBack;
    public List<Sprite> cringeBackEnd;

    public Board Board;
    public GameplayScreen GameplayScreen;
    public StressController StressController;
    public SFXController SFXController;

    private TelephoneCentral _telephoneCentral;
    private PhoneUsers _phoneUsers;
    private PhoneCallsHarcode _phoneCallsHarcoded;

    public static int _currentRound = 0;

    public GameState currentGameState = GameState.NOT_PLAYING;

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

        _telephoneCentral = new TelephoneCentral(this, Board, StressController, _phoneUsers, SFXController);

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

        Board.ShowCable(currentCaller);
    }

    public void CallCompleted(int callerId, int receiverId)
    {
        var caller = _phoneUsers.users.FirstOrDefault(user => user.Id == callerId);
        var receiver = _phoneUsers.users.FirstOrDefault(user => user.Id == receiverId);
        GameplayScreen.CallCompleted(caller, receiver);

        Board.ShowCable(receiverId);

        //if (_currentRound == 2 && callerId == 4 && receiverId == 8)
        if (_currentRound == 2 && callerId == 9 && receiverId == 11)
        {
            StartCoroutine(DoCringe(false));
        }

        if (_currentRound == 3 && callerId == 1 && receiverId == 3)
        {
            StartCoroutine(DoCringe(false));
        }

        if (_currentRound == 3 && callerId == 7 && receiverId == 3)
        {
            StartCoroutine(DoCringe(true));
        }
    }

    public void WrongConnection(int receiverId)
    {
        var receiver = _phoneUsers.users.FirstOrDefault(user => user.Id == receiverId);
        GameplayScreen.ShowReceiverMessage(receiver, false, -1);
    }

    public void NotifyEndOfRound()
    {
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

    private IEnumerator DoCringe(bool endCringe)
    {
        currentGameState = GameState.CRINGE;
        RoundMusicSource.Pause();

        yield return new WaitForSeconds(1f);

        SFXController.Stop();
        SFXController.PlayCringe();

        if (endCringe)
        {
            for (int frame = 0; frame < cringeBackEnd.Count; frame++)
            {
                GameplayScreen.ChangeBackground(cringeBackEnd[frame]);

                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            for (int frame = 0; frame < cringeBack.Count; frame++)
            {
                GameplayScreen.ChangeBackground(cringeBack[frame]);

                yield return new WaitForSeconds(0.05f);
            }
        }

        RoundMusicSource.UnPause();
        currentGameState = GameState.PLAYING;
    }

    public void NotifyGameOver()
    {
        currentGameState = GameState.GAMEOVER;
        Debug.Log("GAME OVER");
    }
}
