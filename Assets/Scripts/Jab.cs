using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Jab : MonoBehaviour 
{
    public SpriteRenderer MyLight;
    public SpriteRenderer Cable;

    public List<Sprite> GroupColors;

    public float timeJabErrorOn = 1.0f;

    public int Id { get; private set; }
    
    public delegate ConnectionResult ReceptorSelected(int id);
    public event ReceptorSelected OnReceptorSelected;

    public void Initialize(int id)
    {
        Id = id;

        if (transform.position.x > 0 )
        {
            Cable.sprite = Resources.Load<Sprite>("CableDerecho");
            Vector3 newPos = Cable.transform.localPosition;
            newPos.x = 0.35f;
            Cable.transform.localPosition = newPos;
        }
    }

    public void IncomingCall()
	{
        MyLight.sprite = GetSpriteColor();
    }

    public void IsEnable(bool enable)
    {
        GetComponent<BoxCollider2D>().enabled = enable;
    }

    public void OnMouseDown()
    {
        if (OnReceptorSelected != null)
        {
            var selectionResponse = OnReceptorSelected(Id);

            if (selectionResponse == ConnectionResult.IS_CALLER || selectionResponse == ConnectionResult.IS_RECEIVER)
            {
                MyLight.sprite = GetSpriteColor();
            }

            if (selectionResponse == ConnectionResult.IS_WRONG && selectionResponse != ConnectionResult.IS_SAME)
            {
                MyLight.sprite = GetSpriteColor();

                StartCoroutine(WrongJab());
            }
        }
    }

    private IEnumerator WrongJab()
    {
        yield return new WaitForSeconds(timeJabErrorOn);

        MyLight.sprite = null;
    }

    public void Reset()
    {
        MyLight.sprite = null;
        HideCable();
    }

    private Sprite GetSpriteColor()
    {
        if (Id == 0 || Id == 9 || Id == 11 || Id == 12 )
            return GroupColors[0];
        if (Id == 5 || Id == 6 || Id == 13 || Id == 15)
            return GroupColors[1];
        if (Id == 2 || Id == 4 || Id == 8 || Id == 10)
            return GroupColors[2];
        if (Id == 1 || Id == 3 || Id == 7 || Id == 14)
            return GroupColors[3];

        return GroupColors[0];
    }

    public void ShowCable()
    {
        Cable.gameObject.SetActive(true);
    }

    public void HideCable()
    {
        Cable.gameObject.SetActive(false);
    }
}
