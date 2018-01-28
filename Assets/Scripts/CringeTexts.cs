using System.Collections.Generic;
using UnityEngine;

public class CringeTexts  {

    public Dictionary<int, string> texts;

    public CringeTexts()
    {
        texts = new Dictionary<int, string>();

        var ta = Resources.Load<TextAsset>("cringe");
        var json = JSONObject.Create(ta.text);

        foreach(var userKey in json.keys)
        {
            var key = userKey.ToLower().Substring(userKey.Length - 1);
            texts[System.Convert.ToInt32(key) - 1] = json[userKey].str;
        }
    }
}
