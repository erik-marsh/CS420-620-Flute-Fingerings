using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Record
    {
        internal Vector3 prePositionRecord;
        Queue<Hi5_Position_Record> mQueuePositionRecord = new Queue<Hi5_Position_Record>();
        internal void RecordPosition(float deltaTime,Transform transform)
        {
            if (mQueuePositionRecord.Count > (Hi5_Interaction_Const.ObjectPinchRecordPositionCount - 1))
            {
                mQueuePositionRecord.Dequeue();
            }
            Hi5_Position_Record record = new Hi5_Position_Record(transform.position, prePositionRecord, deltaTime);
            mQueuePositionRecord.Enqueue(record);
            prePositionRecord = transform.position;
        }

        internal void RecordClean()
        {
            mQueuePositionRecord.Clear();
        }

        internal Queue<Hi5_Position_Record> GetRecord()
        {
            return mQueuePositionRecord;
        }

       
    }

    interface Hi5_Record_Interface
    {
        Queue<Hi5_Position_Record> GetRecord();
    }



}
