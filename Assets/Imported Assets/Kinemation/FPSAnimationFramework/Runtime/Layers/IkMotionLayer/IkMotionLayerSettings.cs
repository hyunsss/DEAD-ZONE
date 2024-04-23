﻿// Designed by KINEMATION, 2024.

using KINEMATION.FPSAnimationFramework.Runtime.Core;
using KINEMATION.KAnimationCore.Runtime.Core;
using KINEMATION.KAnimationCore.Runtime.Rig;
using UnityEngine;

namespace KINEMATION.FPSAnimationFramework.Runtime.Layers.IkMotionLayer
{
    public class IkMotionLayerSettings : FPSAnimatorLayerSettings
    {
        public KRigElement boneToAnimate;

        public VectorCurve rotationCurves = new VectorCurve(new Keyframe[]
        {
            new Keyframe(0f, 0f),
            new Keyframe(1f, 0f)
        });
        
        public VectorCurve translationCurves = new VectorCurve(new Keyframe[]
        {
            new Keyframe(0f, 0f),
            new Keyframe(1f, 0f)
        });

        public Vector3 rotationScale = Vector3.one;
        public Vector3 translationScale = Vector3.one;
        
        [Range(0f, 1f)] public float blendTime = 0f;
        [Range(0f, 2f)] public float playRate = 1f;

        public override FPSAnimatorLayerState CreateState()
        {
            return new IkMotionLayerState();
        }

#if UNITY_EDITOR
        public override void OnRigUpdated()
        {
            UpdateRigElement(ref boneToAnimate);
        }
#endif
    }
}