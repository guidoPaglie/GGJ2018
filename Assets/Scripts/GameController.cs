using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static int CurrentRound = 0;

    public Board Board;
    public GameplayScreen GameplayScreen;

    public GameObject PhoneUserPrefab;

    private TelephoneCentral _telephoneCentral;
    private PhoneUsers _phoneUsers;

	void Start () 
    {
        _phoneUsers = new PhoneUsers();

        _telephoneCentral = new TelephoneCentral(this, Board, _phoneUsers);

        _telephoneCentral.InitializeGame();
	}

    private void Update()
    {
        _telephoneCentral.OnUpdate();    
    }

    public void NotifyShowCaller(int currentCaller)
    {
        GameObject obj = Instantiate(PhoneUserPrefab);
        GameplayScreen.PositionateUser(obj.GetComponent<RectTransform>());

        obj.GetComponent<PhoneUserView>().Initialize(_phoneUsers.users.FirstOrDefault(user => user.Id == currentCaller).CharacterSprite);
    }

    public void NotifyEndOfRound()
    {
        
    }
}
