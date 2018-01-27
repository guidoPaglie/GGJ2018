using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public Board board;

	void Start () 
	{
        StartCoroutine(NotifyIncomingCall());
	}

    private IEnumerator NotifyIncomingCall()
    {
        yield return new WaitForSeconds(0.2f);

        var caller = Random.Range(0, 10);
        var receiver = 12;

        board.IncomingCall(caller, receiver);
    }
}
