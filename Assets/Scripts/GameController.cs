using System;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static int CurrentRound = 0;

    public Board Board;
    public GameplayScreen GameplayScreen;

    public GameObject PhoneUserPrefab;

    private TelephoneCentral _telephoneCentral;
    private PhoneUsers _phoneUsers;
    private PhoneCallsHarcode _phoneCallsHarcoded;

	void Start () 
    {
        _phoneUsers = new PhoneUsers();
        _phoneCallsHarcoded = new PhoneCallsHarcode();

        _telephoneCentral = new TelephoneCentral(this, Board, _phoneUsers);

        _telephoneCentral.InitializeRound(_phoneCallsHarcoded.phoneCalls[CurrentRound], 0.5f, false);
	}

    private void Update()
    {
        _telephoneCentral.OnUpdate();    
    }

    public void NotifyShowCaller(int currentCaller)
    {
        GameplayScreen.PositionateUserWithSprite(_phoneUsers.users.FirstOrDefault(user => user.Id == currentCaller).CharacterSprite);
    }

    public void CallCompleted()
    {
        
    }

    public void NotifyEndOfRound()
    {
        CurrentRound++;
        _telephoneCentral.InitializeRound(_phoneCallsHarcoded.phoneCalls[CurrentRound], 0.5f, false);
    }
}
