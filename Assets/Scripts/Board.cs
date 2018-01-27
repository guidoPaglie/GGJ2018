using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {

	public GameObject Connector;

	public int Rows;
	public int Cols;

	private RectTransform myRT;
	private GameObject[,] board;

	void Start () 
	{
		board = new GameObject[Rows, Cols];

		for (int i = 0; i < Rows; i++) 
		{
			for (int j = 0; j < Cols; j++) 
			{
				GameObject obj = Instantiate (Connector, Vector3.zero, Quaternion.identity, this.transform);
				board [i, j] = obj;
			}
		}
	}
	
	void Update ()
	{
		
	}
}
