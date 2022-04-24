using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class AddressableSpriteLoader : MonoBehaviour
{
    public AssetReferenceSprite newSprite;
    public string newSpriteAddress;
    public bool useAddress;
    
    private SpriteRenderer spriteRenderer;
    public void LoadSprite()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (useAddress)
            Addressables.LoadAssetAsync<Sprite>(newSpriteAddress).Completed += SpriteLoaded;
        else
            newSprite.LoadAssetAsync().Completed += SpriteLoaded;
    }

    void SpriteLoaded(AsyncOperationHandle<Sprite> obj)
    {
        switch (obj.Status)
        {
            case AsyncOperationStatus.Failed :
                Debug.LogError("Sprite Loading Failed");
                break;
            case AsyncOperationStatus.Succeeded :
                spriteRenderer.sprite = obj.Result;
                break;
        }
    }
}
