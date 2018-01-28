using UnityEngine;
using System.Collections;

public class Jab : MonoBehaviour 
{
    public SpriteRenderer MyConnector;
    public GameObject MyLight;

    public float timeJabErrorOn = 1.0f;

    public int Id { get; private set; }
    
    public delegate ConnectionResult ReceptorSelected(int id);
    public event ReceptorSelected OnReceptorSelected;

    public void Initialize(int id)
    {
        Id = id;
    }

    public void IncomingCall()
	{
        MyLight.SetActive(true);
	}

    public void OnMouseDown()
    {
        if (OnReceptorSelected != null)
        {
            var selectionResponse = OnReceptorSelected(Id);

            if (selectionResponse == ConnectionResult.IS_CALLER || selectionResponse == ConnectionResult.IS_RECEIVER)
                MyConnector.color = GetColor();

            if (selectionResponse == ConnectionResult.IS_WRONG && selectionResponse != ConnectionResult.IS_SAME)
            {
                MyConnector.color = GetColor();
                StartCoroutine(WrongJab());
            }
        }
    }

    private IEnumerator WrongJab()
    {
        yield return new WaitForSeconds(timeJabErrorOn);
        MyConnector.color = Color.white;
    }

    public void Reset()
    {
        MyLight.SetActive(false);
        MyConnector.color = Color.white;
    }

    private Color GetColor()
    {
        if (Id == 0 || Id == 9 || Id == 11 || Id == 12 )
            return Color.green;
        if (Id == 1 || Id == 3 || Id == 7 || Id == 14)
            return Color.blue;
        if (Id == 5 || Id == 6 || Id == 13 || Id == 15)
            return Color.red;
        if (Id == 2 || Id == 4 || Id == 8 || Id == 10)
            return Color.yellow;

        return Color.black;
    }
}
