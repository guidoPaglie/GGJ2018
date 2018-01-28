using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneCallsHarcode {

    public List<List<PhoneCall>> phoneCalls;

    public PhoneCallsHarcode()
    {
        phoneCalls = new List<List<PhoneCall>>();

        phoneCalls.Add(new List<PhoneCall>());
        AddPhoneCall(0, 4, 10, 1, 14, 5, 15, 0, 11);

        phoneCalls.Add(new List<PhoneCall>());
        AddPhoneCall(1, 15, 13, 9, 0, 2, 4, 3, 1, 11, 9, 14, 3, 10, 2, 5, 13);

        phoneCalls.Add(new List<PhoneCall>());
        AddPhoneCall(2, 4, 8, 6, 5, 11, 12 , 7 , 1, 2 , 10 , 0 , 9 , 13 , 6 , 14 , 3, 9 , 11 , 15 ,6 , 3 , 7 , 8 , 10);

        phoneCalls.Add(new List<PhoneCall>());
        AddPhoneCall(3, 5 , 13 ,2 , 8 , 12 , 9 , 1 ,3 , 15 ,5 , 11 , 12, 10 , 4, 3 ,14 ,9 , 11, 13 , 15, 14, 1, 4 , 2 , 7 , 3, 6 ,13 ,0 , 11,8 , 10);
    }

    private void AddPhoneCall(int index, params int[] calls)
    {
        for (int i = 0; i < calls.Length; i+=2)
        {
            phoneCalls[index].Add(new PhoneCall(calls[i], calls[i+1]));
        }
    }
}
