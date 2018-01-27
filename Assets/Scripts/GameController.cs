using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour {

    public GameplayScreen GameplayScreen;

    public GameObject PhoneUser;
    public TelephoneCentral TelephoneCentral;

    private GameObject _currentCaller;
    private GameObject _currentReceiver;

    private PhoneUsers _phoneUsers;

	void Start () 
    {
        _phoneUsers = new PhoneUsers();	

        TelephoneCentral.Initialize();
	}

    public void NotifyShowCaller(int currentCaller)
    {
        GameObject obj = Instantiate(PhoneUser);
        GameplayScreen.PositionateUser(obj.GetComponent<RectTransform>());

        obj.GetComponent<PhoneUserView>().Initialize(_phoneUsers.users.FirstOrDefault(user => user.Id == currentCaller).CharacterSprite);
    }

    public void NotifyEndOfRound()
    {
        
    }
}
