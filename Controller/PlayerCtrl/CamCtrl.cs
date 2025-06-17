using UnityEngine;

public class CamCtrl : MonoBehaviour
{
    public static CamCtrl s_instance;
    public static CamCtrl Instance
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
    public Transform target;

    public float smoothSpeed = 3;
    public Vector2 offset;
    public float limitMinX, limitMaxX, limitMinY, limitMaxY;
    float cameraHalfWidth=0f, cameraHalfHeight=0f;

    Camera cam;
    private void Start()
    {
        cam = Camera.main;
        cameraHalfWidth = cam.aspect * cam.orthographicSize;
        cameraHalfHeight = cam.orthographicSize;
       
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(target.position.x + offset.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),   // X
            Mathf.Clamp(target.position.y + offset.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight), // Y
            -10);                                                                                                  // Z
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }
}
