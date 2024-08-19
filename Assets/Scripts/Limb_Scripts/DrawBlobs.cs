using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DrawBlobs : MonoBehaviour
{
    Player player;
    GameObject blob;

    private List<Transform> InstantiatedBlobs = new List<Transform>();

    public List<Limb> limbs = new List<Limb>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DrawBlobsAroundLimb(Limb limb)
    {
        /*Joint2D joint = limb.GetComponent<Joint2D>();

        while (joint != null)
        {
            GameObject newblob = Instantiate(blob, limb.transform.position, Quaternion.identity);
            InstantiatedBlobs.Add(newblob.transform);

            Rigidbody2D attachedBody = joint.attachedRigidbody;
            if (attachedBody == null)
            {
                break;
            }
            

            joint = joint.attachedRigidbody.GetComponent<Joint2D>();
        }*/
    }
}
