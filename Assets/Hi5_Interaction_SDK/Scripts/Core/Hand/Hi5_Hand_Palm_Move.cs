using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Hand_Palm_Move : Hi5_Glove_Interaction_Collider, Hi5_Record_Interface
    {
        internal Hi5_Hand_Visible_Hand mHand;
        protected Hi5_Record mRecord = new Hi5_Record();

        override protected void Awake()
        {
            base.Awake();
            mHand = transform.parent.parent.parent.GetComponent<Hi5_Hand_Visible_Hand>();
          
        }


		internal  Hi5_Record GetHi5Record()
		{
			return mRecord;
		}

        public Queue<Hi5_Position_Record> GetRecord()
        {
            return mRecord.GetRecord() ;
        }

        protected void Update()
        {
           
            mRecord.RecordPosition(Time.deltaTime, transform);
        }

        override protected void OnCollisionEnter(Collision collision)
        {
			base.OnCollisionEnter (collision);
        }

        //override protected void OnCollisionStay(Collision collision)
        //{
        //    int a = 10;
        //}

        override protected void OnCollisionExit(Collision collision)
        {
			base.OnCollisionExit (collision);
            
        }
    }
}
