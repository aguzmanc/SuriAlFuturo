using UnityEngine;
using System.Collections;
using SuriAlFuturo;

public class Follow : MonoBehaviour {
    private GameController _controller;

    void Start () {
        _controller = GameObject.FindGameObjectWithTag(Tag.GameController)
            .GetComponent<GameController>();
    }
    
    void Update () {
        transform.position = _controller.ControlledCharacter.transform.position;
        transform.forward = -_controller.ControlledCharacter.transform.forward;
    }
}
