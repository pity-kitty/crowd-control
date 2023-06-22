using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Animations;
public class AnimatorClipInfoEx
{
    public Vector3[] velocity;
    public Vector3[] angularVelocity;
    public WrapMode wrapMode;
    public List<AnimationEvent> eventList;
    public int totalFrame;
    public int animationNameHash;
    public int animationIndex;
    public int animHashName;
    public int fps;
    public int textureIndex;
    public string animStrName;

    public bool rootMotion;
}