using UnityEngine;

public class Monster_Pink_Rock : PoolAble
{
    private void Update()
    {
        if (transform.position.x >= MonsterPink.SetRockPos.x+5f || transform.position.x <= MonsterPink.SetRockPos.x-5f)
        {
            ReleaseObject();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ArcherCtrl.Instance.OnDamage(MonsterPink.Atk);
            ReleaseObject();
        }
    }
}
