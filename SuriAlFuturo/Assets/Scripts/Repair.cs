using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SuriAlFuturo;

public class Repair : MonoBehaviour {
    public List<Collectable.Tag> OldItems;
    public GameObject RepairedItem;
    public TextAsset Message;
    public bool CantRepair = false;

    private Dialogue[] _digestedMessage;
    private CollectionSystem _collectionController;
    private RepairController _controller;

    void Start () {
        _collectionController = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<CollectionSystem>();
        _controller = _collectionController.GetComponent<RepairController>();
        _controller.RegisterRepair(this);
    }

    void OnDestroy () {
        _controller.Unregister(this);
    }

    public bool TryToRepair () {
        if (CanRepair()) {
            _controller.NotifyRepair(this);
            if (false == CantRepair) {
                SpawnRepairedItem();
                foreach (Collectable.Tag item in OldItems) {
                    _collectionController.RegisterAsGiven(item);
                }
            }
            return true;
        }
        return false;
    }

    private void _DigestMessage () {
        _digestedMessage =
            JsonUtility.FromJson<JsonDialogueData>(Message.text).Dialogues;
    }

    public void SpawnRepairedItem () {
        RepairedItem.SetActive(true);
    }

    public bool CanRepair () {
        foreach (Collectable.Tag oldItem in OldItems) {
            if (!_collectionController.HasItem(oldItem)) {
                return false;
            }
        }
        return true;
    }

    public Dialogue[] GetMessage () {
        if (_digestedMessage == null) {
            _DigestMessage();
        }

        return _digestedMessage;
    }
}
