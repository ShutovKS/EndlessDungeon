using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dummy
{
    public class DummyUpdateMesh : MonoBehaviour
    {
        private Mesh _mesh;
        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;
        private SkinnedMeshRenderer _skinnedMeshRenderer;

        private Collider[] _childrenColliders;

        private void Awake()
        {
            _meshCollider = GetComponent<MeshCollider>();
            _meshFilter = GetComponentInChildren<MeshFilter>();
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

            _mesh = new Mesh();

            _childrenColliders = GetAllCollidersInChildren(transform);
        }

        private void Start()
        {
            StartCoroutine(UpdateNewMesh());
        }

        public void Action()
        {
            _meshCollider.enabled = false;
            foreach (var collider in _childrenColliders)
            {
                collider.enabled = true;
            }
        }

        public void Idle()
        {
            _meshCollider.enabled = true;
            foreach (var collider in _childrenColliders)
            {
                collider.enabled = false;
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

        private Collider[] GetAllCollidersInChildren(Transform parentTransform)
        {
            var colliders = new List<Collider>();

            var childCount = parentTransform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var childTransform = parentTransform.GetChild(i);

                var childColliders = childTransform.GetComponents<Collider>();
                if (childColliders != null && childColliders.Length > 0)
                {
                    colliders.AddRange(childColliders);
                }

                if (childTransform.childCount > 0)
                {
                    var childCollidersInChildren = GetAllCollidersInChildren(childTransform);

                    if (childCollidersInChildren != null && childCollidersInChildren.Length > 0)
                    {
                        colliders.AddRange(childCollidersInChildren);
                    }
                }
            }

            return colliders.ToArray();
        }
    }
}