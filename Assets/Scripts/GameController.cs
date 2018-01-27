﻿using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static float TimerPeopleTalking = 1.0f;

    public Board Board;
    public GameplayScreen GameplayScreen;
    public StressController StressController;

    private TelephoneCentral _telephoneCentral;
    private PhoneUsers _phoneUsers;
    private PhoneCallsHarcode _phoneCallsHarcoded;

    private int _currentRound;
    private bool norahIsAlive = true;

    void Start () 
    {
        _phoneUsers = new PhoneUsers();
        _phoneCallsHarcoded = new PhoneCallsHarcode();

        _telephoneCentral = new TelephoneCentral(this, Board, StressController, _phoneUsers);

        _telephoneCentral.InitializeRound(_phoneCallsHarcoded.phoneCalls[_currentRound], 1.5f, false);
    }

    private void Update()
    {
        if (norahIsAlive)
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
        _currentRound++;

        _telephoneCentral.InitializeRound(_phoneCallsHarcoded.phoneCalls[_currentRound], 0.5f, false);
    }

    public void NotifyGameOver()
    {
        norahIsAlive = false;
        Debug.Log("GAME OVER");
    }
}
