using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UnEquipNotion : MonoBehaviour
{
    public Image[] equipslots;
   
    private Equipment Equip;
    public Text text;
    void Start()
    {
        Equip = FindAnyObjectByType<Equipment>();
        
    }

    // Update is called once per frame
    void Update()
    {
        UnEquipSlots();
        UnEquipSlotsText();
    }
    void UnEquipEffect(int i)
    {
        equipslots[i].color = new Color(0, 0, 0, 255);
    }
    void UnEquipSlots()
    {
        for (int i = 0; i < equipslots.Length; i++)
        {
            if (Equip.equipItemList[i].itemID == 0)
            {
                
                UnEquipEffect(i);
            }
            else
            {
               
                equipslots[i].color = new Color(0, 0, 0, 0);
            }
        }
    }
    void UnEquipSlotsText()
    {

        if (Equip.equipItemList[0].itemID == 0)
        {
            text.text = "착용하지 않은 장비가 있습니다!!";
        }
        else if (Equip.equipItemList[1].itemID == 0)
        {
            text.text = "착용하지 않은 장비가 있습니다!!";
        }
        else if (Equip.equipItemList[2].itemID == 0)
        {
            text.text = "착용하지 않은 장비가 있습니다!!";
        }
        else if (Equip.equipItemList[3].itemID == 0)
        {
            text.text = "착용하지 않은 장비가 있습니다!!";
        }
        else if (Equip.equipItemList[4].itemID == 0)
        {
            text.text = "착용하지 않은 장비가 있습니다!!";
        }
        else if (Equip.equipItemList[5].itemID == 0)
        {
            text.text = "착용하지 않은 장비가 있습니다!!";
        }
        else
        {
            text.text = "";
        }

    }
}
