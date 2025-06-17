using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnEquip : MonoBehaviour
{
    public Image[] UnEquipRegion;
    public int UnEquipIndex;
    private Equipment Equip;
    float countTime = 0;
    void Start()
    {
        Equip = FindAnyObjectByType<Equipment>();
    }

    // Update is called once per frame
    void Update()
    {
        countTime += Time.deltaTime;
        NotionUnEquip();
    }
   

    public void NotionUnEquip()
    {
        for (int i = 0; i < Equip.equipItemList.Length; i++) 
        {
            if (Equip.equipItemList[i].itemID == 0)
            {
                if (countTime % 2 == 1)
                {
                    UnEquipRegion[i].color = new Color32(0, 0, 0, 90);
                }
                else if (countTime % 2 == 0)
                {
                    UnEquipRegion[i].color = new Color32(0, 0, 0, 200);
                }
            }
            else 
            { 
                UnEquipRegion[i].color = new Color(0,0,0,0);
            } 
        }
    }
    
}
