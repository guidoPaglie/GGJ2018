using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneUser
{
	public string _name;
	public int _id;

	public Dictionary<int, JSONObject> roundTexts;

	public PhoneUser(string name, int id)
	{
		_name = name;
		_id = id;

		roundTexts = new Dictionary<int, JSONObject>();
	}

	public void SetRoundJSON(int i, JSONObject jSONObject)
	{
		roundTexts[i] = jSONObject;
	}
}
