#region

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Services.AssetsAddressableService
{
    public interface IAssetsAddressableService
    {
        Task<T> GetAsset<T>(string path) where T : Object;
        Task<T> GetAsset<T>(AssetReference assetReference) where T : Object;
    }
}
