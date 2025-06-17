using UnityEngine;
using UnityEngine.UI;
public class MapName : MonoBehaviour
{
    public Text mapNameText;
    void Update()
    {
        mapNameText.text = $"Ω√¿€¿« Ω£{GameManager.Instance.SceneNum + 1}";
    }
}
