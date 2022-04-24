using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class AddressableAtlasedSpriteLoader : MonoBehaviour
{
    public AssetReferenceAtlasedSprite newSpriteAtlas;
    public string newSpriteAtlasAddress;
    public string atlasedSpriteName;
    public bool useAtlasedSpriteName;
    public bool useManualSpriteLoad;
    
    private SpriteRenderer spriteRenderer;
    

    public void LoadAtlasedSprite()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (useManualSpriteLoad)
        {
            Addressables.LoadAssetAsync<SpriteAtlas>(newSpriteAtlasAddress).Completed += SpriteAtlasLoaded;
        }
        else
        {
            if (useAtlasedSpriteName)
            {
                string atlasedSpriteAddress = newSpriteAtlasAddress + '[' + atlasedSpriteName + ']';
                Addressables.LoadAssetAsync<Sprite>(atlasedSpriteAddress).Completed += SpriteLoaded;
            }
            else
            {
                newSpriteAtlas.LoadAssetAsync().Completed += SpriteLoaded;

            }
        }
       
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
            default:
                break;
        }
    }

    void SpriteAtlasLoaded(AsyncOperationHandle<SpriteAtlas> obj)
    {
        switch (obj.Status)
        {
            case AsyncOperationStatus.Failed :
                Debug.LogError("Sprite Atlas load failed.");
                break;
            case AsyncOperationStatus.Succeeded :
                spriteRenderer.sprite = obj.Result.GetSprite(atlasedSpriteName);
                break;
            default:
                break;
        }
    }

 
}
