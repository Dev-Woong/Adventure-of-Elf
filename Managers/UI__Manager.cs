using UnityEngine;

public class UI__Manager : MonoBehaviour
{
    public static UI__Manager s_instance;
    public static UI__Manager Instance
    {
        get
        {
            if (s_instance == null) return null;
            return s_instance;
        }

    }
    void Awake()
    {
        if (Instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
