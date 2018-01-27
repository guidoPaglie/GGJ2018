using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject PhoneUser;

    private PhoneUsersLoader phoneUserLoader;

	void Start () 
    {
        phoneUserLoader = new PhoneUsersLoader();	
	}
	
	void Update () 
    {
		
	}
}
