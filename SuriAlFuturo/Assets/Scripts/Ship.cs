using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
    public SphereCollider DockDetector;

    void Start () {
        StartCoroutine("DontStuckOnStart");
    }

    IEnumerator DontStuckOnStart () {
        yield return new WaitForSeconds(1);
        EnlargeCollider();
    }

    public void EnlargeCollider () {
        DockDetector.radius *= 2f;
    }

    public void ShrinkCollider () {
        DockDetector.radius /= 2f;
    }
}
