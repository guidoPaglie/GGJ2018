using UnityEngine;

public class Receptor : MonoBehaviour 
{
    public SpriteRenderer MyConnector;
    public SpriteRenderer MyLight;
    public int Id { get; private set; }

    private bool _isCaller;
    private bool _isConnected;

    public void Initialize(int id)
    {
        Id = id;
    }

    public void TurnLightOn()
	{
        _isCaller = true;
		SetLightVisibility(true);
	}

	public void TurnLightOff()
	{
        _isCaller = false;
        _isConnected = false;
        MyConnector.color = Color.white;
		SetLightVisibility(false);
	}

	private void SetLightVisibility(bool visible)
	{
        MyLight.gameObject.SetActive(visible);
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
        else
        {
            
        }
    }
}
