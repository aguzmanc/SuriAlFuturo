using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

    // spanish!!
    [System.Serializable]
    public enum Tag { // implicitly static :O
        chatarra,
        flor,
        holograma,
        patineta,
        patineta_voladora,
        patineta_arreglada,
        patineta_joaquin,
        pollera,
        turbina,
        tmt,
        ydroid,
        camara,
        motor_ionico,
        rueda,
        prueba,
        maleta_verde_arruinada,
        maleta_verde_arreglada,
        dron,
        rueda1,
        rueda2,
        rueda3,
        rueda4,
        motor,
        energia,
        pala,
        cosechadora,
        dron_completo,
        llave,
        notas,

        NONE
    };

    public Vector3 PersistenceKey;
    public Tag Name = Tag.NONE;
    public bool IsTaken;
    public GameObject EnablesSomething;

    private CollectionSystem _controller;

    void Start () {
        PersistenceKey = transform.position;
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController)
            .GetComponent<CollectionSystem>();
        _controller.Load(this);
    }

    void Update () {
        if (EnablesSomething != null) {
            EnablesSomething.SetActive(true);
        }

        if (IsTaken) {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter (Collider player) {
        IsTaken = true;
        _controller.RegisterAsTaken(this);

        GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController).GetComponent<SFXController>().PlayPickItem();
    }

    void OnDestroy () {
        _controller.Save(this);
    }

    void OnDisable () {
        _controller.Save(this);
    }

    public PersistedCollectable GetPersistedObject () {
        return new PersistedCollectable(IsTaken, Name);
    }

    public void LoadPersistedObject (PersistedCollectable persisted) {
        this.IsTaken = persisted.IsTaken;
        this.Name = persisted.Name;
    }
}
