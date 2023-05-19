using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Services.AssetsAddressableService
{
    public class AssetsAddressableService : IAssetsAddressableService
    {
        public async Task<T> GetAsset<T>(string path) where T : Object
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(path);

            await asyncOperationHandle.Task;

            return asyncOperationHandle.Result;
        }
        
        public async Task<T> GetAsset<T>(AssetReference assetReference) where T : Object
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(assetReference.AssetGUID); 
            
            await asyncOperationHandle.Task; 
 
            return asyncOperationHandle.Result; 
        }
    }
}