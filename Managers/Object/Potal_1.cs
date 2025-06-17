using UnityEngine;

public class Potal_1 : MonoBehaviour
{
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameObject.transform.position.x >= 0)
            {
                GameManager.Instance.isNextSceneChange = true;
                
            }
            if (gameObject.transform.position.x <= 0)
            {
                GameManager.Instance.isBackSceneChange = true;
                
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.isBackSceneChange= false;
            GameManager.Instance.isNextSceneChange = false;
        }
    }
}
