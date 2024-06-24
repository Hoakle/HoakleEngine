using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace HoakleEngine.Addons
{
    public class MeshTrail : MonoBehaviour
    {
        [SerializeField] private Transform _CharacterTransform;
        [SerializeField] private List<SkinnedMeshRenderer> _skinnedMeshRenderers;
        [SerializeField] private Material _Materail;
        [SerializeField] private string _ShaderValue;
        [SerializeField] private float _MeshRefreshRate = 0.2f;
        [SerializeField] private float _MeshDestroyDelay = 2f;

        [HideInInspector] public float Speed;
        
        private bool _IsActive;
        private float _LastActivationTime;

        public bool IsActive
        {
            set => _IsActive = value;
        }

        private void Update()
        {
            if (!_IsActive)
                return;

            if (Time.time - _LastActivationTime > _MeshRefreshRate)
            {
                foreach (var skin in _skinnedMeshRenderers)
                {
                    GameObject obj = new GameObject();
                    MeshRenderer mr = obj.AddComponent<MeshRenderer>();
                    MeshFilter mf = obj.AddComponent<MeshFilter>();

                    obj.transform.SetPositionAndRotation(skin.transform.position, skin.transform.rotation);

                    Mesh mesh = new Mesh();
                    skin.BakeMesh(mesh);
                    mf.mesh = mesh;
                    mr.material = _Materail;

                    StartCoroutine(MoveMesh(obj.transform));
                    StartCoroutine(FadeOut(mr.material));
                    
                    Destroy(obj, _MeshDestroyDelay);
                }
                _LastActivationTime = Time.time;
            }
        }

        private IEnumerator MoveMesh(Transform transform)
        {
            var waiter = new WaitForEndOfFrame();
            var transformPosition = transform.position;
            
            while (transform != null)
            {
                transformPosition.z -= Speed;
                transform.position = transformPosition;
                yield return waiter;
            }
        }
        
        private IEnumerator FadeOut(Material mat)
        {
            var waiter = new WaitForEndOfFrame();
            float valueToAnimate = mat.GetFloat(_ShaderValue);
            float iniatialValue = valueToAnimate;
            float fadeOutDuration = 0f;
            
            while (valueToAnimate > 0f && mat != null)
            {
                valueToAnimate = Mathf.Lerp(iniatialValue, 0, fadeOutDuration / _MeshDestroyDelay);
                fadeOutDuration += Time.deltaTime;
                mat.SetFloat(_ShaderValue, valueToAnimate);
                yield return waiter;
            }
        }
    }
}
