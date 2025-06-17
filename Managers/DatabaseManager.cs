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
                Debug.Log("Hp가 50 회복되었습니다.");
                ArcherCtrl.Instance.currHP += 50;
                if (ArcherCtrl.Instance.currHP > ArcherCtrl.Instance.initHP)
                {
                    ArcherCtrl.Instance.currHP = ArcherCtrl.Instance.initHP;
                }
                break;
            case 10002:
                Debug.Log("Mp가 30 회복되었습니다.");
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
        itemList.Add(new Item(10001, "체력 포션", "사용시 체력을 50 회복시켜 줍니다.", Item.ItemType.Use,0,0,0,0,0,0,0,0));
        itemList.Add(new Item(10002, "마나 포션", "사용시 마나를 30 회복시켜 줍니다.", Item.ItemType.Use,0,0,0,0,0,0,0,0));
        itemList.Add(new Item(20001, "조악한 나무 활", "길에 굴러다니는 나뭇가지로 만든 것 같은 활", Item.ItemType.Equip, 8, 0, 0, 0, 0, 10, 15));
        itemList.Add(new Item(20101, "거친 가죽갑옷", "가라테 늑대의 가죽으로 만든 가죽 갑옷",Item.ItemType.Equip, 0, 4, 30, 0, 0, 0, 0));
        itemList.Add(new Item(20201, "헌 가죽 신발","누군가 오래 쓰다가 버린 신발" , Item.ItemType.Equip, 0, 0, 10, 0, 0.5f, 0, 0 ));
        itemList.Add(new Item(20301, "녹슨 체인 목걸이", "이런걸 목에 왜 두르고 다니는지 모르겠다", Item.ItemType.Equip,0,0,10,20,0.1f,5,5));
        itemList.Add(new Item(20401, "가죽 암릿","손목 보호대 정도로는 적당한 것 같다",Item.ItemType.Equip, 0, 2, 10, 15, 0, 5, 5));
        itemList.Add(new Item(20501, "싸구려 철제반지", "??? : 지금 이딴 싸구려 반지로 청혼하는거예요?", Item.ItemType.Equip, 0, 0, 0, 30, 0, 5, 8));
        Debug.Log(itemList);
    }

   
}
