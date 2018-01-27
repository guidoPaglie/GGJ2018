using UnityEngine;
using System;

public class Receptor : MonoBehaviour 
{
    public SpriteRenderer MyConnector;
    public GameObject MyLight;

    public int Id { get; private set; }

    private bool _isCaller;
    private bool _isConnected;
    
    public event Action<int> OnReceptorSelected = delegate { };

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
        if (_isCaller)
        {
            if (!_isConnected)
            {
				_isConnected = true;    
                MyConnector.color = Color.red;
            }
        }

        OnReceptorSelected(Id);
    }

    public void Reset()
    {
        _isCaller = false;
        _isConnected = false;
        MyLight.SetActive(false);
        MyConnector.color = Color.white;
    }
}
