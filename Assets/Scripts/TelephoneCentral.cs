using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TelephoneCentral : MonoBehaviour
{
    public Board board;

    private List<PhoneCall> phoneCalls = new List<PhoneCall>();
    private float phoneCallRate;
    private float phoneCallRateTimer = 1;

    private int phoneCallIndex;
    private PhoneCall currentPhoneCall;

    public void InitializeRound(IEnumerable<PhoneCall> roundPhoneCalls, float roundPhoneCallRate, bool endlessRound = false)
    {
        phoneCallRate = roundPhoneCallRate;
        phoneCalls.Clear();
        
        phoneCalls.AddRange(roundPhoneCalls);
        StartCoroutine(GenerateCalls());
    }

    public bool ConnectCall(int receptorId)
    {
        if (phoneCalls.Count(x => !x.connected) > 0)
        {
            if (currentPhoneCall == null)
            {
                currentPhoneCall = phoneCalls.Take(phoneCallIndex).FirstOrDefault(x => !x.connected && x.caller == receptorId);
                //someone.NotifyShowCaller();
                return true;
            }
            else if (currentPhoneCall.receiver == receptorId)
            {
                currentPhoneCall.connected = true;
                board.CallCompleted(currentPhoneCall.caller, currentPhoneCall.receiver);
                currentPhoneCall = null;
                return true;
            }
        }

        return false;
    }

    public void NotifyIncomingCall(PhoneCall phoneCall)
    {
        board.IncomingCall(phoneCall.caller);
    }

    private void Start()
    {
        board.SubscribeToReceptorEvent(ConnectCall);

        InitializeRound(new List<PhoneCall> { new PhoneCall(1, 2), new PhoneCall(3, 4) }, 2, true);
    }

    private void Update()
    {
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
            //someone.NotifyEndOfRound();
        }
    }

    private bool NextPhoneCallIsValid()
    {
        var nextPhoneCall = phoneCalls[phoneCallIndex];

        return phoneCalls.Take(phoneCallIndex).All(x => x.connected || (x.caller != nextPhoneCall.caller && x.receiver != nextPhoneCall.caller && x.caller != nextPhoneCall.receiver && x.receiver != nextPhoneCall.receiver));
    }

    private IEnumerator GenerateCalls()
    {
        // NOTE: Test
        phoneCalls.Add(CreateRandomPhoneCall());
        phoneCalls.Add(CreateRandomPhoneCall());
        phoneCalls.Add(CreateRandomPhoneCall());
        phoneCalls.Add(CreateRandomPhoneCall());
        phoneCalls.Add(CreateRandomPhoneCall());

        while (true)
        {
            phoneCalls.Add(CreateRandomPhoneCall());

            yield return new WaitForSeconds(0.5f);
        }
    }

    private static PhoneCall CreateRandomPhoneCall()
    {
        var callerId = UnityEngine.Random.Range(0, 16);
        var receiverId = UnityEngine.Random.Range(0, 16);

        // TODO: Get the correct receiverId based on callerId

        return new PhoneCall(callerId, callerId == receiverId ? receiverId + 1 : receiverId);
    }
}