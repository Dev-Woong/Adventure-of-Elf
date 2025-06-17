using UnityEngine;

public class ArrowCtrl : PoolAble
{
    private void Update()
    {
        if (transform.position.x >= ArcherCtrl.s_instance.SetArrowPos.x + 5 || transform.position.x <= ArcherCtrl.s_instance.SetArrowPos.x - 5 )
        {
            ReleaseObject();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")|| collision.CompareTag("Monster_Pink"))
        {
            ReleaseObject();
        }
    }
}
