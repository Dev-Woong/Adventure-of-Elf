using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillIcon : MonoBehaviour
{
    public Image[] SkillIcon;
    public GameObject SkillWindow;
    public GameObject HighLight;
    public GameObject[] SkillInfo;
    public GameObject[] SelectSkillSlot;
    public int selectedSlot=0;
    private void Start()
    {
        for (int i = 0; i < SkillIcon.Length; i++)
        {
            SkillIcon[i].color = new Color32(85,65,65,255);
        }
    }
    void VisibleSkillInfo()
    {
        for (int i = 0; i < SkillInfo.Length - 1; i++)
        {
            SkillInfo[i].SetActive(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !SkillWindow.activeSelf)
        {
            ArcherCtrl.Instance.moveAble = false;
            SkillWindow.SetActive(true);
            SkillIconVisible();
            VisibleSkillInfo();
        }
        else if (Input.GetKeyDown(KeyCode.K) && SkillWindow.activeSelf)
        {
            ArcherCtrl.Instance.moveAble = true;
            SkillWindow.SetActive(false);
        }
        if (SkillWindow.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectedSlot < SkillIcon.Length - 1)
                {
                    selectedSlot++;
                }
                else
                {
                    selectedSlot = 0;
                }
                SelectedSkill();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectedSlot > 0)
                {
                    selectedSlot--;
                }
                else { selectedSlot = SkillIcon.Length - 1; }
                SelectedSkill();
            }

        }
    }
    public void SelectedSkill()
    {
        HighLight.transform.position= SelectSkillSlot[selectedSlot].transform.position;
    }
    void SkillIconVisible()
    {
        if (ArcherCtrl.Instance.level == 1)
        {
            SkillIcon[0].color = new Color32(0, 255, 0, 255);
        }
        else if (ArcherCtrl.Instance.level == 2)
        {
            SkillIcon[1].color = new Color32(255, 255, 255, 255);
        }
    }
    void SkillInfoVisible(int Num)
    {
        if (selectedSlot==0)
        {

        }
    }
}
