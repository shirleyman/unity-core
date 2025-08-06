using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace OrbitalNine.Core.Tests
{
    // A dummy class to test SingletonMonoBehaviour
    public class TestSingleton : SingletonMonoBehaviour<TestSingleton>
    {
        public bool awakeCalled = false;
        public bool onDestroyCalled = false;

        protected override void Awake()
        {
            base.Awake();
            awakeCalled = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onDestroyCalled = true;
        }
    }

    public class SingletonMonoBehaviourTests
    {
        [SetUp]
        public void Setup()
        {
            // Ensure no existing instance before each test
            var existingInstance = Object.FindAnyObjectByType<TestSingleton>();
            if (existingInstance != null)
            {
                Object.DestroyImmediate(existingInstance.gameObject);
            }
        }

        [TearDown]
        public void Teardown()
        {
            // Clean up after each test
            var existingInstance = Object.FindAnyObjectByType<TestSingleton>();
            if (existingInstance != null)
            {
                Object.DestroyImmediate(existingInstance.gameObject);
            }
        }

        [UnityTest]
        public IEnumerator Instance_CreatesNewGameObjectIfNoneExists()
        {
            // Arrange
            TestSingleton.Instance.gameObject.name = "TestSingleton"; // Set name for easier finding

            // Act
            var instance = TestSingleton.Instance;

            // Assert
            Assert.IsNotNull(instance);
            Assert.AreEqual("TestSingleton", instance.gameObject.name);
            Assert.IsTrue(instance.awakeCalled);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Instance_ReturnsExistingInstanceIfOneExists()
        {
            // Arrange
            var go = new GameObject("ExistingTestSingleton");
            var existingInstance = go.AddComponent<TestSingleton>();
            existingInstance.awakeCalled = false; // Reset for test

            // Act
            var instance = TestSingleton.Instance;

            // Assert
            Assert.AreSame(existingInstance, instance);
            Assert.IsFalse(instance.awakeCalled); // Awake should not be called again
            yield return null;
        }

        [UnityTest]
        public IEnumerator Instance_OnlyOneInstanceExistsAfterMultipleCalls()
        {
            // Act
            var instance1 = TestSingleton.Instance;
            var instance2 = TestSingleton.Instance;

            // Assert
            Assert.AreSame(instance1, instance2);
            var allInstances = Object.FindObjectsByType<TestSingleton>(FindObjectsSortMode.None);
            Assert.AreEqual(1, allInstances.Length);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Instance_OnRootGameObject_IsDontDestroyOnLoad()
        {
            // Arrange
            var instance = TestSingleton.Instance;
            Assert.IsNotNull(instance);

            // Act
            yield return null; // Wait a frame for DontDestroyOnLoad to take effect.

            // Assert
            // The instance should be in the DontDestroyOnLoad scene.
            Assert.AreEqual("DontDestroyOnLoad", instance.gameObject.scene.name, "Singleton instance on a root object should be marked as DontDestroyOnLoad.");
        }

        [UnityTest]
        public IEnumerator Instance_OnChildGameObject_IsNotDontDestroyOnLoad()
        {
            // Arrange
            // Create a parent GameObject
            var parentGo = new GameObject("Parent");
            // Create a child GameObject and add the singleton component
            var childGo = new GameObject("ChildSingleton");
            childGo.transform.SetParent(parentGo.transform);
            var instance = childGo.AddComponent<TestSingleton>();

            // Act
            // Access the instance to ensure Awake() has been called
            Assert.IsNotNull(TestSingleton.Instance);
            yield return null; // Wait a frame for scene processing.

            // Assert
            // The instance should be in the current test scene, not DontDestroyOnLoad scene.
            Assert.AreNotEqual("DontDestroyOnLoad", instance.gameObject.scene.name, "Singleton instance on a child object should not be marked as DontDestroyOnLoad.");
        }

        [UnityTest]
        public IEnumerator OnDestroy_NullifiesInstance()
        {
            // Arrange
            var instance = TestSingleton.Instance;
            Assert.IsNotNull(TestSingleton.Instance); // Ensure it's set

            // Act
            GameObject.DestroyImmediate(instance.gameObject);
            yield return null; // Wait for a frame for Unity to process destruction

            // Assert
            Assert.IsNull(Object.FindAnyObjectByType<TestSingleton>()); // No object in scene
            Assert.IsNotNull(TestSingleton.Instance); // A new instance should be created on next access
            Assert.IsTrue(instance.onDestroyCalled);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Awake_DestroysDuplicateInstance()
        {
            // Arrange
            var go1 = new GameObject("Singleton1");
            var instance1 = go1.AddComponent<TestSingleton>();
            // Access Instance to ensure it's set to instance1
            var _ = TestSingleton.Instance;

            var go2 = new GameObject("Singleton2");
            var instance2 = go2.AddComponent<TestSingleton>();

            // Act - Awake on instance2 will be called by Unity
            // We need to yield to allow Unity's Awake to process
            yield return null;

            // Assert
            Assert.IsNotNull(instance1);
            Assert.AreEqual(1, Object.FindObjectsByType<TestSingleton>(FindObjectsSortMode.None).Length); // Only one instance should exist
            Assert.AreSame(instance1, TestSingleton.Instance);
            yield return null;
        }
    }
}