using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressController : MonoBehaviour
{
    public float maxStressLevel;

    public Transform needle;

    public float waitTimeToIncreaseStress;
    public float stressOnDelay;
    public float stressOnError;

    public float angleMin;
    public float angleMax;
    
    private float stressLevel;
    private float currentMaxStressLevel;
    private Dictionary<int, PendingCall> pendingCalls = new Dictionary<int, PendingCall>();

    public delegate void StressPeak();
    public event StressPeak OnStressPeak;

    public void SetupStresslevels(float startLevel, float maxLevel)
    {
        stressLevel = startLevel;
        currentMaxStressLevel = maxLevel;
    }

    public void RegisterCall(int id)
    {
        pendingCalls.Add(id, new PendingCall(DateTime.UtcNow));
        //Debug.Log("call registered" + id);
    }

    public void EndCall(int id)
    {
        pendingCalls.Remove(id);
        //Debug.Log("call ended" + id);
    }

    public void WrongConnection()
    {
        stressLevel += stressOnError;
        Debug.Log("wrong connection");
    }

    private void Update()
    {
        var now = DateTime.UtcNow;

        foreach (var pendingCall in pendingCalls)
        {
            if ((now - pendingCall.Value.startTime).TotalSeconds > (waitTimeToIncreaseStress * pendingCall.Value.delay))
            {
                stressLevel += stressOnDelay;
                pendingCall.Value.delay++;
                //Debug.Log("Delay" + pendingCall.Key);
            }
        }

        if (stressLevel >= currentMaxStressLevel)
        {
            stressLevel = currentMaxStressLevel;
        }

        SetNeedleRotation();

        if (stressLevel >= maxStressLevel && OnStressPeak != null)
        {
            OnStressPeak();
        }
    }

    private void SetNeedleRotation()
    {
        var t = Mathf.InverseLerp(0, maxStressLevel, stressLevel);

        needle.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(angleMin, angleMax, t));
    }

    private class PendingCall
    {
        public DateTime startTime;
        public int delay;

        public PendingCall(DateTime start)
        {
            startTime = start;
        }
    }
}
