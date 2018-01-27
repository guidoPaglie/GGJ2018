using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneUserView : MonoBehaviour {

    public Image MyImage;
    [HideInInspector]
    public bool inUse = false;
    [HideInInspector]
    public int id;

	public void SetUser (Sprite sprite, int id) 
    {
        this.id = id;

        inUse = true;
        MyImage.gameObject.SetActive(true);
        MyImage.sprite = sprite;
	}

    public void Reset()
    {
        id = -1;
        inUse = false;
        MyImage.gameObject.SetActive(false);
    }
}
