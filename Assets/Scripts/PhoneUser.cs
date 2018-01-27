using UnityEngine;
using System.Collections.Generic;

public class PhoneUser
{
    public string Name;
	public int Id;
    public Sprite CharacterSprite;

	private Dictionary<int, JSONObject> roundTexts;

    public PhoneUser (string name, int id)
	{
		Name = name;
		Id = id;
        Debug.Log(name + " " + id);

		roundTexts = new Dictionary<int, JSONObject>();

        CharacterSprite = Resources.Load<Sprite>("Characters/" + name);
        //Debug.Log(CharacterSprite);
	}

	public void SetRoundJSON(int i, JSONObject jSONObject)
	{
		roundTexts[i] = jSONObject;
	}
}
