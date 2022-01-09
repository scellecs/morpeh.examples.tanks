using System.Collections;
using Morpeh;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tanks {
    [TestFixture]
    public abstract class EcsTestFixture {
        protected World testWorld;

        private SystemsGroup testSystems;

        [SetUp]
        public void FixtureSetUp() {
            testWorld = World.Create();
            testWorld.UpdateByUnity = false;

            testSystems = testWorld.CreateSystemsGroup();
            InitSystems(testSystems);
        }

        [TearDown]
        public void FixtureTearDown() {
            testSystems.Dispose();
            testSystems = null;

            testWorld.Dispose();
            testWorld = null;
        }

        [UnityTearDown]
        public IEnumerator SceneTearDown() {
            var scene = SceneManager.GetActiveScene();
            foreach (var o in scene.GetRootGameObjects()) {
                if (o.name.EndsWith("tests runner")) continue;

                Object.Destroy(o);
            }

            yield return null;
        }

        protected abstract void InitSystems(SystemsGroup systemsGroup);

        protected void RegisterSystem(ISystem system) {
            testSystems.AddSystem(system);
        }

        protected void RunFixedSystems() {
            testWorld.FixedUpdate(Time.fixedDeltaTime);
            testWorld.UpdateFilters();
        }

        protected void RunUpdateSystems(float dt) {
            testWorld.Update(dt);
            testWorld.UpdateFilters();
        }

        protected void RunLateUpdateSystems(float dt) {
            testWorld.LateUpdate(dt);
            testWorld.UpdateFilters();
        }
    }
}