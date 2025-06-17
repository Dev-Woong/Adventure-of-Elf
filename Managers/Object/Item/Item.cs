using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[System.Serializable]
public class Item 
{
    public int itemID; // 아이템 고유 ID 
    public string itemName;   // 아이템의 이름, 중복가능
    public string itemDescription; // 아이템 설명
    public int itemCount; // 소지 개수
    public Sprite itemIcon; // 아이템의 아이콘
    public ItemType itemType;
    public enum ItemType
    {
        Use,
        Equip,
        Quest,
        ETC
    }
    public float itemAtk;
    public float itemCritProb;
    public float itemCritDmg;
    public float itemDef;
    public float itemSpd;
    public float itemHp;
    public float itemMp;


    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, float _atk, float _def,float _hp, float _mp, float _speed, float _critProb, float _critDmg, int _itemCount = 1)
    {
        itemID= _itemID;
        itemName= _itemName;
        itemDescription= _itemDes;
        itemType = _itemType;
        itemCount= _itemCount;
        itemIcon = Resources.Load("ItemIcon/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
        itemAtk=_atk;
        itemDef =_def;
        itemHp = _hp ;
        itemMp= _mp ;
        itemSpd=  _speed;
        itemCritProb =_critProb;
        itemCritDmg = _critDmg;
    }
}
