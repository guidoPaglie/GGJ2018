using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public GameObject Receptor;

	public int Rows;
	public int Cols;

	private List<Receptor> board;

	void Awake () 
	{
		board = new List<Receptor>();

        InstantiateReceptors ();
	}

    void InstantiateReceptors ()
	{
        int id = 0;

		for (int i = 0; i < Cols; i++) 
		{
			for (int j = 0; j < Rows; j++) 
			{
				Vector2 pos = new Vector2 (j * 1, i * 1);
				Receptor newReceptor = Instantiate (Receptor, pos, Quaternion.identity, this.transform).GetComponent<Receptor>();
                newReceptor.Initialize(id);
				board.Add(newReceptor);

                id++;
			}
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