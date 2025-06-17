using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    private Inventory Inven;
    private OkOrCancel OOC;
    public const int WEAPON = 0, Armor = 1, Shoes = 2, Necklace = 3, Bracelet = 4, Ring = 5;

    public GameObject go;
    public GameObject go_OOC;
    //public Text[] text; 스탯
    public Image[] img_slots; // 장비 아이콘
    public GameObject go_selected_slot_Highlight; // 장비 슬롯 선택
    public GameObject[] OtherUI;
    public GameObject MenuUI;
    public Item[] equipItemList;

    private int selectedSlot;

    public static bool activated = false;
    public bool inputKey = true;
    void Start()
    {
        Inven = FindFirstObjectByType<Inventory>();
        OOC = FindFirstObjectByType<OkOrCancel>();
    }
    public void SelectedSlot()
    {
        go_selected_slot_Highlight.transform.position = img_slots[selectedSlot].transform.position;
    }
    public void ClearEquip()
    {
        Color color = img_slots[0].color;
        color.a = 0;
        for (int i = 0; i < img_slots.Length; i++)
        {
            img_slots[i].sprite = null;
            img_slots[i].color = color;
        }
    }
    public void ShowEquip()
    {
        Color color = img_slots[0].color;
        color.a = 1f;
        for (int i = 0; i < img_slots.Length; i++)
        {
            if (equipItemList[i].itemID != 0)
            {
                img_slots[i].sprite = equipItemList[i].itemIcon;
                img_slots[i].color = color;
            }
        }
    }
    public void EquipItem(Item _item)
    {
        string temp = _item.itemID.ToString();
        temp = temp.Substring(0, 3);
        switch (temp)
        {
            case "200":     // 무기
                EquipItemCheck(WEAPON,_item);
                break;
            case "201":     // 방어구
                EquipItemCheck(Armor, _item);
                break;
            case "202":     // 신발
                EquipItemCheck(Shoes, _item);
                break;
            case "203":     // 목걸이
                EquipItemCheck(Necklace, _item);
                break;
            case "204":     // 팔찌
                EquipItemCheck(Bracelet, _item);
                break;
            case "205":     // 반지
                EquipItemCheck(Ring, _item);
                break;
        }
    }
    public void EquipItemCheck(int _equipSlotNum, Item _item)
    {
        if (equipItemList[_equipSlotNum].itemID == 0)
        {
            equipItemList[_equipSlotNum] = _item;
        }
        else
        {
            Inven.EquipToInvetory(equipItemList[_equipSlotNum]);
        }
        EquipItemEffect(_item);
    }
    public void EquipItemEffect(Item _item) // 공, 방, 크뎀, 크확, 체, 마나, 이속
    {
        ArcherCtrl.Instance.itemAtk += _item.itemAtk;
        ArcherCtrl.Instance.itemDef += _item.itemDef;
        ArcherCtrl.Instance.itemCritDmg += _item.itemCritDmg;
        ArcherCtrl.Instance.itemCritProb += _item.itemCritProb;
        ArcherCtrl.Instance.itemHP += _item.itemHp;
        ArcherCtrl.Instance.itemMP += _item.itemMp;
        ArcherCtrl.Instance.itemSpeed += _item.itemSpd;
    }
    public void UnEquipItemEffect(Item _item) // 공, 방, 크뎀, 크확, 체, 마나, 이속
    {
        ArcherCtrl.Instance.itemAtk -= _item.itemAtk;
        ArcherCtrl.Instance.itemDef -= _item.itemDef;
        ArcherCtrl.Instance.itemCritDmg -= _item.itemCritDmg;
        ArcherCtrl.Instance.itemCritProb -= _item.itemCritProb;
        ArcherCtrl.Instance.itemHP -= _item.itemHp;
        ArcherCtrl.Instance.itemMP -= _item.itemMp;
        ArcherCtrl.Instance.itemSpeed -= _item.itemSpd;
    }

    void Update()
    {
        if (!MenuUI.activeSelf)
        {
            if (inputKey)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    
                    Inven.stopKeyInput =false;
                    activated = !activated;
                    if (activated)
                    {
                        ArcherCtrl.Instance.moveAble = false;
                        Inventory.instance.activated = false;
                        for (int i = 0; i < OtherUI.Length -1; i++)
                        {
                            OtherUI[i].SetActive(false);
                        }
                        go.SetActive(true);
                        selectedSlot = 0;
                        SelectedSlot();
                        ClearEquip();
                        ShowEquip();
                    }
                    else
                    {
                        go.SetActive(false);
                        ClearEquip();
                        ArcherCtrl.Instance.moveAble = true;
                    }
                }
                if (activated)
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        if (selectedSlot < img_slots.Length - 1)
                        {
                            selectedSlot++;
                        }
                        else
                        {
                            selectedSlot = 0;
                        }
                        SelectedSlot();
                        // 슬롯 선택 사운드 재생 컴포넌트 호출 위치
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (selectedSlot > 0)
                        {
                            selectedSlot--;
                        }
                        else
                        {
                            selectedSlot = img_slots.Length - 1;
                        }
                        SelectedSlot();
                        // 슬롯 선택 사운드 재생 컴포넌트 호출 위치
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedSlot < img_slots.Length - 3)
                        {
                            selectedSlot+=3;
                        }
                        else
                        {
                            return;
                        }
                        SelectedSlot();
                        // 슬롯 선택 사운드 재생 컴포넌트 호출 위치
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedSlot > img_slots.Length-4 )
                        {
                            selectedSlot-=3;
                        }
                        else
                        {
                            return;
                        }
                        SelectedSlot();
                        // 슬롯 선택 사운드 재생 컴포넌트 호출 위치
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        inputKey = false;
                        StartCoroutine(OOCCoroutine("벗기", "취소"));
                    }
                }
            }
        }
    }
        IEnumerator OOCCoroutine(string _up, string _down)
    {
        go_OOC.SetActive(true);
        OOC.ShowTwoChoice(_up,_down);
        yield return new WaitUntil(() => !OOC.activated);
        if (OOC.GetResult())
        {
            Inven.EquipToInvetory(equipItemList[selectedSlot]);
            
            equipItemList[selectedSlot] = new Item(0, "", "", Item.ItemType.Equip,0,0,0,0,0,0,0);
            ClearEquip();
            ShowEquip(); 
        }
        inputKey = true;
        go_OOC.SetActive(false);
    }
}

