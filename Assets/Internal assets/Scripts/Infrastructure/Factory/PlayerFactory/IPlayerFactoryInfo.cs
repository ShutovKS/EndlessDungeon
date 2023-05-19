using UnityEngine;

namespace Infrastructure.Factory.PlayerFactory
{
    public interface IPlayerFactoryInfo
    {
        GameObject PlayerInstance { get; }
    }
}