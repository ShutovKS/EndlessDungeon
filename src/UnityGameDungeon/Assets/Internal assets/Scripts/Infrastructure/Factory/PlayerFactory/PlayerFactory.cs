#region

using UnityEngine;
using Zenject;

#endregion

namespace Infrastructure.Factory.PlayerFactory
{
    public class PlayerFactory : IPlayerFactory
    {
        public PlayerFactory(DiContainer container)
        {
            _container = container;
        }

        private readonly DiContainer _container;

        public GameObject PlayerInstance { get; private set; }

        public GameObject CreatePlayer(GameObject prefab, Vector3 position)
        {
            var instance = _container.InstantiatePrefab(prefab, position, Quaternion.identity, null);

            PlayerInstance = instance;

            return instance;
        }

        public void DestroyPlayer()
        {
            Object.Destroy(PlayerInstance);
        }
    }
}
