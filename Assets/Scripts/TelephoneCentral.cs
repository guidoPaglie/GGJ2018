using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ConnectionResult
{
    IS_CALLER,
    IS_RECEIVER,
    IS_WRONG,
    IS_VOID,  // es cuando no hay que prender nada
    IS_SAME
}

public class TelephoneCentral
{
    public static TelephoneCentral Instance;

    private Board board;
    private StressController stressController;
    private GameController gameController;
    private PhoneUsers phoneUsers;

    private List<CallGroup> callGroups; 

    private List<PhoneCall> phoneCalls = new List<PhoneCall>();
    private float phoneCallRate;
    private float phoneCallRateTimer = 1;

    private int phoneCallIndex;
    private PhoneCall currentPhoneCall;

    public TelephoneCentral(GameController gameController, Board board, StressController stressController, PhoneUsers phoneUsers)
    {
        Instance = this;

        this.board = board;
        this.stressController = stressController;
        this.gameController = gameController;
        this.phoneUsers = phoneUsers;

        board.SubscribeToJabEvent(ConnectCall);
        stressController.OnStressPeak += EndCalls;

        LoadCallGroups();
    }

    private void LoadCallGroups()
    {
        callGroups = new List<CallGroup>();

        for (int i = 1; i < 5; i++)
        {
            var auxCallGroup = new CallGroup();
            auxCallGroup.callers = phoneUsers.users.FindAll(user => user.CallGroup == i).Select(u => u.Id).ToList();
            callGroups.Add(auxCallGroup);
        }
    }

    public void InitializeRound(IEnumerable<PhoneCall> roundPhoneCalls, float roundPhoneCallRate, bool endlessRound = false)
    {
        phoneCallRate = roundPhoneCallRate;
        phoneCalls.Clear();
        
        phoneCalls.AddRange(roundPhoneCalls);

        if (endlessRound)
        {
            gameController.StartCoroutine(GenerateCalls());
        }
    }

    public ConnectionResult ConnectCall(int receptorId)
    {
        if (phoneCalls.Count(x => !x.connected) > 0)
        {
            if (currentPhoneCall == null)
            {
                currentPhoneCall = phoneCalls.Take(phoneCallIndex).FirstOrDefault(x => !x.connected && x.caller == receptorId);

                if (currentPhoneCall != null)
                {
                    gameController.NotifyShowCaller(currentPhoneCall.caller);
                    return ConnectionResult.IS_CALLER;
                }
            }
            else if (currentPhoneCall.receiver == receptorId)
            {
                currentPhoneCall.connected = true;
                stressController.EndCall(phoneCalls.IndexOf(currentPhoneCall));
                gameController.CallCompleted(currentPhoneCall.caller, receptorId);
                currentPhoneCall = null;
                return ConnectionResult.IS_RECEIVER;
            }
            else
            {
                stressController.WrongConnection();

                if (receptorId == currentPhoneCall.caller)
                    return ConnectionResult.IS_SAME;

                if (phoneCalls.Take(phoneCallIndex).Any(x => !x.connected && x.caller == receptorId))
                    return ConnectionResult.IS_SAME;
                
                gameController.WrongConnection(receptorId);

                return ConnectionResult.IS_WRONG;
            }
        }

        return ConnectionResult.IS_VOID;
    }

    public void NotifyIncomingCall(PhoneCall phoneCall)
    {
        board.IncomingCall(phoneCall.caller);
        stressController.RegisterCall(phoneCalls.IndexOf(phoneCall));
    }

    public void EndCalls()
    {
        gameController.StopCoroutine(GenerateCalls());
        gameController.NotifyGameOver();
    }

    public void OnUpdate()
    {
        var text = string.Empty;
        foreach (var call in phoneCalls.Where(x => !x.connected))
        {
            text += string.Format("Call from{0} to {1}{2}", call.caller, call.receiver, Environment.NewLine);
        }

        if (phoneCallIndex < phoneCalls.Count)
        {
            phoneCallRateTimer -= Time.deltaTime;

            if (phoneCallRateTimer <= 0 && NextPhoneCallIsValid())
            {
                NotifyIncomingCall(phoneCalls[phoneCallIndex]);
                phoneCallRateTimer = phoneCallRate;
                
                phoneCallIndex++;
            } 
        }

        if (phoneCalls.Count(x => x.connected) == phoneCalls.Count)
        {
            RoundFinish();
        }
    }

    private void RoundFinish()
    {
        phoneCallIndex = 0;
        gameController.NotifyEndOfRound();
    }

    private bool NextPhoneCallIsValid()
    {
        var nextPhoneCall = phoneCalls[phoneCallIndex];

        return phoneCalls.Take(phoneCallIndex).All(x => x.connected || (x.caller != nextPhoneCall.caller && x.receiver != nextPhoneCall.caller && x.caller != nextPhoneCall.receiver && x.receiver != nextPhoneCall.receiver));
    }

    private IEnumerator GenerateCalls()
    {
        while (true)
        {
            if (phoneCalls.Count(x => !x.connected) < 8)
            {
                phoneCalls.Add(CreateRandomPhoneCall());
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private PhoneCall CreateRandomPhoneCall()
    {
        int callerId;
        int receiverId;
        
        do
        {
            callerId = UnityEngine.Random.Range(0, 16);
        } while (phoneCalls.Skip(phoneCalls.Count - 6).All(x => !x.connected && (x.caller == callerId || x.receiver == callerId)));


        var callGroup = callGroups.FirstOrDefault(x => x.callers.Contains(callerId));
        
        do
        {
            receiverId = callGroup.callers[UnityEngine.Random.Range(0, callGroup.callers.Count)];
        } while (callerId == receiverId);

        return new PhoneCall(callerId, receiverId);
    }

    public bool HasCurrentCall()
    {
        return Instance.currentPhoneCall != null ;
    }
}