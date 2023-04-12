#if UNITY_EDITOR
using System.IO;
using UnityEditor;
namespace Cat.CatEditor
{
    public class CreateAssetBundles : Editor
    {
        [MenuItem("Assets/Cat_ABP")]
        static void BuildAllAssetBundles()
        {
            string assetBundleDirectory = "ABP";
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
}
# endif