using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private Transform orbit;
    
    // Start is called before the first frame update
    void Start()
    {
        orbit = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            orbit.transform.rotation = Quaternion.Euler(0,-12 * Time.deltaTime, 0) *  orbit.transform.rotation;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            orbit.transform.rotation = Quaternion.Euler(0,+12 * Time.deltaTime, 0) *  orbit.transform.rotation;
        }
    }
}
