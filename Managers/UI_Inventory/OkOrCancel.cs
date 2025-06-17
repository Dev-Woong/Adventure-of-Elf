using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OkOrCancel : MonoBehaviour
{
    public GameObject up_Panel;
    public GameObject down_Panel;
    public GameObject ChoicePanel;
    public Text up_Text;
    public Text down_Text;

    public bool activated;
    private bool keyInput;
    private bool result=true;
    public void Selected()
    {
        result = !result;
        if (result)
        {
            up_Panel.SetActive(false);
            down_Panel.SetActive(true);
        }
        else
        {
            up_Panel.SetActive(true);
            down_Panel.SetActive(false);
        }
    }
    public void ShowTwoChoice(string _upText, string _downText)
    {
        activated = true;
        result = true;
        up_Text.text = _upText;
        down_Text.text = _downText;
        up_Panel.SetActive(false);
        down_Panel.SetActive(true);
        StartCoroutine(ShowTwoChoiceCoroutine());
    }
    public bool GetResult()
    {
        return result;
    }
    IEnumerator ShowTwoChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        keyInput = true;
    }
    void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Selected();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Selected();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                keyInput = false;
                activated = false;
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                keyInput = false;
                activated = false;
                result = false;
            }
        }

    }
}
