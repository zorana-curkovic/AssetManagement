using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetReferenceUtility : MonoBehaviour
{
    public AssetReferenceGameObject gameObjectToLoad;
    
    public AssetReference objectToLoad;

    public AssetReference accessoryObjectToLoad;

    private GameObject instantiatedObject;

    private GameObject instantiatedAccessoryObject;

    public void InstantiateGO()
    {
        Addressables.InstantiateAsync(gameObjectToLoad);
    }
    
    public void LoadHierarchy()
    {
        Addressables.LoadAssetAsync<GameObject>(objectToLoad).Completed += ObjectLoadDone;
    }

    private void ObjectLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        GameObject loadedObject = obj.Result;
        Debug.Log("Successfully loaded object.");
        instantiatedObject = Instantiate(loadedObject);
        Debug.Log("Successfully instantiated object");
        if (accessoryObjectToLoad != null)
        {
            accessoryObjectToLoad.InstantiateAsync(instantiatedObject.transform).Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    instantiatedAccessoryObject = op.Result;
                    Debug.Log("Successfully loaded and instantiated accessory object.");
                }
            };
        }
    }

}
