using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[System.Serializable]
public class Item 
{
    public int itemID; // ������ ���� ID 
    public string itemName;   // �������� �̸�, �ߺ�����
    public string itemDescription; // ������ ����
    public int itemCount; // ���� ����
    public Sprite itemIcon; // �������� ������
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
