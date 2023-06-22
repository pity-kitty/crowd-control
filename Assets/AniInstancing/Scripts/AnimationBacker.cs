using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class AnimationBacker : MonoBehaviour
{
    public ComputeShader infoTexGen;
    public Shader playShader;

    public struct VertInfo
    {
        public Vector3 position;
        public Vector3 normal;
    }

    // Use this for initialization
    void Start()
    {
        var animator = GetComponent<Animator>();
        var skin = GetComponentInChildren<SkinnedMeshRenderer>();
        var vCount = skin.sharedMesh.vertexCount;
        var texWidth = Mathf.NextPowerOfTwo(vCount);
        var mesh = new Mesh();

        int clipInfoCount = animator.GetCurrentAnimatorClipInfoCount(0);
        Debug.Log("............animation info count..................." + clipInfoCount);
        for (int i = 0; i < clipInfoCount; i++)
        {
            var info = animator.GetCurrentAnimatorClipInfo(i);
            Debug.Log("............animation clip..................." + info[0].clip);
            Debug.Log("............animation weight..................." + info[0].weight);
        }

    }
}