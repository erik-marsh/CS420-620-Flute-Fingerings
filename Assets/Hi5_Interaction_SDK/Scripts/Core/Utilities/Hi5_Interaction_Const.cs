using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
namespace Hi5_Interaction_Core
{
    public class Hi5_Interaction_Const {
		public static string m_Path = "C:\\ProgramData\\Noitom\\hi5_sdk";//System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\HI5";
		public static string m_Name = "\\hi5_interaction_object_move.xml";
		public static string m_Name_1 = "\\hi5_interaction_object_position.xml";
        public static float PRECISION = 0.000001f;
        public static float FingerPinchReleaseAngle = 140.0f;
        public static float FingerPalmPinchAngle = 100.0f;
        public static float FingerCollisionSaveTime = 0.4f;
        public static float FingerFlyPinchAngle = 125.0f;
        public static float PalmPinchFingerAngle = 120.0f;
        public static float FingerClapFourMinAngle = -15.0f;
        public static float FingerClapFourMaxAngle = 25.0f;
        public static float FingerClapThreeMinAngle = -15.0f;
        public static float FingerClapThreeMaxAngle = 13.0f;
        public static float FingerClapTwoMinAngle = -15.0f;
        public static float FingerClapTwoMaxAngle = 10.0f;
        public static float FingerPinch2Release = 10.0f;
        public static float FingerColliderPinchPauseTime = 0.6f;
        public static float ThrowObjectProtectionDistance = 0.055f;
        //public static float FingerPinchPauseProtectionTime = 0.1f;
        public static float ThrowSpeed = 1.0f;
        public static float Clapcd = 1.0f;
        public static float liftChangeMoveDistance = 0.20f;
		public static float PokeProtectionCd = 0.5f;
        //释放权重值
        public static int ObjectPinchRecordPositionCount = 10;
       // public static int ObjectColliderRecordPostionCount = 5;
        public static int[] RecordPositionWeight = new int[] { 1,2,3,4,5,6,7,8,9,10};


		public static int PlaneLayer()
		{
			return LayerMask.NameToLayer ("Hi5Plane");
		}
		public static int ObjectGrasp()
		{
			return LayerMask.NameToLayer ("Hi5ObjectGrasp");
		}
//		public static int ThumbTailLayer()
//		{
//			return LayerMask.NameToLayer ("Hi5ThumbTail");
//		}
//
//		public static int ThumbSecondLayer()
//		{
//			return LayerMask.NameToLayer ("Hi5ThumbSecond");
//		}
//
//		public static int ThumbOtherLayer()
//		{
//			return LayerMask.NameToLayer ("Hi5ThumbOther");
//		}
		public static int OtherFingerTailLayer()
		{
			return LayerMask.NameToLayer ("Hi5OtherFingerTail");
		}
		
		public static int OtherFingerOtherLayer()
		{
			return LayerMask.NameToLayer ("Hi5OtherFingerOther");
		}
		public static int ObjectTriggerLayer()
		{
			return LayerMask.NameToLayer ("Hi5ObjectTrigger");
		}
       // public static int ThumbTailLayer = 8;
       // public static int ThumbSecondLayer = 9;
       // public static int ThumbOtherLayer = 10;
        //public static int OtherFingerTailLayer = 11;
       // public static int OtherFingerSecondLayer = 12;
       // public static int OtherFingerOtherLayer = 13;
        //public static int ObjectTriggerLayer = 17;
//		public static int PalmLayer()
//		{
//			return LayerMask.NameToLayer ("Hi5Palm");
//		}
       // public static int PalmLayer = 14;
//		public static int PalmRigidbodyLayer()
//		{
//			return LayerMask.NameToLayer ("Hi5PalmRigidbody");
//		}
       // public static int PalmRigibodyLayer = 20;

        public static float FlyLiftPalmAngle = 25.0f;
		public static float PlaneFrictionRateConst = 0.3f;
        //public static bool TestAssembly = true;
       // public static bool TestModifyConstraints = true;
        //public static bool TestLiftRelease = true;
        public static bool TestPinchOpenCollider = true;
        public static bool IsUseVisibleHand = false;
        public static bool TestPinch2 = true;
       
