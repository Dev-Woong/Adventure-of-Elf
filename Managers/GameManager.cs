using UnityEngine;
public class GameManager : MonoBehaviour
{
    public bool isNextSceneChange = false;
    public bool isBackSceneChange = false;
    public int SceneNum = 0;
    // ������ ������Ʈ�� key�������� ���� ����
    
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
