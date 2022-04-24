using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Life : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    
    void Start()
    {
        Invoke("Release", lifeTime);
    }

    // Update is called once per frame
    void Release()
    {
        Addressables.ReleaseInstance(gameObject);
    }
}
