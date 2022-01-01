using System;
using System.Collections;
using UnityEngine;

public class PooledMonoBehaviour : MonoBehaviour
{
    [SerializeField] int initialPoolSize = 10;

    public event Action<PooledMonoBehaviour> OnReturnToPool;

    public int InitialPoolSize => initialPoolSize;

    public T Get<T>(bool enable = true) where T : PooledMonoBehaviour
    {
        var pool = Pool.GetPool(this);
        var pooledObject = pool.Get<T>();

        if (enable)
        {
            pooledObject.gameObject.SetActive(true);
        }

        return pooledObject;
    }
    public T Get<T>(Vector3 position, Quaternion rotation) where T : PooledMonoBehaviour
    {
        var pooledObject = Get<T>();

        pooledObject.transform.position = position;
        pooledObject.transform.rotation = rotation;
        
        return pooledObject;
    }


    protected virtual void OnDisable()
    {
        if (OnReturnToPool != null)
            OnReturnToPool(this);
    }

    protected void ReturnToPool(float delayTimer = 0)
    {
        StartCoroutine(ReturnToPoolAfterSeconds(delayTimer));
    }

    IEnumerator ReturnToPoolAfterSeconds(float delayTimer)
    {
        yield return new WaitForSeconds(delayTimer);
        gameObject.SetActive(false);
    }
}