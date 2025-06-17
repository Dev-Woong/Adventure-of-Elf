using TMPro;
using UnityEngine;
public class DamageText: MonoBehaviour
{
    // 폰트사이즈
    private float minFontSize;
    private float sizeChangeSpeed;

    //LifeTime
    private float moveSpeed=0.8f;
    private float alphaSpeed = 1.5f;
    public float destroyTime = 1f;

    //Set Timer
    private float time;

    //Set Damage Text 
    public string damage;

    Color alpha;
    TextMeshPro txt;

    private void Awake()
    {
        if (gameObject.name == "CriticalDmgText") 
        {
            minFontSize = 1.2f;
            sizeChangeSpeed = 2f;
        }
        else
        {
            minFontSize = 1f;
            sizeChangeSpeed = 1.5f;
        }
    }
    void Start()
    {
        time = 0;
        txt = GetComponent<TextMeshPro>();
        txt.text = damage;
        alpha = txt.color;

        Destroy(gameObject, destroyTime);
    }
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));

        if (time < -0.4f)
        {
            txt.fontSize += Time.deltaTime * sizeChangeSpeed;
        }
        else
        {
            if (!(txt.fontSize <= minFontSize))
            {
                txt.fontSize -=Time.deltaTime * sizeChangeSpeed;
            }
        }
        time += Time.deltaTime * sizeChangeSpeed;
        alpha.a = Mathf.Lerp(alpha.a, 80,Time.deltaTime* alphaSpeed);
        txt.color = alpha;
    }
}
