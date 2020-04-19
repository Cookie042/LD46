using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectShooter : MonoBehaviour
{

    public List<Rigidbody> spawnablePrefabs;
    public Vector3 spawnPointOffset;
    public float spawnSpeed;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        var mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            var rb = Instantiate(
                spawnablePrefabs[Random.Range(0, spawnablePrefabs.Capacity)], 
                transform.position + transform.TransformDirection(spawnPointOffset),
                Quaternion.Euler(Random.insideUnitSphere * 90));

            var childRBs = rb.GetComponentsInChildren<Rigidbody>();

            for (var i = 0; i < childRBs.Length; i++)
            {
                var childRb = childRBs[i];
                childRb.velocity = mouseRay.direction * spawnSpeed;
            }
        }
    }
}
