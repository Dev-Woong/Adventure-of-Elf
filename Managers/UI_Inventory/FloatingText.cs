using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed;
    public float destroyTime;

    private float colorSpeed = 1.5f;
    public Text text;
    Color color;

    private void Start()
    {
        color = text.color;
    }
    void Update()
    {
        transform.Translate(new Vector3(0,1*moveSpeed * Time.deltaTime, 0));
        destroyTime -=Time.deltaTime;
        color.a = Mathf.Lerp(color.a, 80, Time.deltaTime * colorSpeed);
        text.color = color;
        if (destroyTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
