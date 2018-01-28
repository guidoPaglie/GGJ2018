using UnityEngine;
using System.Collections.Generic;

public class PhoneUser
{
    public string Name;
	public int Id;
    public Sprite CharacterSprite;
    public int CallGroup;

	public Dictionary<int, JSONObject> roundTexts;

    public PhoneUser (string name, int id, int callGroup)
	{
		Name = name;
		Id = id;
        CallGroup = callGroup;
        //Debug.Log(name + " " + id + " " + callGroup);

		roundTexts = new Dictionary<int, JSONObject>();

        CharacterSprite = Resources.Load<Sprite>("Characters/" + name);
        //Debug.Log(CharacterSprite);
	}

	public void SetRoundJSON(int i, JSONObject jSONObject)
	{
		roundTexts[i] = jSONObject;
	}
}
