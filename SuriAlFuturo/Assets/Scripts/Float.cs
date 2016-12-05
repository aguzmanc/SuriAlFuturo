using UnityEngine;
using System.Collections;

public class Float : MonoBehaviour {
    public GameObject Up;
    public GameObject Down;

    public int _direction = 1;
    public float _elapsedTime = 0;
    public float _time;

    void Start () {
        StartCoroutine("ChangeDirection");
        _direction = 1;
    }
    
    void Update () {
	if (_direction > 0) {
            GoUp();
        } else {
            GoDown();
        }
    }

    public void GoUp () {
        _elapsedTime += Time.deltaTime;
        transform.position =
            Vector3.Lerp(transform.position, Up.transform.position, _elapsedTime/_time);
    }

    public void GoDown () {
        _elapsedTime += Time.deltaTime;
        transform.position =
            Vector3.Lerp(transform.position, Down.transform.position, _elapsedTime/_time);
    }

    IEnumerator ChangeDirection () {
        _direction *= -1;
        _elapsedTime = 0;
        _time = Random.Range(.5f, 1f);
        yield return new WaitForSeconds(_time * .7f);
        StartCoroutine("ChangeDirection");
    }
}
