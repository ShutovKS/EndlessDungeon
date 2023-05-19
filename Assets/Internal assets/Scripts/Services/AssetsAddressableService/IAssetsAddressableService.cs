using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Services.AssetsAddressableService
{
    public interface IAssetsAddressableService
    {
        public Task<T> GetAsset<T>(string path) where T : Object;
        public Task<T> GetAsset<T>(AssetReference assetReference) where T : Object;
    }
}

