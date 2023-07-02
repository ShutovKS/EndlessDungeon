#region

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

namespace Dummy
{
    public class DummyUpdateCollider : MonoBehaviour
    {
        private Mesh _mesh;
        private MeshCollider _meshCollider;
        private MeshFilter _meshFilter;
        private SkinnedMeshRenderer _skinnedMeshRenderer;
        private List<Collider> CollidersInChildrenNonTriggers { get; } = new List<Collider>();

        private void Awake()
        {
            _meshCollider = GetComponent<MeshCollider>();
            _meshFilter = GetComponentInChildren<MeshFilter>();
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

            _mesh = new Mesh();

            FillAllCollidersInChildrenNonTriggers();
        }

        private void Start()
        {
            StartCoroutine(UpdateNewMesh());
        }

        public void AnimationTriggerAction()
        {
            _meshCollider.enabled = false;
            foreach (var childrenCollider in CollidersInChildrenNonTriggers)
            {
                childrenCollider.enabled = true;
            }
        }

        public void AnimationTriggerIdle()
        {
            _meshCollider.enabled = true;
            foreach (var childrenCollider in CollidersInChildrenNonTriggers)
            {
                childrenCollider.enabled = false;
            }
        }

        private IEnumerator UpdateNewMesh()
        {
            _skinnedMeshRenderer.BakeMesh(_mesh);

            _meshFilter.mesh = _mesh;
            _meshFilter.mesh.RecalculateBounds();

            yield return new WaitForSeconds(0.1f);

            _meshCollider.sharedMesh = _mesh;
            _meshCollider.sharedMesh.RecalculateBounds();
        }

        private void FillAllCollidersInChildrenNonTriggers()
        {
            var colliders = transform.GetComponentsInChildren<Collider>();

            foreach (var thisCollider in colliders
                .Where(thisCollider => thisCollider is not MeshCollider)
                .Where(thisCollider => thisCollider.isTrigger == false))
            {
                CollidersInChildrenNonTriggers.Add(thisCollider);
            }
        }
    }
}
