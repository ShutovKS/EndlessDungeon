#region

using UnityEngine;

#endregion

namespace Infrastructure.Factory.PlayerFactory
{
    public interface IPlayerFactoryInfo
    {
        GameObject PlayerInstance { get; }
    }
}