        static float GetAngle(Transform leftTransform, Transform midTransform, Transform rightTransform)
        {
            Vector3 left = leftTransform.position;
            Vector3 middle = midTransform.position;
            Vector3 right = rightTransform.position; // finger tip
            Vector3 vLeft = left - middle;
            Vector3 vRight = right - middle;
            float angle = Vector3.Angle(vLeft, vRight);
            // float angle1 = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(vLeft, vRight) / (vLeft.magnitude * vRight.magnitude));
            return angle;
        }

        public static float GetDistance(Transform palm, Hi5_Glove_Interaction_Item ObjectItem,out bool ContactIsSelf)
        {
            if (palm == null || ObjectItem == null)
            {
                ContactIsSelf = false;
                return 0.0f;
            }
            Ray ray = new Ray(palm.position, ObjectItem.transform.position - palm.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
				if (hit.transform == ObjectItem.transform || hit.transform.parent == ObjectItem.transform)
                {
                    ContactIsSelf = true;
                    float distance = Vector3.Distance(palm.position, hit.point);
                    return distance;
                }
                else
                {
                    ContactIsSelf = false;
                    return 0.0f;
                }
            }
            else
            {
                ContactIsSelf = false;
                return 0.0f;
            }
        }

		public static void WriteItemMovePositionXml(Queue<Hi5_Position_Record> records,Hi5_Object_Move.ObjectMoveData objectData)
		{
			string temp = m_Path + m_Name_1;
			if(File.Exists(temp))
				File.Delete(temp);
			XmlDocument xmlDoc = new XmlDocument ();
			XmlDeclaration xmlDeclar;
			xmlDeclar = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
			xmlDoc.AppendChild(xmlDeclar);          

			XmlElement xmlElement = xmlDoc.CreateElement("", "Itemthrow", "");
			xmlDoc.AppendChild(xmlElement);        

			XmlNode root = xmlDoc.SelectSingleNode("Itemthrow");

			if (records != null && records.Count > 0)
			{
				XmlElement xe1 = xmlDoc.CreateElement("Hi5_Position_Record");	
				foreach(Hi5_Position_Record item in records)
				{
					XmlElement xeSub1 = xmlDoc.CreateElement("RecordItem");
					xeSub1.SetAttribute("MoveVectorX", item.mMoveVector.x.ToString());
					xeSub1.SetAttribute("MoveVectorY", item.mMoveVector.y.ToString());
					xeSub1.SetAttribute("MoveVectorZ", item.mMoveVector.z.ToString());
					xeSub1.SetAttribute("PositionX", item.position.x.ToString());
					xeSub1.SetAttribute("PositionY", item.position.y.ToString());
					xeSub1.SetAttribute("PositionZ", item.position.z.ToString());
					xeSub1.SetAttribute("IntervalTime", item.mIntervalTime.ToString());
					xe1.AppendChild (xeSub1);
				}
				root.AppendChild(xe1);


			}
			if (objectData != null)
			{
				XmlElement xe2 = xmlDoc.CreateElement("ObjectMoveData");	
				XmlElement xeSub1 = xmlDoc.CreateElement("ObjectMoveDataItem");
				xeSub1.SetAttribute("DirectionX", objectData.mDirection.x.ToString());
				xeSub1.SetAttribute("DirectionY", objectData.mDirection.y.ToString());
				xeSub1.SetAttribute("DirectionZ", objectData.mDirection.z.ToString());
				xeSub1.SetAttribute("Y", objectData.y.ToString());
				xe2.AppendChild (xeSub1);
				root.AppendChild(xe2);
			}
			xmlDoc.Save(m_Path+m_Name);
		}

