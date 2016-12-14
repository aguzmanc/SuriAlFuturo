using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class Persist : MonoBehaviour {
    public Vector3 PersistenceKey;
    public bool Enabled = false;

    private PersistenceController _controller;

    void Start () {
        PersistenceKey = transform.position;

        _controller = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<PersistenceController>();

        if (_controller.HasSavedData(this)) {
            _controller.Load(this);
        }
    }

    void Update () {
        Enabled = gameObject.activeSelf;
    }

    void OnDisable () {
        Enabled = false;
        _controller.Save(this);
    }

    void OnDestroy () {
        _controller.Save(this);
    }

    public void Load (PersistedGameObject persisted) {
        transform.position = persisted.Position;
        transform.rotation = persisted.Rotation;
        transform.localScale = persisted.Scale;
        gameObject.SetActive(persisted.Enabled);
    }

    public PersistedGameObject GetPersistenceObject () {
        return new PersistedGameObject(transform.position,
                                       transform.rotation,
                                       transform.localScale,
                                       Enabled);
    }
}
