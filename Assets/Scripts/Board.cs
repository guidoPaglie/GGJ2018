using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {

	public GameObject Connector;

	public int Rows;
	public int Cols;

	public float offsetBetweenConnectors;

	private RectTransform myRT;
	private GameObject[,] board;

	void Start () 
	{
		board = new GameObject[Rows, Cols];

		InstantiateViewConnectors ();
	}

	void InstantiateViewConnectors ()
	{
		for (int i = 0; i < Cols; i++) 
		{
			for (int j = 0; j < Rows; j++) 
			{
				Vector2 pos = new Vector2 (i * offsetBetweenConnectors - Cols/2, j * offsetBetweenConnectors - Rows/2);
				GameObject obj = Instantiate (Connector, pos, Quaternion.identity, this.transform);
				board [j, i] = obj;
			}
		}	
	}
}
