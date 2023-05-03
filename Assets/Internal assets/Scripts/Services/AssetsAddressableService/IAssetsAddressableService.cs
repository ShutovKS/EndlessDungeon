using System.Threading.Tasks;
using UnityEngine;

namespace Services.AssetsAddressableService
{
    public interface IAssetsAddressableService
    {
        public Task<T> GetAsset<T>(string path) where T : Object;
    }
}

