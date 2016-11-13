using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour {
    public int ActiveItem = 0;
    public List<UIContainer> Slots;

    private Dictionary<Collectable.Tag, Sprite> _icons =
        new Dictionary<Collectable.Tag, Sprite>();

    private CollectionSystem _controller;

    void Start () {
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController)
            .GetComponent<CollectionSystem>();

        Sprite[] icons = Resources.LoadAll<Sprite>("Icons");
        for (int i=0; i < icons.Length; i++) {
            try {
                Collectable.Tag parsedTag =
                    (Collectable.Tag) System.Enum.Parse(typeof(Collectable.Tag),
                                                        icons[i].name);
                _icons[parsedTag] = icons[i];
            } catch {
                Debug.LogWarning(icons[i].name + " has no enumerable on Collectable.Tag :(");
            }
        }

        for (int i=0; i < Slots.Count; i++) {
            Slots[i].Index = i;
        }

        Refresh();
    }

    public void Refresh () {
        // ActiveItem will always be between 0 and inventory size-1.
        ActiveItem = Mathf.Max(Mathf.Min(ActiveItem, _controller.Inventory.Count-1), 0);

        for (int i=0; i < Slots.Count; i++) {
            if (i < _controller.Inventory.Count) {
                Slots[i].gameObject.SetActive(true);
                Slots[i].SetSprite(_icons[_controller.Inventory[i]]);
            } else {
                Slots[i].gameObject.SetActive(false);
            }
            Slots[i].Active = false;
        }

        Slots[ActiveItem].Active = true;
    }

    public Collectable.Tag GetActiveRequirement () {
        if (_controller.CountOwnedItems() == 0) {
            return Collectable.Tag.NONE;
        }
        return _controller.Inventory[ActiveItem];
    }

    public void SetActive (UIContainer slot) {
        ActiveItem = slot.Index;
        Refresh();
    }
}
