using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace OrbitalNine.Core.Tests
{
    public class DontDestroyOnLoadTests
    {
        [UnityTest]
        public IEnumerator GameObjectWithDontDestroyOnLoad_PersistsAfterSceneLoad()
        {
            // Arrange
            var go = new GameObject("MyGameObject");
            go.AddComponent<DontDestroyOnLoad>();

            // Act
            // Reload the current scene to simulate a scene change.
            yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            // Assert
            // The GameObject should still exist after loading a new scene.
            var persistentObject = GameObject.Find("MyGameObject");
            Assert.IsNotNull(persistentObject);

            // Clean up
            Object.Destroy(persistentObject);
        }

        [UnityTest]
        public IEnumerator GameObjectWithoutDontDestroyOnLoad_IsDestroyedAfterSceneLoad()
        {
            // Arrange
            var go = new GameObject("MyVolatileGameObject");

            // Act
            // Reload the current scene to simulate a scene change.
            yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            // Assert
            var volatileObject = GameObject.Find("MyVolatileGameObject");
            Assert.IsNull(volatileObject);
        }
    }
}
