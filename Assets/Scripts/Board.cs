using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public GameObject JabPrefab;

	public int Rows;
	public int Cols;

	private List<Jab> board;

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
				Vector2 pos = new Vector2 (j * 2.0f, i * -1.20f);
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