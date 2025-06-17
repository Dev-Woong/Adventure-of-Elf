using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject MenuUI;
    private const int resume = 0, save = 1, title = 2, exit = 3;
    public Button[] Buttons;
    public GameObject Selected_Btn_Highlight;
    public GameObject[] OtherUI;
    public bool[] OtherUIAct;
    private int selectedMenu;
    private bool activated = false;
    public void OtherUIActFalse()
    {
        for (int i = 0; i < OtherUI.Length - 1; i++)
        {
            if (!OtherUI[i].activeSelf)
            {
                Equipment.activated = false;
                Inventory.instance.activated = false;
            }
        }
    }
    public void SelectResume()
    {
        MenuUI.SetActive(false);
        Time.timeScale = 1;
    }
    public void SelectSave()
    {

    }
    public void SelectTitle()
    {

    }
    public void SelectExit()
    {
        Application.Quit();
    }
    public void SelectedMenu()
    {
        Selected_Btn_Highlight.transform.position = Buttons[selectedMenu].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activated = !activated;
            if (activated)
            {
                Time.timeScale = 0;
                ArcherCtrl.Instance.moveAble = false;
                for (int i = 0; i < OtherUI.Length - 1; i++) 
                { OtherUI[i].SetActive(false); }
                OtherUIActFalse();
                MenuUI.SetActive(true);
                selectedMenu = 0;
                SelectedMenu();
            }
            else 
            {
                MenuUI.SetActive(false); 
                OtherUI[4].SetActive(true); 
                ArcherCtrl.Instance.moveAble = true;
                Time.timeScale = 1;
            }
        }
        if (activated)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectedMenu < Buttons.Length-1)
                    selectedMenu++;
                else
                    selectedMenu = 0;
                SelectedMenu();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectedMenu > 0)
                    selectedMenu--;
                else
                    selectedMenu = Buttons.Length - 1;
                SelectedMenu();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                if (selectedMenu == resume)
                {
                    SelectResume();
                }
                else if (selectedMenu == save)
                {
                    SelectSave();
                    Debug.Log("시스템 저장");
                }
                else if (selectedMenu == title)
                {
                    SelectTitle();
                    Debug.Log("타이틀 이동");
                }
                else if (selectedMenu == exit)
                {
                    SelectExit();
                    Debug.Log("시스템 종료");
                }
            }
        }
    }
}
