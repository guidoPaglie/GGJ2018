using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public GameObject JabPrefab;

	public int Rows;
	public int Cols;

    public float difX = 2.0f;
    public float difY = 1.20f;

	private List<Jab> board;

    private List<int> round1Participants = new List<int>() {0,5,4,1,11,15,10,14 };
    private List<int> round2Participants = new List<int>() { 0, 5, 4, 1, 11, 15, 10, 14,9,13,2,3 };

	void Awake () 
	{
		board = new List<Jab>();

        InstantiateJabs ();
	}

    void InstantiateJabs ()
	{
        int id = 0;

		for (int i = 0; i < Cols; i++) 
		{
			for (int j = 0; j < Rows; j++) 
			{
                Vector2 pos = new Vector2 (j * difX, i * -difY);
                Jab newJab = Instantiate (JabPrefab, pos, Quaternion.identity).GetComponent<Jab>();
                newJab.Initialize(id);
				board.Add(newJab);
                newJab.gameObject.transform.SetParent(this.transform, false);
                id++;
			}
		}	
	}

    public void SubscribeToJabEvent(Jab.ReceptorSelected callback)
    {
        foreach (var receptor in board)
        {
            receptor.OnReceptorSelected += callback;
        }
    }

    public void ResetBoard()
    {
        if (GameController._currentRound == 0)
            board.ForEach(jab => jab.IsEnable(round1Participants.Contains(jab.Id)));
        else if (GameController._currentRound == 1)
            board.ForEach(jab => jab.IsEnable(round2Participants.Contains(jab.Id)));
        else
              board.ForEach(jab => jab.IsEnable(true));
    }
    public void IncomingCall (int caller)
	{
        board.FirstOrDefault(receptor => receptor.Id == caller).IncomingCall();
	}

    public void CallCompleted(int caller, int receiver)
    {
        board.FirstOrDefault(receptor => receptor.Id == caller).Reset();
        board.FirstOrDefault(receptor => receptor.Id == receiver).Reset();
    }
}