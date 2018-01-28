using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverScreen : MonoBehaviour {

    public List<GameObject> screens;
    public AudioSource audioSource;

    public AudioClip gameoverClip;

    public float[] timers;
    private int currentScreen = 0;

	void Start () {
        audioSource.Play();

        StartCoroutine(StartAnimation());		
	}

    private IEnumerator StartAnimation()
    {
        while(currentScreen < 2)
        {
            yield return new WaitForSeconds(timers[currentScreen]);
            screens[currentScreen].SetActive(false);
            currentScreen++;
            screens[currentScreen].SetActive(true);

            if (currentScreen == 1)
            {
                audioSource.clip = gameoverClip;
                audioSource.Play();
            }
        }
    }
}
