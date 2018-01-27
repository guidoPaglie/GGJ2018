using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TelephoneCentral : MonoBehaviour
{
    public Board board;
    public List<CallGroup> callGroups; 

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

        if (endlessRound)
        {
            StartCoroutine("GenerateCalls"); 
        }
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

    public void EndCalls()
    {
        StopCoroutine("GenerateCalls");
    }

    private void Start()
    {
        board.SubscribeToReceptorEvent(ConnectCall);

        InitializeRound(new List<PhoneCall> { new PhoneCall(1, 2), new PhoneCall(3, 4) }, 2, true);
    }

    private void Update()
    {
        var text = string.Empty;
        foreach (var call in phoneCalls.Where(x => !x.connected))
        {
            text += string.Format("Call from{0} to {1}{2}", call.caller, call.receiver, Environment.NewLine);
        }

        Debug.Log(text);

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
}