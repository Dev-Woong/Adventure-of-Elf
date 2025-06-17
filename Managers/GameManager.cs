using UnityEngine;
public class GameManager : MonoBehaviour
{
    public bool isNextSceneChange = false;
    public bool isBackSceneChange = false;
    public int SceneNum = 0;
    // 생성할 오브젝트의 key값지정을 위한 변수
    
    private static GameManager s_instance = null;
    
    public static GameManager Instance
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
}
