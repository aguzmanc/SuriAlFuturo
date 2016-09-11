using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {
    public GameObject DockedShip;
    public GameObject Suri;
    public GameObject DisembarkPlace;
    public GameObject Indicator;
    public bool IsSuriAtPort;


    private FollowCamera _camera;

    void Start () {
        Suri = GameObject.FindGameObjectWithTag(Tag.Player);
        _camera = GameObject.FindGameObjectWithTag(Tag.MainCamera).transform.parent.
            gameObject.GetComponent<FollowCamera>();
    }
    
    void Update () {
        Indicator.SetActive(false);

        if ((DockedShip != null && Suri.activeSelf && IsSuriAtPort) ||
            (DockedShip != null && !Suri.activeSelf)) {
            Indicator.SetActive(true);
        }

        if (DockedShip != null && Input.GetButtonDown("LoadUnloadToShip")) {
            if (Suri.activeSelf && IsSuriAtPort) {
                Suri.SetActive(false);
                DockedShip.GetComponent<CharacterMovement>().IsControlledByPlayer = true;
                _camera.Target = DockedShip;
            } else if (!Suri.activeSelf) {
                Suri.transform.position = DisembarkPlace.transform.position;
                Debug.Log(Suri.transform.position);
                Suri.SetActive(true);
                DockedShip.GetComponent<CharacterMovement>().IsControlledByPlayer = false;
                _camera.Target = Suri;
            }
        }
    }

    void OnTriggerEnter (Collider c) {
        GameObject parent = c.transform.parent.gameObject;

        if (parent.CompareTag(Tag.Ship)) {
            DockedShip = parent;
        } else {
            IsSuriAtPort = true;
        }
    }

    void OnTriggerExit (Collider c) {
        GameObject parent = c.transform.parent.gameObject;

        if (parent.CompareTag(Tag.Ship)) {
            DockedShip = null;
        } else {
            IsSuriAtPort = false;
        }
    }
}
