using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressablesSceneLoader : MonoBehaviour
{
    [SerializeField] private AssetReference sceneAssetReference;
    
    public void LoadScene()
    {
        sceneAssetReference.LoadSceneAsync(UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

   
}
