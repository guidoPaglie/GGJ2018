using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoraImages : MonoBehaviour {

    public List<Sprite> noraSprites;
    public SpriteRenderer noraSR;

	public void ChangeImage (int index) 
    {
        if (index < noraSprites.Count)
        {
            noraSR.sprite = noraSprites[index];
        }
	}
}
