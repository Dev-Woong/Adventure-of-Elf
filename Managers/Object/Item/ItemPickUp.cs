using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ItemPickUp : MonoBehaviour
{
    public int itemID;
    public int count;
    
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            Inventory.instance.GetAnItem(itemID, count);
            Destroy(this.gameObject);
        }
    }
}
