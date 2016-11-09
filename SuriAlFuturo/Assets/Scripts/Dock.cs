using UnityEngine;
using System.Collections;

public class Dock : MonoBehaviour {
    public GameObject DockedShip;
    public GameObject Suri;
    public GameObject DisembarkPlace;
    public GameObject Indicator;
    public bool IsSuriAtPort;

    private GameController _gameController;
    private DockController _dockController;
    private bool _isInterfaceInteractionTriggered;
    private FollowCamera _camera;

    void Start () {
        _isInterfaceInteractionTriggered = false;

        _gameController = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController)
            .GetComponent<GameController>();
        _dockController = _gameController.GetComponent<DockController>();
        _dockController.Docks.Add(this);
        Suri = _gameController.GetComponent<GameController>().Suri;
        _camera = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.MainCamera).transform.parent.
            gameObject.GetComponent<FollowCamera>();
    }
    
    void Update () {
        Indicator.SetActive(CanEmbark() || CanDisembark());
        if (Indicator.activeSelf) {
            _dockController.CanUseDock = true;
        }

        if ((Input.GetButtonDown("LoadUnloadToShip") ||
             _isInterfaceInteractionTriggered)) {
            _isInterfaceInteractionTriggered = false;
            if (CanEmbark()) {
                Embark();
            } else if (CanDisembark()) {
                Disembark();
            }
        }
    }

    void OnDestroy () {
        _dockController.Docks.Remove(this);
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

        _dockController.CanUseDock = false;
    }

    public bool CanEmbark () {
        return DockedShip != null && Suri.activeSelf && IsSuriAtPort;
    }

    public bool CanDisembark () {
        return DockedShip != null && !Suri.activeSelf;
    }

    public void Embark () {
        Suri.SetActive(false);
        DockedShip.GetComponent<CharacterMovement>().IsControlledByPlayer = true;
        _camera.Target = DockedShip;
        _gameController.SetDrivingBoat(true);
    }

    public void Disembark () {
        Suri.transform.position = DisembarkPlace.transform.position;
        Suri.SetActive(true);
        DockedShip.GetComponent<CharacterMovement>().IsControlledByPlayer = false;
        _camera.Target = Suri;
        _gameController.SetDrivingBoat(false);
    }

    public void TriggerInterfaceInteraction () {
        _isInterfaceInteractionTriggered = true;
    }
}
