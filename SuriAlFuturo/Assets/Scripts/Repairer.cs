using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class Repairer : MonoBehaviour {
    public Talkable TheTalkable;
    public RepairController _controller;

    void Start () {
        _controller = GameObject.FindGameObjectWithTag(Tag.GameController).
            GetComponent<RepairController>();
        _controller.RegisterTalkable(TheTalkable);
    }

    void OnTriggerEnter () {
        _controller.RepairButton.SetActive(true);
    }

    void OnTriggerExit () {
        _controller.RepairButton.SetActive(false);
    }

    void OnDestroy() {
        _controller.RepairButton.SetActive(false);
    }
    
}
