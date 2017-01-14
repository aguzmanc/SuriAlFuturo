using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class Persist : MonoBehaviour {
    public Vector3 PersistenceKey;
    public bool Enabled = true;
    public bool ResetsOnGameStart = false;

    private float _disabledOn = 0;
    private bool _isInitialized = false;

    private PersistenceController _controller;

    void Start () {
        PersistenceKey = transform.position;

        _controller = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<PersistenceController>();

        if (_controller.HasSavedData(this)) {
            if ((ResetsOnGameStart &&
                 _controller.HasBeenLoaded.ContainsKey(this.PersistenceKey)) ||
                !ResetsOnGameStart) {
                _controller.Load(this);
            } else {
                _controller.NotifyLoad(this);
            }
        }

        gameObject.SetActive(Enabled);

        _isInitialized = true;
    }

    void OnEnable () {
        if (_isInitialized) {
            Enabled = true;
        }
    }

    void OnDisable () {
        _disabledOn = Time.time;
        Enabled = false;
        _controller.Save(this);
    }

    void OnDestroy () {
        // cuando se desactiva al destruirse, los eventos ocurren en el mismo frame.
        if (_disabledOn == Time.time) {
            Enabled = true;
        }
        _controller.Save(this);
    }

    public void Load (PersistedGameObject persisted) {
        transform.position = persisted.Position;
        transform.rotation = persisted.Rotation;
        transform.localScale = persisted.Scale;
        Enabled = persisted.Enabled;
    }

    public PersistedGameObject GetPersistenceObject () {
        return new PersistedGameObject(transform.position,
                                       transform.rotation,
                                       transform.localScale,
                                       Enabled);
    }
}
