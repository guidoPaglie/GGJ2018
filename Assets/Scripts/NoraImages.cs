using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoraImages : MonoBehaviour {

    public List<Sprite> noraSprites;
    public SpriteRenderer noraSR;

    private int lastRound = 0;

	void Update () 
    {
        if (GameController._currentRound != lastRound)
        {
            if (lastRound < noraSprites.Count)
            {
                noraSR.sprite = noraSprites[lastRound];
                lastRound++;    
            }
        }
	}
}
