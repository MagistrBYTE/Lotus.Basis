#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Lotus.Core
{
    /// <summary>
    /// Статический класс для тестирования методов подсистемы ECS базового ядра.
    /// </summary>
    public static class XCoreECSTesting
    {
        /// <summary>
        /// Служебный класс для тестирования.
        /// </summary>
        public struct TWeapon
        {
            public int Fire;
        }

        /// <summary>
        /// Служебный класс для тестирования.
        /// </summary>
        public struct THealth
        {
            public int Live;
        }

        /// <summary>
        /// Служебный класс для тестирования.
        /// </summary>
        public struct TPlayer
        {
            public int Id;
        }

        /// <summary>
        /// Служебный класс для тестирования.
        /// </summary>
        public struct TDeadStatus
        {
            public bool IsDead;
        }

        /// <summary>
        /// Тестирование методов подсистемы ECS.
        /// </summary>
        [Test]
        public static void TestECS()
        {
            var world = new CEcsWorld();

            var pety = world.NewEntity();
            var sany = world.NewEntity();
            var igor = world.NewEntity();

            world.AddComponent<TWeapon>(pety.Id);
            world.AddComponent<THealth>(pety.Id);
            world.AddComponent<TPlayer>(pety.Id);

            world.AddComponent<TWeapon>(sany.Id);
            world.AddComponent<THealth>(sany.Id);

            world.AddComponent<THealth>(igor.Id);
            world.AddComponent<TPlayer>(igor.Id);

            var filter_health = world.CreateFilterComponent();
            filter_health.Include<THealth>().Include<TPlayer>();

            var filter_entities = filter_health.GetEntities();
            ClassicAssert.AreEqual(filter_health.CountEntities, 2);
            for (var i = 0; i < filter_health.CountEntities; i++)
            {
                ref var health = ref world.GetComponent<THealth>(filter_entities[i]);
                health.Live = 325;

                ref var player = ref world.GetComponent<TPlayer>(filter_entities[i]);
                player.Id = 17;
            }

            filter_health.Include<TDeadStatus>();
            filter_entities = filter_health.GetEntities();
            ClassicAssert.AreEqual(filter_health.CountEntities, 0);
            for (var i = 0; i < filter_health.CountEntities; i++)
            {
                ref var health = ref world.GetComponent<THealth>(filter_entities[i]);
                health.Live = 325;

                ref var player = ref world.GetComponent<TPlayer>(filter_entities[i]);
                player.Id = 17;
            }

            ClassicAssert.AreEqual(world.HasComponent<TWeapon>(pety.Id), true);
            ClassicAssert.AreEqual(world.HasComponent<THealth>(pety.Id), true);
            ClassicAssert.AreEqual(world.HasComponent<TPlayer>(pety.Id), true);

            ClassicAssert.AreEqual(world.HasComponent<TWeapon>(sany.Id), true);
            ClassicAssert.AreEqual(world.HasComponent<THealth>(sany.Id), true);

            ClassicAssert.AreEqual(world.HasComponent<THealth>(igor.Id), true);
            ClassicAssert.AreEqual(world.HasComponent<TPlayer>(igor.Id), true);
        }
    }
}