		public static void WriteItemMoveXml(Queue<Hi5_Position_Record> records,Hi5_Object_Move.ObjectMoveData objectData)
		{
			return;
			string temp = m_Path + m_Name;
			if(File.Exists(temp))
				File.Delete(temp);
			XmlDocument xmlDoc = new XmlDocument ();
			XmlDeclaration xmlDeclar;
			xmlDeclar = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
			xmlDoc.AppendChild(xmlDeclar);          

			XmlElement xmlElement = xmlDoc.CreateElement("", "Itemthrow", "");
			xmlDoc.AppendChild(xmlElement);        

			XmlNode root = xmlDoc.SelectSingleNode("Itemthrow");

			if (records != null && records.Count > 0)
			{
				XmlElement xe1 = xmlDoc.CreateElement("Hi5_Position_Record");	
				foreach(Hi5_Position_Record item in records)
				{
					XmlElement xeSub1 = xmlDoc.CreateElement("RecordItem");
					xeSub1.SetAttribute("MoveVectorX", item.mMoveVector.x.ToString());
					xeSub1.SetAttribute("MoveVectorY", item.mMoveVector.y.ToString());
					xeSub1.SetAttribute("MoveVectorZ", item.mMoveVector.z.ToString());
					xeSub1.SetAttribute("PositionX", item.position.x.ToString());
					xeSub1.SetAttribute("PositionY", item.position.y.ToString());
					xeSub1.SetAttribute("PositionZ", item.position.z.ToString());
					xeSub1.SetAttribute("IntervalTime", item.mIntervalTime.ToString());
					xe1.AppendChild (xeSub1);
				}
				root.AppendChild(xe1);


			}
			if (objectData != null)
			{
				XmlElement xe2 = xmlDoc.CreateElement("ObjectMoveData");	
				XmlElement xeSub1 = xmlDoc.CreateElement("ObjectMoveDataItem");
				xeSub1.SetAttribute("DirectionX", objectData.mDirection.x.ToString());
				xeSub1.SetAttribute("DirectionY", objectData.mDirection.y.ToString());
				xeSub1.SetAttribute("DirectionZ", objectData.mDirection.z.ToString());
				xeSub1.SetAttribute("Y", objectData.y.ToString());
				xe2.AppendChild (xeSub1);
				root.AppendChild(xe2);
			}
			xmlDoc.Save(m_Path+m_Name);
			/*if (leftLeapMotionScripe != null) {
				XmlElement xe1 = xmlDoc.CreateElement("Hand");
				xe1.SetAttribute("DeviceName", leftLeapMotionScripe.DeviceName);
				xe1.SetAttribute("GloveSide", leftLeapMotionScripe.handSide.ToString());        
				for (int i = 0; i < leftLeapMotionScripe.leftPosition.Length; i++) 
				{
					XmlElement xeSub1 = xmlDoc.CreateElement("Position");
					xeSub1.SetAttribute("X", leftLeapMotionScripe.leftPosition[i].x.ToString());
					xeSub1.SetAttribute("Y", leftLeapMotionScripe.leftPosition[i].y.ToString());
					xeSub1.SetAttribute("Z", leftLeapMotionScripe.leftPosition[i].z.ToString());
					xe1.AppendChild (xeSub1);
				}
				root.AppendChild(xe1);
			}
			if (rightLeapMotionScripe != null) {
				XmlElement xe1 = xmlDoc.CreateElement("Hand");
				xe1.SetAttribute("DeviceName", rightLeapMotionScripe.DeviceName);
				xe1.SetAttribute("GloveSide", rightLeapMotionScripe.handSide.ToString());        
				for (int i = 0; i < rightLeapMotionScripe.rightPosition.Length; i++) 
				{
					XmlElement xeSub1 = xmlDoc.CreateElement("Position");
					xeSub1.SetAttribute("X", rightLeapMotionScripe.rightPosition[i].x.ToString());
					xeSub1.SetAttribute("Y", rightLeapMotionScripe.rightPosition[i].y.ToString());
					xeSub1.SetAttribute("Z", rightLeapMotionScripe.rightPosition[i].z.ToString());
					xe1.AppendChild (xeSub1);
				}
				root.AppendChild(xe1);
			}
			//保存的路径*/
		}

    }
}
