using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {
    public Sprite Image;

    private CollectionSystem _controller;

    void Start () {

        if (!GameObject.FindGameObjectWithTag(Tag.GameController)) {
            throw(new UnityException("Collectable items need a CollectionSystem component in the GameController"));
        }

        _controller = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<CollectionSystem>();
        if (!_controller) {
            throw (new UnityException("Collectable items need a CollectionSystem component in the GameController"));
        }

        if (!Image) {
            throw (new UnityException("I need an image as an icon to show on the inventory!"));
        }

        _controller.RegisterCollectable(this);
    }
	
    void Update () {
	
    }

    void OnTriggerEnter(Collider player) {
        Debug.Log(player.name);
        _controller.IngameCollector.Take(this);
        this.gameObject.SetActive(false);
    }
}
