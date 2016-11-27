using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

    // spanish!!
    public enum Tag { // implicitly static :O
        chatarra,
        flor,
        holograma,
        patineta_voladora,
        patineta_arreglada,
        patineta_joaquin,
        pollera,
        turbina,
        tmt,

        NONE
    };

    public Vector3 PersistenceKey;
    public Tag Name = Tag.NONE;
    public bool IsTaken;

    private CollectionSystem _controller;

    void Start () {
        PersistenceKey = transform.position;
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController)
            .GetComponent<CollectionSystem>();
        _controller.Load(this);
    }

    void Update () {
        if (IsTaken) {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter (Collider player) {
        IsTaken = true;
        _controller.RegisterAsTaken(this);
    }

    void OnDestroy () {
        _controller.Save(this);
    }

    void OnDisable () {
        _controller.Save(this);
    }

    public PersistedCollectable GetPersistedObject () {
        return new PersistedCollectable(IsTaken);
    }

    public void LoadPersistedObject (PersistedCollectable persisted) {
        this.IsTaken = persisted.IsTaken;
    }
}
