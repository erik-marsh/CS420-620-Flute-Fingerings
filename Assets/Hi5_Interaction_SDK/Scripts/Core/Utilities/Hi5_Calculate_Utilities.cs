using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hi5_Interaction_Core
{
	public class Hi5_PhySics_Calculate_Utilities 
	{

		internal static Vector3 CalculateForce(float mass,Vector3 dVector1,float dTime1,Vector3 dVector2,float dTime2)
		{
			Vector3 acceleration = CalculateAccelerationBySpeed (dVector1,dTime1,dVector2,dTime2);
			//Debug.Log ("acceleration" + acceleration);
			Vector3 force = acceleration*mass ;
			return force;
		}

		internal static Vector3 CalculateAccelerationBySpeed(Vector3 dVector1,float dTime1,Vector3 dVector2,float dTime2)
		{

			Vector3 acceleration = (dVector2 - dVector1)/dTime2;
			//Debug.Log ("acceleration" + acceleration);
			return acceleration;
		}

		internal static Vector3 CalculateAccelerationByForce(float mass,Vector3 force)
		{
			return force/mass ;
		}
	}
}
