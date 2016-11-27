using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class WaterTap : MonoBehaviour {
    public Vector3 PersistenceKey;

    public bool IsOn {
        get {
            return _isOn;
        }
    }

    private ParticleSystem _fountain;
    public bool _isOn;
    public bool _canInteract;
    private bool _interactionTriggered;

    private TimeTravelController _timeTravelController;
    private TapController _tapController;
    private GameController _gameController;

    void Start () {
        _isOn = true;
        PersistenceKey = this.transform.position;

        _fountain = GetComponentInChildren<ParticleSystem>();
        _gameController = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<GameController>();
        _timeTravelController = _gameController.GetComponent<TimeTravelController>();
        _tapController = _gameController.GetComponent<TapController>();

        _tapController.Taps.Add(this);
        _tapController.Load(this);
    }
    
    void Update () {
        if (Input.GetButtonDown("Interact") || _interactionTriggered) {
            _interactionTriggered = false;
            if (_canInteract) {
                ToggleFountain(!_isOn);
            }
        }
    }

    void OnTriggerEnter (Collider c) {
        _gameController.CanUseTap = _canInteract = true;
    }

    void OnTriggerExit (Collider c) {
        _gameController.CanUseTap = _canInteract = false;
    }

    void OnDestroy () {
        _tapController.Save(this);
        _tapController.Taps.Remove(this);
    }

    public void ToggleFountain (bool value){
        _isOn = value;

        if(_isOn) {
            _fountain.Play();
        } else {
            _fountain.Stop();
        }
    }

    public void TriggerInteraction () {
        _interactionTriggered = true;
    }

    #region PERSISTENCE
    public bool GetPersistedObject () {
        return _isOn;
    }

    public void Load (bool isOn) {
        ToggleFountain(isOn);
    }
    #endregion
}
