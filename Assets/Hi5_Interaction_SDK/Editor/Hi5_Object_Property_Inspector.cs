using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hi5_Interaction_Core
{
    [CustomEditor(typeof(Hi5_Object_Property))]
    public class Hi5_Object_Property_Inspector : Editor
    {
        Hi5_Object_Property attack;
        void OnEnable()
        {
            attack = (Hi5_Object_Property)serializedObject.targetObject;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.BeginVertical();
            //AirFrictionRate
            //GUILayout.BeginHorizontal();
            //GUILayout.Label("AirFrictionRate");
            //string temp = GUILayout.TextField(attack.ObjectProperty.AirFrictionRate.ToString());
            ////string temp = attack.ObjectProperty.AirFrictionRate.ToString();
            //float tempAir;
            //bool isFloat = float.TryParse(temp,out tempAir);
            //if (isFloat)
            //{
            //    attack.ObjectProperty.AirFrictionRate = tempAir;
            //}
            //GUILayout.EndHorizontal();
            //AirMoveProperty
            GUILayout.BeginVertical();
            GUILayout.Label("AirMoveProperty");
            // GUILayout.Label("ConstraintsFreezeRotation");
            GUILayout.BeginHorizontal();
            GUILayout.Label("ConstraintsFreezeRotation");
            attack.ObjectProperty.AirMoveProperty.ConstraintsFreezeRotation = GUILayout.Toggle(attack.ObjectProperty.AirMoveProperty.ConstraintsFreezeRotation, "");
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            //StaticProperty
            GUILayout.BeginVertical();
            GUILayout.Label("StaticProperty");
            GUILayout.BeginHorizontal();
            GUILayout.Label("ConstraintsFreezeRotation");
            //GUILayout.Label("ConstraintsFreezeRotation");
            attack.ObjectProperty.StaticProperty.ConstraintsFreezeRotation = GUILayout.Toggle(attack.ObjectProperty.StaticProperty.ConstraintsFreezeRotation, "");
            GUILayout.EndHorizontal();
            //GUILayout.BeginHorizontal();
            //attack.ObjectProperty.StaticProperty.ConstraintsFreezeRotationX = GUILayout.Toggle(attack.ObjectProperty.StaticProperty.ConstraintsFreezeRotationX, "X");
            //attack.ObjectProperty.StaticProperty.ConstraintsFreezeRotationY = GUILayout.Toggle(attack.ObjectProperty.StaticProperty.ConstraintsFreezeRotationY, "Y");
            //attack.ObjectProperty.StaticProperty.ConstraintsFreezeRotationZ = GUILayout.Toggle(attack.ObjectProperty.StaticProperty.ConstraintsFreezeRotationZ, "Z");
            //GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            //PlaneMoveProperty
            GUILayout.BeginVertical();
            GUILayout.Label("PlaneMoveProperty");
            GUILayout.BeginHorizontal();
            GUILayout.Label("ConstraintsFreezeRotation");
            attack.ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotation = GUILayout.Toggle(attack.ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotation, "");
            //attack.ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotationX = GUILayout.Toggle(attack.ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotationX, "X");
            //attack.ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotationY = GUILayout.Toggle(attack.ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotationY, "Y");
            //attack.ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotationZ = GUILayout.Toggle(attack.ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotationZ, "Z");
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            //IsPinch
            GUILayout.BeginHorizontal();
            GUILayout.Label("IsPinch");
            attack.ObjectProperty.IsPinch = GUILayout.Toggle(attack.ObjectProperty.IsPinch, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("IsLift  ");
            attack.ObjectProperty.IsLift = GUILayout.Toggle(attack.ObjectProperty.IsLift, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("IsClap ");
            attack.ObjectProperty.IsClap = GUILayout.Toggle(attack.ObjectProperty.IsClap, "");
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
        private void ProgressBar(float value, string label)
        {
            Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
            EditorGUI.ProgressBar(rect, value, label);
            EditorGUILayout.Space();
        }
    }
}

