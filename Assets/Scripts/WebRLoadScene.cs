using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WebRLoadScene : MonoBehaviour
{
    [SerializeField] private string[] _bundleNames;

    public string bundleUrl = "http://localhost:8000/AssetBundles/";
    AssetBundle[] _bundles;
    AssetBundleRequest[] _requests;

    int _downloads;

    void Start()
    {
        Debug.Log("Started Asset Downloading");
        DontDestroyOnLoad(this.gameObject);

        _downloads = 0;
        _bundles = new AssetBundle[_bundleNames.Length];
        _requests = new AssetBundleRequest[_bundleNames.Length];

        int assetIdx = 0;
        foreach (var bundleName in _bundleNames) {
            string addr = bundleUrl + bundleName;
            StartCoroutine(DownloadAssetBundle(addr, assetIdx, OnBundleDownloaded));
            assetIdx++;
        }
    }

    IEnumerator DownloadAssetBundle(string URI, int assetIdx, System.Action callback)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(URI);
        yield return www.SendWebRequest();


#if UNITY_2019
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
         
            Debug.Log($"Bundle {URI} downloaded");

            // Get downloaded asset bundle
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            _bundles[assetIdx] = bundle;
            callback();
        }

#endif

#if UNITY_2020
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log($"Bundle {URI} downloaded");

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            
            _bundles[assetIdx] = bundle;
            callback();
        }
#endif

    }
 

    public void OnBundleDownloaded()
    {
        _downloads++;
            
        Debug.Log($"{_downloads}/{_bundleNames.Length} downloaded");

        if (_downloads == _bundleNames.Length)
        {
            StartCoroutine(PostDownloadLoadAssets());
        }
    }

    public void OnDestroy()
    {
         UnloadBundles();
    }

    public void UnloadBundles()
    {
        Debug.Log("Unload Bundles");
        for (int i = 0; i < _bundles.Length; i++)
        {
            if (_bundles[i])
            {
                Debug.Log($"Unload Bundle {i}");

                _bundles[i].Unload(true);
            }
        }
    }

    IEnumerator PostDownloadLoadAssets()
    {
        Debug.Log("Post Download Assets");
        for (int i = 0; i < _bundles.Length; i++)
        {
            if (_bundles[i] == null)
            {
                Debug.Log($"Bundle {i} is null");
            }
            else if (_bundles[i].isStreamedSceneAssetBundle)
            {
                Debug.Log($"<color=green>Loading assets from the {i}. bundle {_bundleNames[i]} (containing scene)</color>");

                string[] names = _bundles[i].GetAllScenePaths();
                foreach (var name in names)
                {
                    Debug.Log($"Loading scene path {name}");

                    string sceneName = System.IO.Path.GetFileNameWithoutExtension(name);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
                }
            }
            else
            {
                Debug.Log($"<color=yellow>Loading assets from the {i}. bundle {_bundleNames[i]} (not scene)</color>");
                _requests[i] = _bundles[i].LoadAllAssetsAsync();
                while (!_requests[i].isDone)
                    yield return null;

                Object[] objs = _requests[i].allAssets;

                for (int obIdx = 0; obIdx < objs.Length; obIdx++)
                {
                    if (objs[obIdx] != null && objs[obIdx].GetType() == typeof(ShaderVariantCollection))
                    {
                        var shadercollection = (ShaderVariantCollection)objs[obIdx];
                        Debug.Log("ShaderVariants " + _bundles[i] + " " + name);
                        Debug.Log("Begin shader warmup.");
                        if (!shadercollection.isWarmedUp)
                        {
                            shadercollection.WarmUp();
                        }
                        Debug.Log("Shader warm up complete.");
                    }
                    else if (objs[obIdx] != null)
                    {

                        if (objs[obIdx].GetType() == typeof(GameObject))
                        {
                            Instantiate(objs[obIdx] as GameObject);
                            Debug.Log($"Instantiating asset named {objs[obIdx].name}");
                        }
                    }
                    else
                    {
                        Debug.Log($"Loaded asset is null = {obIdx}");
                    }
                }
            }
        }

        yield break;
    }

   




}
