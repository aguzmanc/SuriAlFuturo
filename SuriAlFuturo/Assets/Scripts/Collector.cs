
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collector : MonoBehaviour {
    public Dictionary<Sprite, bool> Inventory = new Dictionary<Sprite, bool>();

    private CollectionSystem _controller;
    
    void Start () {
        _controller = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<CollectionSystem>();
    }
    
    void Update () {
    }

    public void RegisterCollectable (Collectable collectable) {
        Inventory.Add(collectable.Image, false);
    }

    public void Take (Collectable collectable) {
        Inventory[collectable.Image] = true;
        _controller.UIController.AddItem(collectable.Image);
        _controller.TakenStuff.Add(collectable);
    }

    public void Give (Collectable collectable) {
        Inventory[collectable.Image] = false;
        _controller.UIController.RemoveItem(collectable.Image);
    }

}
