using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
namespace Hi5_Interaction_Core
{
	public class Hi5_Layer_Set : MonoBehaviour {


		public static bool IsResetLayer = false;
		// Use this for initialization
		void Awake () {
//			if (!IsHasLayer ("Hi5OtherFingerTail")) 
//			{
//				AddLayer("Hi5OtherFingerTail");
//				AddLayer("Hi5OtherFingerOther");
//				AddLayer ("Hi5Palm");
//				AddLayer ("Hi5ObjectGrasp");
//				AddLayer ("Hi5Plane");
//				AddLayer ("Hi5ObjectTigger");
//			}
//
//
//			int Hi5OtherFingerTail = LayerMask.NameToLayer ("Hi5OtherFingerTail");
//			int Hi5OtherFingerOther = LayerMask.NameToLayer ("Hi5OtherFingerOther");
//			int Hi5Palm = LayerMask.NameToLayer ("Hi5Palm");
//			int Hi5ObjectGrasp = LayerMask.NameToLayer ("Hi5ObjectGrasp");
//			int Hi5Plane = LayerMask.NameToLayer ("Hi5Plane");
//			int Hi5ObjectTigger = LayerMask.NameToLayer ("Hi5ObjectTigger");
//			Physics.IgnoreLayerCollision (Hi5OtherFingerTail,Hi5OtherFingerOther,true);
//			Physics.IgnoreLayerCollision (Hi5OtherFingerTail,Hi5OtherFingerTail,true);
//			Physics.IgnoreLayerCollision (Hi5OtherFingerTail,Hi5Palm,true);
//			Physics.IgnoreLayerCollision (Hi5OtherFingerTail,Hi5Plane,true);
//			Physics.IgnoreLayerCollision (Hi5OtherFingerTail,Hi5ObjectGrasp,false);
//			Physics.IgnoreLayerCollision (Hi5OtherFingerTail,Hi5ObjectTigger,false);
//
//			Physics.IgnoreLayerCollision (Hi5OtherFingerOther,Hi5Palm,true);
//			Physics.IgnoreLayerCollision (Hi5OtherFingerOther,Hi5OtherFingerOther,true);
//			Physics.IgnoreLayerCollision (Hi5OtherFingerOther,Hi5Plane,true);
//			Physics.IgnoreLayerCollision (Hi5OtherFingerOther,Hi5ObjectGrasp,false);
//			Physics.IgnoreLayerCollision (Hi5OtherFingerOther,Hi5ObjectTigger,false);
//
//			Physics.IgnoreLayerCollision (Hi5Palm,Hi5Plane,true);
//			Physics.IgnoreLayerCollision (Hi5Palm,Hi5Palm,true);
//			Physics.IgnoreLayerCollision (Hi5Palm,Hi5ObjectGrasp,false);
//			Physics.IgnoreLayerCollision (Hi5Palm,Hi5ObjectTigger,false);
//
//			Physics.IgnoreLayerCollision (Hi5Plane,Hi5Plane,true);
//			Physics.IgnoreLayerCollision (Hi5Plane,Hi5ObjectGrasp,false);
//			Physics.IgnoreLayerCollision (Hi5Plane,Hi5ObjectTigger,false);
//			Physics.IgnoreLayerCollision (Hi5ObjectGrasp,Hi5ObjectTigger,true);
//			Hi5_Layer_Set.IsResetLayer = true;
		}

		// Update is called once per frame
		void Update () {

		}

		private static bool IsHasLayer(string layerName)
		{
//			int layerId = LayerMask.NameToLayer (layerName);
//			if (layerId > 1)
//				return true;
//			else
//				return false;
//			for(int i=0;i<UnityEditorInternal.InternalEditorUtility.layers[i].Length;i++)
//			{
//				if(UnityEditorInternal.InternalEditorUtility.layers[i].Contains(layerName))
//				{
//					return true;
//				}
//			}
			return false;
		}

		static void AddLayer(string layer)
		{
			//if (!IsHasLayer(layer))
			{
//				SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
//				SerializedProperty it = tagManager.GetIterator();
//				while (it.NextVisible(true))
//				{
//					if (it.name== "layers")
//					{
//						for (int i = 0; i < it.arraySize; i++)
//						{
//							if (i==3||i == 6||i==7) continue;
//							SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
//							if (string.IsNullOrEmpty(dataPoint.stringValue))
//							{
//								dataPoint.stringValue = layer;
//								tagManager.ApplyModifiedProperties();
//								return;
//							}
//						}
//					}
//				}
			}
		}
	}

}
