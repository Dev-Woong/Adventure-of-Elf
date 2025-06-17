using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;
    private DatabaseManager Database;
    private OkOrCancel OOC;
    private Equipment Equip;
    public Transform target;
    private Inventory_Slot[] slots; // 인벤토리 슬롯들
    

    private  List<Item> inventoryItemList; // 플레이어가 소지한 아이템 리스트.
    private List<Item> inventoryTabList; // 선택한 탭에 따라 다르게 보여질 아이템 리스트.

    public Text Description_Text; // 부연 설명.
    public string[] tabDescription; // 탭 부연 설명.

    public RectTransform tf; // slot 부모객체.
    public RectTransform ViewRect;

    public GameObject go; // 인벤토리 활성화 비활성화.
    public GameObject[] selectedTabImages;
    public GameObject go_OOC; // 선택지 활성화 비활성화.
    public GameObject FloatingTextPref;
    public GameObject WarningTextPref;
    public GameObject[] OtherUI;
    public GameObject MenuUI;
    
    private int selectedItem; // 선택된 아이템.
    private int selectedTab; // 선택된 탭.

    public  bool activated; // 인벤토리 활성화시 true;
    private bool tabActivated; // 탭 활성화시 true
    private bool itemActivated; // 아이템 활성화시 true.
    public bool stopKeyInput; // 키입력 제한 (소비할 때 질의가 나올 텐데, 그 때 키입력 방지)
    private bool preventExec; // 중복실행 제한.
    
    Camera cam;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    float textCool = 2;
    Vector3 StartViewPos;
    // Use this for initialization
    void Start()
    {
         
        StartViewPos = ViewRect.position;
        textCool-=Time.deltaTime;
        instance = this;
        cam = Camera.main;
        Database = FindFirstObjectByType<DatabaseManager>();
        OOC = FindFirstObjectByType<OkOrCancel>();
        Equip = FindFirstObjectByType<Equipment>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<Inventory_Slot>();
        inventoryItemList.Add(new Item(10001, "체력 포션", "사용시 체력을 50 회복시켜 줍니다.", Item.ItemType.Use, 0, 0, 0, 0, 0, 0, 0, 0));
        inventoryItemList.Add(new Item(10002, "마나 포션", "사용시 마나를 30 회복시켜 줍니다.", Item.ItemType.Use, 0, 0, 0, 0, 0, 0, 0, 0));
        inventoryItemList.Add(new Item(20001, "조악한 나무 활", "길에 굴러다니는 나뭇가지로 만든 것 같은 활", Item.ItemType.Equip, 8, 0, 0, 0, 0, 10, 15));
        inventoryItemList.Add(new Item(20101, "거친 가죽갑옷", "가라테 늑대의 가죽으로 만든 가죽 갑옷", Item.ItemType.Equip, 0, 4, 30, 0, 0, 0, 0));
        inventoryItemList.Add(new Item(20201, "헌 가죽 신발", "누군가 오래 쓰다가 버린 신발", Item.ItemType.Equip, 0, 0, 10, 0, 0.5f, 0, 0));
        inventoryItemList.Add(new Item(20301, "녹슨 체인 목걸이", "이런걸 목에 왜 두르고 다니는지 모르겠다", Item.ItemType.Equip, 0, 0, 10, 20, 0.1f, 5, 5));
        inventoryItemList.Add(new Item(20401, "가죽 암릿", "손목 보호대 정도로는 적당한 것 같다", Item.ItemType.Equip, 0, 2, 10, 15, 0, 5, 5));
        inventoryItemList.Add(new Item(20501, "싸구려 철제반지", "??? : 지금 이딴 싸구려 반지로 청혼하는거예요?", Item.ItemType.Equip, 0, 0, 0, 30, 0, 5, 8));

    }
    public void EquipToInvetory(Item _item)
    {
        Equip.UnEquipItemEffect(_item);
        if (Equip.equipItemList[0].itemID != 0)
        {
            inventoryItemList.Add(_item);
        }
        else if (Equip.equipItemList[1].itemID != 0)
        {
            inventoryItemList.Add(_item);
        }
        else if (Equip.equipItemList[2].itemID != 0)
        {
            inventoryItemList.Add(_item);
        }
        else if (Equip.equipItemList[3].itemID != 0)
        {
            inventoryItemList.Add(_item);
        }
        else if (Equip.equipItemList[4].itemID != 0)
        {
            inventoryItemList.Add(_item);
        }
        else if (Equip.equipItemList[5].itemID != 0)
        {
            inventoryItemList.Add(_item);
        }
    }
    public void GetAnItem(int _itemID, int _count = 1)
    {
        if (inventoryItemList.Count< slots.Length)
        {
            for (int i = 0; i < Database.itemList.Count; i++)// 데이터베이스 아이템 검색
            {
                if (_itemID == Database.itemList[i].itemID) // 데이터베이스에 아이템 아이디 찾음
                {
                    GameObject clone = Instantiate(FloatingTextPref, cam.WorldToScreenPoint(new Vector3(target.transform.position.x, target.transform.position.y + 0.4f, target.transform.position.z)), Quaternion.Euler(Vector3.zero));
                    clone.GetComponent<FloatingText>().text.text = Database.itemList[i].itemName + " " + _count + "개 획득 +";
                    clone.transform.SetParent(this.transform);
                    for (int j = 0; j < inventoryItemList.Count; j++) // 소지품에 같은 아이템이 있는지
                    {
                        if (inventoryItemList[j].itemID == _itemID) // 같은 아이템이 있다면 갯수만 증감시켜줌
                        {
                            if (inventoryItemList[j].itemType == Item.ItemType.Use)
                            {
                                inventoryItemList[j].itemCount += _count;
                            }
                            else if (inventoryItemList[j].itemType == Item.ItemType.Quest)
                            {
                                inventoryItemList[j].itemCount += _count;
                            }
                            else if (inventoryItemList[j].itemType == Item.ItemType.ETC)
                            {
                                inventoryItemList[j].itemCount += _count;
                            }
                            else
                            {
                                inventoryItemList.Add(Database.itemList[i]);
                            }
                            return;
                        }
                       
                    }
                    inventoryItemList.Add(Database.itemList[i]); // 소지품에 같은아이템이 없다면 해당 아이템 추가.
                    return;
                }
            }
            Debug.LogError("데이터베이스에 해당 ID값을 가진 아이템이 존재하지 않습니다."); // 데이터베이스에 해당 아이템 ID없음
        }
        
            else if  (inventoryItemList.Count>=slots.Length&&textCool <= 0)
            {
                GameObject WarningText = Instantiate(WarningTextPref, cam.WorldToScreenPoint(new Vector3(target.transform.position.x, 1, target.transform.position.z)), Quaternion.Euler(Vector3.zero));
                WarningText.GetComponent<FloatingText>().text.text = "인벤토리가 가득 찼습니다.";
                WarningText.transform.SetParent(this.transform);
            }
            
        
    }

    public void BetterThanEquip()
    {

        if (Equip.equipItemList[0].itemID == 0)
        {
            for (int j = 0; j < inventoryItemList.Count; j++)
            {
                string temp = inventoryItemList[j].itemID.ToString();
                temp = temp.Substring(0, 3);
                switch (temp)
                {
                    case "200":
                        slots[selectedItem].NotionIcon.SetActive(true);
                        break;
                }
            }
        }
        else if (Equip.equipItemList[1].itemID == 0)
        {
            for (int j = 0; j < inventoryItemList.Count; j++)
            {
                string temp = inventoryItemList[j].itemID.ToString();
                temp = temp.Substring(0, 3);
                switch (temp)
                {
                    case "201":
                        slots[selectedItem].NotionIcon.SetActive(true);
                        break;
                }
            }
        }
        else if (Equip.equipItemList[2].itemID == 0)
        {
            for (int j = 0; j < inventoryItemList.Count; j++)
            {
                string temp = inventoryItemList[j].itemID.ToString();
                temp = temp.Substring(0, 3);
                switch (temp)
                {
                    case "202":
                        slots[selectedItem].NotionIcon.SetActive(true);
                        break;
                }
            }
        }
        else if (Equip.equipItemList[3].itemID == 0)
        {
            for (int j = 0; j < inventoryItemList.Count; j++)
            {
                string temp = inventoryItemList[j].itemID.ToString();
                temp = temp.Substring(0, 3);
                if (temp == "203")
                {
                    slots[selectedItem].NotionIcon.SetActive(true);
                }
            }
        }
        else if (Equip.equipItemList[4].itemID == 0)
        {
            for (int j = 0; j < inventoryItemList.Count; j++)
            {
                string temp = inventoryItemList[j].itemID.ToString();
                temp = temp.Substring(0, 3);
                switch (temp)
                {
                    case "204":
                        slots[selectedItem].NotionIcon.SetActive(true);
                        break;
                }
            }
        }
        else if (Equip.equipItemList[5].itemID == 0)
        {
            for (int j = 0; j < inventoryItemList.Count; j++)
            {
                string temp = inventoryItemList[j].itemID.ToString();
                temp = temp.Substring(0, 3);
                switch (temp)
                {
                    case "205":
                        slots[selectedItem].NotionIcon.SetActive(true);
                        break;
                }
            }
        }
        else
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            { slots[i].NotionIcon.SetActive(false); }
        }

    }
    public void RemoveSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    } // 인벤토리 슬롯 초기화

    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
        ViewRect.position=StartViewPos; 
    } // 탭 활성화

    public void SelectedTab()
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    } // 선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값 0으로 조정.

    IEnumerator SelectedTabEffectCoroutine()
    {
        while (tabActivated)
        {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    } // 선택된 탭 반짝임 효과

    public void ShowItem()
    {
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;
        ViewRect.position = StartViewPos;
        
        switch (selectedTab)
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                    {
                       
                        inventoryTabList.Add(inventoryItemList[i]);
                        BetterThanEquip();
                    }
                }
                break;
            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                   
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        } // 탭에 따른 아이템 분류. 그것을 인벤토리 탭 리스트에 추가

        for (int i = 0; i < inventoryTabList.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].AddItem(inventoryTabList[i]);
        } // 인벤토리 탭 리스트의 내용을, 인벤토리 슬롯에 추가

        SelectedItem();
    } // 아이템 활성화 (inventoryTabList에 조건에 맞는 아이템들만 넣어주고, 인벤토리 슬롯에 출력)

    public void SelectedItem()
    {
        StopAllCoroutines();
        if (inventoryTabList.Count > 0)
        {
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i < inventoryTabList.Count; i++)
                slots[i].selected_Item.GetComponent<Image>().color = color;
            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
            Description_Text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
    } // 선택된 아이템을 제외하고, 다른 모든 탭의 컬러 알파값을 0으로 조정.

    IEnumerator SelectedItemEffectCoroutine()
    {
        while (itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    } // 선택된 아이템 반짝임 효과.

    // Update is called once per frame
    void Update()
    {
        if (!MenuUI.activeSelf)
        {
            if (!stopKeyInput)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    activated = !activated;
                   
                    if (activated)
                    {
                        ArcherCtrl.Instance.moveAble = false;
                        Equipment.activated = false;
                        for (int i = 0; i < OtherUI.Length-1 ; i++)
                        {
                            OtherUI[i].SetActive(false);
                        }
                        go.SetActive(true);
                        selectedTab = 0;
                        tabActivated = true;
                        itemActivated = false;
                        ShowTab();
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            StopAllCoroutines();
                            go_OOC.gameObject.SetActive(false);
                            go.SetActive(false);
                            tabActivated = false;
                            itemActivated = false;
                            
                            ArcherCtrl.Instance.moveAble = true;
                        }
                    }
                } // 인벤토리 열고 닫기

                if (activated)
                {
                    if (tabActivated)
                    {
                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selectedTab < selectedTabImages.Length - 1)
                                selectedTab++;
                            else
                                selectedTab = 0;

                            SelectedTab();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selectedTab > 0)
                                selectedTab--;
                            else
                                selectedTab = selectedTabImages.Length - 1;

                            SelectedTab();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z))
                        {
                            ///theAudio.Play(enter_sound);
                            Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                            color.a = 0.25f;
                            selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                            itemActivated = true;
                            tabActivated = false;
                            preventExec = true;
                            ShowItem();
                        }

                    } // 탭 활성화시 키입력 처리.

                    else if (itemActivated)
                    {
                        if (inventoryTabList.Count > 0)
                        {
                            if (Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                if (selectedItem < inventoryTabList.Count - 2)
                                {
                                    selectedItem += 2;
                                    ViewRect.position = new Vector3(ViewRect.position.x, ViewRect.position.y + 30, ViewRect.position.z);
                                }
                                SelectedItem();
                            }
                            else if (Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                if (selectedItem > 1)
                                {
                                    selectedItem -= 2;
                                    ViewRect.position = new Vector3(ViewRect.position.x, ViewRect.position.y - 30, ViewRect.position.z);
                                }

                                SelectedItem();
                            }
                            else if (Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                if (selectedItem < inventoryTabList.Count - 1)
                                {
                                    selectedItem++;

                                    if (selectedItem % 2 == 0)
                                    {
                                        ViewRect.position = new Vector3(ViewRect.position.x, ViewRect.position.y + 30, ViewRect.position.z);
                                    }
                                }
                                SelectedItem();
                            }
                            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                if (selectedItem > 0)
                                {
                                    selectedItem--;
                                    if (selectedItem >= 1 && selectedItem % 2 == 1)
                                    {
                                        ViewRect.position = new Vector3(ViewRect.position.x, ViewRect.position.y - 30, ViewRect.position.z);
                                    }
                                }
                                SelectedItem();
                            }
                            else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                            {
                                if (selectedTab == 0) // 장비
                                {
                                    StartCoroutine(OOCCoroutine("장착", "취소"));
                                }
                                else if (selectedTab == 1) // 소비아이템
                                {
                                    StartCoroutine(OOCCoroutine("사용", "취소"));
                                }
                            }

                        }

                        if (Input.GetKeyDown(KeyCode.X))
                        {
                            StopAllCoroutines();
                            itemActivated = false;
                            tabActivated = true;
                            ShowTab();
                        }
                    } // 아이템 활성화시 키입력 처리.

                    if (Input.GetKeyUp(KeyCode.Z)) // 중복 실행 방지.
                        preventExec = false;
                } // 인벤토리가 열리면 키입력처리 활성화.
            }
        }
    }

    IEnumerator OOCCoroutine(string _up, string _down)
    {
        stopKeyInput = true;

        go_OOC.SetActive(true);
        OOC.ShowTwoChoice(_up, _down);
        yield return new WaitUntil(() => !OOC.activated);
        if (OOC.GetResult())
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    if (selectedTab == 0)
                    {
                        Equip.EquipItem(inventoryItemList[i]);
                        inventoryItemList.RemoveAt(i);
                        ShowItem();
                        break;
                        
                    }
                    else if (selectedTab == 1)
                    {
                        Database.UseItem(inventoryItemList[i].itemID);
                        if (inventoryItemList[i].itemCount > 1)
                        {
                            inventoryItemList[i].itemCount--;
                        }
                        else
                        {
                            inventoryItemList.RemoveAt(i);
                        }
                        ShowItem();
                        break;
                    }
                }
            }
        }
        stopKeyInput = false;
        go_OOC.SetActive(false);
    }
}