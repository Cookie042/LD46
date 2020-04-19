using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class DrawNavPath : MonoBehaviour
{
   private NavMeshAgent _agent;
   private LineRenderer _lr;

   void Start()
   {
      _agent = GetComponent<NavMeshAgent>();
      _lr = GetComponent<LineRenderer>();
   }

   private Vector3[] pathPoints = new Vector3[100];

   void Update()
   {
      
      
      var cornerCount = _agent.path.GetCornersNonAlloc(pathPoints);

      if (cornerCount > 0)
      {
         _lr.positionCount = cornerCount;
         _lr.SetPositions(pathPoints);
      }
   }

   private void OnDrawGizmos()
   {
      if (Application.isPlaying)
      {
         Handles.Label(transform.position, $"{_agent.pathStatus}, {_agent.path.status}, {_agent.isPathStale}");
      }
   }
}