using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
	/**
     * Enumeration of hand types.
    **/
    public enum EHandType
    {
        ENone = 0,
        EHandLeft,
        EHandRight
    }
	/**
     * Enumeration of object  types.
    **/
    public enum EObject_Type
    {
        ECommon = 0,
        EButton = 1
    }
	/**
     * Enumeration of object event types.
    **/
    public enum EEventObjectType
    {
        ENone = 0,
        EClap,
        EPoke,
        EPinch,
        EMove,
        ELift,
        EStatic
    }
	/**
     * Enumeration of hand event types.
    **/
    public enum EEventHandType
    {
        ENone = 0,
        EClap,
		EPoke,
        EPinch,
        EThrow,
        ELift,
        ERelease,
    }
	/**
     * Enumeration of objedct state.
    **/
    public enum E_Object_State
    {
        EStatic = -1,
        EPinch = 3,
        EMove = 2,
        EClap = 4,
        EFlyLift = 5,
		EPoke = 6,
    }
	/**
     * Enumeration of hand state interface.
    **/
	public enum E_Interface_Hand_State
	{
		ERelease = -1,
		EPinch = 2,
		ELift = 4,
	}

  

    public class Hi5_Glove_Interaction_Object_Event_Data
    {
        public int mObjectId = -1;
        public EObject_Type mObjectType = EObject_Type.ECommon;
        public EHandType mHandType = EHandType.EHandLeft;
        public EEventObjectType mEventType = EEventObjectType.ENone;

        private Hi5_Glove_Interaction_Object_Event_Data()
        {

        }

        public static Hi5_Glove_Interaction_Object_Event_Data Instance(int ObjectId, EObject_Type objectType, EHandType handType, EEventObjectType eventType)
        {
            Hi5_Glove_Interaction_Object_Event_Data data = new Hi5_Glove_Interaction_Object_Event_Data();
            data.mObjectId = ObjectId;
            data.mObjectType = objectType;
            data.mHandType = handType;
            data.mEventType = eventType;
            return data;
        }

    }

    public class Hi5_Glove_Interaction_Hand_Event_Data
    {
        public int mObjectId = -1;
        public EEventHandType mEventType = EEventHandType.ENone;
        public EHandType mHandType = EHandType.EHandLeft;
        private Hi5_Glove_Interaction_Hand_Event_Data()
        {

        }
        public static Hi5_Glove_Interaction_Hand_Event_Data Instance(int ObjectId, EHandType handType, EEventHandType eventType)
        {
            Hi5_Glove_Interaction_Hand_Event_Data data = new Hi5_Glove_Interaction_Hand_Event_Data();
            data.mObjectId = ObjectId;
            data.mHandType = handType;
            data.mEventType = eventType;
            return data;
        }
    }
}
