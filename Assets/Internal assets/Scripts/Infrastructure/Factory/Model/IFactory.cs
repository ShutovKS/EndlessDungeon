using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Factory.Model
{
    public interface IFactory
    {
        public void DestroyInstance(GameObject instance);
        public void DestroyAllInstances();
        public void DestroyAllInstances<T>(List<T> list) where T : Object;
    }
}