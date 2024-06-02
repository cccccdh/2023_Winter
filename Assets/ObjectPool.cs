using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public GameObject poolingObjAmmo;
    public GameObject poolingObjBullet;

    Queue<AmmoController> poolingAmmoQueue = new Queue<AmmoController>();
    Queue<BulletController> poolingBulletQueue = new Queue<BulletController>();

    private void Awake()
    {
        Instance = this;

        Initialize(6);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingAmmoQueue.Enqueue(CreateAmmo());
            poolingBulletQueue.Enqueue(CreateBullet());
        }
    }

    private AmmoController CreateAmmo()
    {
        var newObj = Instantiate(poolingObjAmmo).GetComponent<AmmoController>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static AmmoController GetAmmo()
    {
        if (Instance.poolingAmmoQueue.Count > 0)
        {
            var obj = Instance.poolingAmmoQueue.Dequeue();
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(null);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateAmmo();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(Instance.transform);
            return newObj;
        }
    }

    public static void ReturnObjectAmmo(AmmoController obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingAmmoQueue.Enqueue(obj);
    }

    private BulletController CreateBullet()
    {
        var newObj = Instantiate(poolingObjBullet).GetComponent<BulletController>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static BulletController GetBullet()
    {
        if (Instance.poolingBulletQueue.Count > 0)
        {
            var obj = Instance.poolingBulletQueue.Dequeue();
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(null);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateBullet();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(Instance.transform);
            return newObj;
        }
    }

    public static void ReturnObjectBullet(BulletController obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingBulletQueue.Enqueue(obj);
    }
}

