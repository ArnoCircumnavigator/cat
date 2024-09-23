using NUnit.Framework;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Cat.RuntimeTests
{
    public class Test_Async : IPrebuildSetup, IPostBuildCleanup
    {
        static readonly Loger loger = new Loger("asyncTest");

        private string originalScene;

        private const string target_SceneName = "Assets/Tests/Test_1_Async/Test_Async.unity";

        public void Setup()
        {
#if UNITY_EDITOR
            if (EditorBuildSettings.scenes.Any(scene => scene.path == target_SceneName))
            {
                return;
            }

            var includedScenes = EditorBuildSettings.scenes.ToList();
            includedScenes.Add(new EditorBuildSettingsScene(target_SceneName, true));
            EditorBuildSettings.scenes = includedScenes.ToArray();
#endif
        }

        [UnitySetUp]
        public IEnumerator SetupBeforeTest()
        {
            originalScene = SceneManager.GetActiveScene().path;
            if (!File.Exists(target_SceneName))
            {
                Assert.Inconclusive("The path to the Scene is not correct. Set the correct path for the k_SceneName variable.");
            }
            SceneManager.LoadScene(target_SceneName);
            yield return null; // Skip a frame, allowing the scene to load.
        }

        [UnityTest]
        public IEnumerator VerifyScene_2()
        {
            //yield return new WaitForSeconds(60);
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }
        }

        [TearDown]
        public void TeardownAfterTest()
        {
            SceneManager.LoadScene(originalScene);
        }

        public void Cleanup()
        {
#if UNITY_EDITOR
            EditorBuildSettings.scenes = EditorBuildSettings.scenes.Where(scene => scene.path != target_SceneName).ToArray();
#endif
        }
    }
}