using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour {
    public int ActiveItem = 0;
    public List<UIContainer> Slots;
    public GameObject InventoryLabel;

    private Dictionary<Collectable.Tag, Sprite> _icons =
        new Dictionary<Collectable.Tag, Sprite>();

    private CollectionSystem _controller;

    Dictionary<Collectable.Tag, string> _names;

    void Awake()
    {
        _names = new Dictionary<Collectable.Tag, string>();
        _names.Add(Collectable.Tag.camara, "Cámara");
        _names.Add(Collectable.Tag.chatarra, "Chatarra");
        _names.Add(Collectable.Tag.cosechadora, "Cosechadora");
        _names.Add(Collectable.Tag.dron, "Dron");
        _names.Add(Collectable.Tag.dron_completo, "Dron Completo");
        _names.Add(Collectable.Tag.energia, "Energía");
        _names.Add(Collectable.Tag.flor, "Flor");
        _names.Add(Collectable.Tag.holograma, "Holograma");
        _names.Add(Collectable.Tag.llave, "Llave");
        _names.Add(Collectable.Tag.maleta_verde_arreglada, "Maleta Arreglada");
        _names.Add(Collectable.Tag.maleta_verde_arruinada, "Maleta Arruinada");
        _names.Add(Collectable.Tag.motor, "Motor");
        _names.Add(Collectable.Tag.motor_ionico, "Motor Iónico");
        _names.Add(Collectable.Tag.NONE, "Ninguno");
        _names.Add(Collectable.Tag.notas, "Notas de Natalia");
        _names.Add(Collectable.Tag.pala, "Pala Cosechadora");
        _names.Add(Collectable.Tag.patineta, "Patineta");
        _names.Add(Collectable.Tag.patineta_arreglada, "Patineta Modificada");
        _names.Add(Collectable.Tag.patineta_joaquin, "Patineta de Joaquín");
        _names.Add(Collectable.Tag.patineta_voladora, "Patineta Flotante");
        _names.Add(Collectable.Tag.pollera, "Pollera");
        _names.Add(Collectable.Tag.prueba, "Prueba");
        _names.Add(Collectable.Tag.rueda, "Rueda");
        _names.Add(Collectable.Tag.rueda1, "Rueda Nro 1");
        _names.Add(Collectable.Tag.rueda2, "Rueda Nro 2");
        _names.Add(Collectable.Tag.rueda3, "Rueda Nro 3");
        _names.Add(Collectable.Tag.rueda4, "Rueda Nro 4");
        _names.Add(Collectable.Tag.tmt, "Cerebro TIM");
        _names.Add(Collectable.Tag.turbina, "Turbina");
        _names.Add(Collectable.Tag.ydroid, "Teléfono modelo yDroid");
    }



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
                Slots[i].SetSprite(_icons[_controller.Inventory[i]], _controller.Inventory[i]);
            } else {
                Slots[i].gameObject.SetActive(false);
            }
            Slots[i].Active = false;
        }

        Slots[ActiveItem].Active = true;
    }

    public void Update () {
        if (Input.GetButtonDown("InventaryNext")) {
            ActiveItem++;
            Refresh();
            Debug.Log("Next! " + Time.time);
        } else if (Input.GetButtonDown("InventaryPrevious")) {
            ActiveItem--;
            Refresh();
            Debug.Log("previous! " + Time.time);
        }
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

    public void ShowInventoryLabel(Collectable.Tag tag)
    {
        InventoryLabel.SetActive(true);
        InventoryLabel.GetComponent<Text>().text = _names[tag]; 
    }


    public void HideInventoryLabel()
    {
        InventoryLabel.SetActive(false);
    }

}
