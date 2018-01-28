using System.Collections;
using UnityEngine;

public class MinigameController : MonoBehaviour {

    public GameObject dolphinPrefab;

    private float timer = 1.0f;

    private void Start()
    {
        StartCoroutine(DolphinsCreator());
    }
    private IEnumerator DolphinsCreator()
    {
        while (true)
        {
            Vector2 pos = Camera.main.ViewportToWorldPoint(new Vector2(1.0f, UnityEngine.Random.value));

            GameObject obj = Instantiate(dolphinPrefab, pos, Quaternion.identity);

            obj.GetComponent<Rigidbody2D>().AddForce(Vector3.left * 100, ForceMode2D.Force);
            yield return new WaitForSeconds(timer);
        }
    }
}
