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
    private Inventory_Slot[] slots; // �κ��丮 ���Ե�
    

    private  List<Item> inventoryItemList; // �÷��̾ ������ ������ ����Ʈ.
    private List<Item> inventoryTabList; // ������ �ǿ� ���� �ٸ��� ������ ������ ����Ʈ.

    public Text Description_Text; // �ο� ����.
    public string[] tabDescription; // �� �ο� ����.

    public RectTransform tf; // slot �θ�ü.
    public RectTransform ViewRect;

    public GameObject go; // �κ��丮 Ȱ��ȭ ��Ȱ��ȭ.
    public GameObject[] selectedTabImages;
    public GameObject go_OOC; // ������ Ȱ��ȭ ��Ȱ��ȭ.
    public GameObject FloatingTextPref;
    public GameObject WarningTextPref;
    public GameObject[] OtherUI;
    public GameObject MenuUI;
    
    private int selectedItem; // ���õ� ������.
    private int selectedTab; // ���õ� ��.

    public  bool activated; // �κ��丮 Ȱ��ȭ�� true;
    private bool tabActivated; // �� Ȱ��ȭ�� true
    private bool itemActivated; // ������ Ȱ��ȭ�� true.
    public bool stopKeyInput; // Ű�Է� ���� (�Һ��� �� ���ǰ� ���� �ٵ�, �� �� Ű�Է� ����)
    private bool preventExec; // �ߺ����� ����.
    
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
        inventoryItemList.Add(new Item(10001, "ü�� ����", "���� ü���� 50 ȸ������ �ݴϴ�.", Item.ItemType.Use, 0, 0, 0, 0, 0, 0, 0, 0));
        inventoryItemList.Add(new Item(10002, "���� ����", "���� ������ 30 ȸ������ �ݴϴ�.", Item.ItemType.Use, 0, 0, 0, 0, 0, 0, 0, 0));
        inventoryItemList.Add(new Item(20001, "������ ���� Ȱ", "�濡 �����ٴϴ� ���������� ���� �� ���� Ȱ", Item.ItemType.Equip, 8, 0, 0, 0, 0, 10, 15));
        inventoryItemList.Add(new Item(20101, "��ģ ���װ���", "������ ������ �������� ���� ���� ����", Item.ItemType.Equip, 0, 4, 30, 0, 0, 0, 0));
        inventoryItemList.Add(new Item(20201, "�� ���� �Ź�", "������ ���� ���ٰ� ���� �Ź�", Item.ItemType.Equip, 0, 0, 10, 0, 0.5f, 0, 0));
        inventoryItemList.Add(new Item(20301, "�콼 ü�� �����", "�̷��� �� �� �θ��� �ٴϴ��� �𸣰ڴ�", Item.ItemType.Equip, 0, 0, 10, 20, 0.1f, 5, 5));
        inventoryItemList.Add(new Item(20401, "���� �ϸ�", "�ո� ��ȣ�� �����δ� ������ �� ����", Item.ItemType.Equip, 0, 2, 10, 15, 0, 5, 5));
        inventoryItemList.Add(new Item(20501, "�α��� ö������", "??? : ���� �̵� �α��� ������ ûȥ�ϴ°ſ���?", Item.ItemType.Equip, 0, 0, 0, 30, 0, 5, 8));

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
            for (int i = 0; i < Database.itemList.Count; i++)// �����ͺ��̽� ������ �˻�
            {
                if (_itemID == Database.itemList[i].itemID) // �����ͺ��̽��� ������ ���̵� ã��
                {
                    GameObject clone = Instantiate(FloatingTextPref, cam.WorldToScreenPoint(new Vector3(target.transform.position.x, target.transform.position.y + 0.4f, target.transform.position.z)), Quaternion.Euler(Vector3.zero));
                    clone.GetComponent<FloatingText>().text.text = Database.itemList[i].itemName + " " + _count + "�� ȹ�� +";
                    clone.transform.SetParent(this.transform);
                    for (int j = 0; j < inventoryItemList.Count; j++) // ����ǰ�� ���� �������� �ִ���
                    {
                        if (inventoryItemList[j].itemID == _itemID) // ���� �������� �ִٸ� ������ ����������
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
                    inventoryItemList.Add(Database.itemList[i]); // ����ǰ�� ������������ ���ٸ� �ش� ������ �߰�.
                    return;
                }
            }
            Debug.LogError("�����ͺ��̽��� �ش� ID���� ���� �������� �������� �ʽ��ϴ�."); // �����ͺ��̽��� �ش� ������ ID����
        }
        
            else if  (inventoryItemList.Count>=slots.Length&&textCool <= 0)
            {
                GameObject WarningText = Instantiate(WarningTextPref, cam.WorldToScreenPoint(new Vector3(target.transform.position.x, 1, target.transform.position.z)), Quaternion.Euler(Vector3.zero));
                WarningText.GetComponent<FloatingText>().text.text = "�κ��丮�� ���� á���ϴ�.";
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
    } // �κ��丮 ���� �ʱ�ȭ

    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
        ViewRect.position=StartViewPos; 
    } // �� Ȱ��ȭ

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
    } // ���õ� ���� �����ϰ� �ٸ� ��� ���� �÷� ���İ� 0���� ����.

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
    } // ���õ� �� ��¦�� ȿ��

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
        } // �ǿ� ���� ������ �з�. �װ��� �κ��丮 �� ����Ʈ�� �߰�

        for (int i = 0; i < inventoryTabList.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].AddItem(inventoryTabList[i]);
        } // �κ��丮 �� ����Ʈ�� ������, �κ��丮 ���Կ� �߰�

        SelectedItem();
    } // ������ Ȱ��ȭ (inventoryTabList�� ���ǿ� �´� �����۵鸸 �־��ְ�, �κ��丮 ���Կ� ���)

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
            Description_Text.text = "�ش� Ÿ���� �������� �����ϰ� ���� �ʽ��ϴ�.";
    } // ���õ� �������� �����ϰ�, �ٸ� ��� ���� �÷� ���İ��� 0���� ����.

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
    } // ���õ� ������ ��¦�� ȿ��.

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
                } // �κ��丮 ���� �ݱ�

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

                    } // �� Ȱ��ȭ�� Ű�Է� ó��.

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
                                if (selectedTab == 0) // ���
                                {
                                    StartCoroutine(OOCCoroutine("����", "���"));
                                }
                                else if (selectedTab == 1) // �Һ������
                                {
                                    StartCoroutine(OOCCoroutine("���", "���"));
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
                    } // ������ Ȱ��ȭ�� Ű�Է� ó��.

                    if (Input.GetKeyUp(KeyCode.Z)) // �ߺ� ���� ����.
                        preventExec = false;
                } // �κ��丮�� ������ Ű�Է�ó�� Ȱ��ȭ.
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