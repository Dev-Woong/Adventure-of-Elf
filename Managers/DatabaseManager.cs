using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;
    private void Awake()
    {
       if (instance != null)
        {
            Destroy(this.gameObject);
        }
       else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
    public void UseItem(int _itemID)
    {
        switch (_itemID)
        {
            case 10001:
                Debug.Log("Hp�� 50 ȸ���Ǿ����ϴ�.");
                ArcherCtrl.Instance.currHP += 50;
                if (ArcherCtrl.Instance.currHP > ArcherCtrl.Instance.initHP)
                {
                    ArcherCtrl.Instance.currHP = ArcherCtrl.Instance.initHP;
                }
                break;
            case 10002:
                Debug.Log("Mp�� 30 ȸ���Ǿ����ϴ�.");
                ArcherCtrl.Instance.currMP += 30;
                if (ArcherCtrl.Instance.currMP > ArcherCtrl.Instance.initMP)
                {
                    ArcherCtrl.Instance.currMP = ArcherCtrl.Instance.initMP;
                }
                break;
        }
    }
    
    public List<Item> itemList = new List<Item>();
    void Start()
    {
        itemList.Add(new Item(10001, "ü�� ����", "���� ü���� 50 ȸ������ �ݴϴ�.", Item.ItemType.Use,0,0,0,0,0,0,0,0));
        itemList.Add(new Item(10002, "���� ����", "���� ������ 30 ȸ������ �ݴϴ�.", Item.ItemType.Use,0,0,0,0,0,0,0,0));
        itemList.Add(new Item(20001, "������ ���� Ȱ", "�濡 �����ٴϴ� ���������� ���� �� ���� Ȱ", Item.ItemType.Equip, 8, 0, 0, 0, 0, 10, 15));
        itemList.Add(new Item(20101, "��ģ ���װ���", "������ ������ �������� ���� ���� ����",Item.ItemType.Equip, 0, 4, 30, 0, 0, 0, 0));
        itemList.Add(new Item(20201, "�� ���� �Ź�","������ ���� ���ٰ� ���� �Ź�" , Item.ItemType.Equip, 0, 0, 10, 0, 0.5f, 0, 0 ));
        itemList.Add(new Item(20301, "�콼 ü�� �����", "�̷��� �� �� �θ��� �ٴϴ��� �𸣰ڴ�", Item.ItemType.Equip,0,0,10,20,0.1f,5,5));
        itemList.Add(new Item(20401, "���� �ϸ�","�ո� ��ȣ�� �����δ� ������ �� ����",Item.ItemType.Equip, 0, 2, 10, 15, 0, 5, 5));
        itemList.Add(new Item(20501, "�α��� ö������", "??? : ���� �̵� �α��� ������ ûȥ�ϴ°ſ���?", Item.ItemType.Equip, 0, 0, 0, 30, 0, 5, 8));
        Debug.Log(itemList);
    }

   
}
