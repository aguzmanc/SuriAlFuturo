using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIContainer : MonoBehaviour {
    public bool Active;
    public GameObject ActiveIndicator;
    public Image Icon;

    public int Index;
    private UIInventory _controller;

    void Start () {
        Icon = GetComponent<Image>();
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.Canvas)
            .GetComponent<UIInventory>();
    }

    void Update () {
        ActiveIndicator.SetActive(Active);
    }

    public void SetSprite (Sprite sprite) {
        if (Icon == null) {
            Icon = GetComponent<Image>();
        }
        Icon.sprite = sprite;
    }

    public void SetActive () {
        _controller.SetActive(this);
    }
}
