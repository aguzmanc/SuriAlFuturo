using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {
    public GameObject DockedShip;
    public GameObject Suri;
    public GameObject DisembarkPlace;
    public GameObject Indicator;
    private GameController _gameController;
    public bool IsSuriAtPort;


    private FollowCamera _camera;

    void Start () {
        _gameController = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController)
            .GetComponent<GameController>();
        Suri = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.Player);
        _camera = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.MainCamera).transform.parent.
            gameObject.GetComponent<FollowCamera>();
    }
    
    void Update () {
        if ((DockedShip != null && Suri.activeSelf && IsSuriAtPort) ||
            (DockedShip != null && !Suri.activeSelf)) {
            Indicator.SetActive(true);
        } else {
            Indicator.SetActive(false);
        }

        if (DockedShip != null && Input.GetButtonDown("LoadUnloadToShip")) {
            if (Suri.activeSelf && IsSuriAtPort) {
                Suri.SetActive(false);
                DockedShip.GetComponent<CharacterMovement>().IsControlledByPlayer = true;
                _camera.Target = DockedShip;
                _gameController.SetDrivingBoat(true);
            } else if (!Suri.activeSelf) {
                Suri.transform.position = DisembarkPlace.transform.position;
                Suri.SetActive(true);
                DockedShip.GetComponent<CharacterMovement>().IsControlledByPlayer = false;
                _camera.Target = Suri;
                _gameController.SetDrivingBoat(false);
            }
        }
    }

    void OnTriggerEnter (Collider c) {
        GameObject parent = c.transform.parent.gameObject;

        if (parent.CompareTag(SuriAlFuturo.Tag.Ship)) {
            DockedShip = parent;
        } else {
            IsSuriAtPort = true;
        }
    }

    void OnTriggerExit (Collider c) {
        GameObject parent = c.transform.parent.gameObject;

        if (parent.CompareTag(SuriAlFuturo.Tag.Ship)) {
            DockedShip = null;
        } else {
            IsSuriAtPort = false;
        }
    }
}
