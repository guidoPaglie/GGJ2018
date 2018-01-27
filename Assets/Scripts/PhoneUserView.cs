using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneUserView : MonoBehaviour {

    public Image MyImage;

	public void Initialize (Sprite sprite) {
        MyImage.sprite = sprite;
	}
	
	void Update () {
		
	}
}
