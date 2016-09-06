using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour {
    public List<Collectable> OwnedItems = new List<Collectable>();
    public GameObject ItemContainerPrototype;
    public float Offset = 150;

    private List<GameObject> _containers = new List<GameObject>();

    void Start () {
        if (!ItemContainerPrototype) {
            throw( new UnityException("I need a UI prototype where to put the inventory item icon. There is one on the prefabs! When created, drag it into the UI Prototype field!"));
        }
    }
    
    void Update () {
	
    }

    public void Refresh () {
        for (int i=0; i<_containers.Count; i++) {
            Destroy(_containers[i]);
        }
        _containers = new List<GameObject>();
        
        for (int i=0; i<OwnedItems.Count; i++) {
            GameObject container = Instantiate(ItemContainerPrototype);
            container.transform.SetParent(this.transform);
            container.SetActive(true);
            container.GetComponent<Image>().sprite = OwnedItems[i].Image;

            container.GetComponent<RectTransform>().anchoredPosition =
                ItemContainerPrototype.GetComponent<RectTransform>().anchoredPosition + 
                new Vector2(Offset * i, 0);

            _containers.Add(container);
        }
    }

    public void AddItem(Collectable item) {
        OwnedItems.Add(item);
        Refresh();
    }

    public void RemoveItem(Collectable item) {
        OwnedItems.Remove(item);
        Refresh();
    }
}
