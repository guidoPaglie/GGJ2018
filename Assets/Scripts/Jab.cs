using UnityEngine;

public class Jab : MonoBehaviour 
{
    public SpriteRenderer MyConnector;
    public GameObject MyLight;

    public int Id { get; private set; }

    private bool _isCaller;

    public delegate bool ReceptorSelected(int id);
    public event ReceptorSelected OnReceptorSelected;

    public void Initialize(int id)
    {
        Id = id;
    }

    public void IncomingCall()
	{
        _isCaller = true;
        MyLight.SetActive(true);
	}

    public void OnMouseDown()
    {
        if (OnReceptorSelected != null)
        {
            var selectionResponse = OnReceptorSelected(Id);

            if (selectionResponse && _isCaller)
            {
                MyConnector.color = Color.red;
            }
        }
    }

    public void Reset()
    {
        _isCaller = false;
        MyLight.SetActive(false);
        MyConnector.color = Color.white;
    }
}
