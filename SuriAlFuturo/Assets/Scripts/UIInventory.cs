using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour {
    public List<Sprite> OwnedItems = new List<Sprite>();
    public GameObject ItemContainerPrototype;
    public float Offset = 150;
    public int ActiveItem = 0;

    private List<GameObject> _containers = new List<GameObject>();

    void Start () {
        if (!ItemContainerPrototype) {
            throw( new UnityException("I need a UI prototype where to put the inventory item icon. There is one on the prefabs! When created, drag it into the UI Prototype field!"));
        }
    }

    void Update () {
        if (_containers.Count > 0) {
            _containers[ActiveItem].GetComponent<UIContainer>().Active = false;

            if (Input.GetButtonDown("InventaryNext")) {
                ActiveItem = Mathf.Min(_containers.Count-1, ActiveItem+1);
            }

            if (Input.GetButtonDown("InventaryPrevious")) {
                ActiveItem = Mathf.Max(0, ActiveItem-1);
            }

            _containers[ActiveItem].GetComponent<UIContainer>().Active = true;
        }
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
            container.GetComponent<Image>().sprite = OwnedItems[i];

            container.GetComponent<RectTransform>().anchoredPosition =
                ItemContainerPrototype.GetComponent<RectTransform>().anchoredPosition +
                new Vector2(Offset * i, 0);

            _containers.Add(container);
        }

        if (OwnedItems.Count > 0) {
            ActiveItem = Mathf.Min(ActiveItem, _containers.Count-1);
            _containers[ActiveItem].GetComponent<UIContainer>().Active = true;
        }
    }

    public void AddItem(Sprite item) {
        OwnedItems.Add(item);
        Refresh();
    }

    public void RemoveItem(Sprite item) {
        OwnedItems.Remove(item);
        Refresh();
    }
}
