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
    private Collectable.Tag _tag;

    void Start () {
        Icon = GetComponent<Image>();
        _controller = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.Canvas)
            .GetComponent<UIInventory>();
    }

    void Update () {
        ActiveIndicator.SetActive(Active);
    }

    public void SetSprite (Sprite sprite, Collectable.Tag tag) 
    {
        if (Icon == null) {
            Icon = GetComponent<Image>();
        }
        Icon.sprite = sprite;
        _tag = tag;
    }

    public void SetActive () {
        _controller.SetActive(this);
    }


    public void Press()
    {
        Debug.Log("Press");
        _controller.ShowInventoryLabel(_tag);
    }



    public void Release()
    {
        Debug.Log("Release");
        _controller.HideInventoryLabel();
    }


}
