using UnityEngine;
using System.Collections.Generic;
using System;

public class PhoneUsers
{
    public List<PhoneUser> users;
    private List<List<int>> usersInRound;  // hay 4 listas, y en cada lista estan los ids de cada user

    public PhoneUsers () 
    {
        users = new List<PhoneUser>();
        usersInRound = new List<List<int>>();

        var ta = Resources.Load<TextAsset>("personas");
        var json = JSONObject.Create(ta.text);

        foreach(var userKey in json.keys)
        {
            var user = new PhoneUser(userKey, Convert.ToInt32(json[userKey]["id"].f));
            users.Add(user);

            for (int i = 0; i < 4; i++)
            {
                usersInRound.Add(new List<int>());

                var roundStr = string.Format("round{0}", i + 1);

                if (json[userKey].HasField(roundStr))
                {
                    usersInRound[i].Add(user.Id);

                    user.SetRoundJSON(i,json[userKey][roundStr]);
                }
            }
        }
	}
}