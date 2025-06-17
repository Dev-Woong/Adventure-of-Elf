using UnityEngine;

public class Effect : PoolAble
{
    void EffectEndEvent()
    {
        Pool.Release(gameObject);
    }
}
