using UnityEngine;
using UnityEngine.UI;
public class MapName : MonoBehaviour
{
    public Text mapNameText;
    void Update()
    {
        mapNameText.text = $"������ ��{GameManager.Instance.SceneNum + 1}";
    }
}
