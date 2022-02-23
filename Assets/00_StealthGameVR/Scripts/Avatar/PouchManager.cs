using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PouchManager : MonoBehaviour
{
    public Material hoverMaterial;
    public int maxStoneCount;
    public int stoneCount;
    public GameObject stoneSocket;
    private Material standardMaterial;

    void Start()
    {
        // instantiate max of stone sockets and disable each collider
        for(int i = 0; i < maxStoneCount; i++)
        {
            Instantiate(stoneSocket, this.transform);
            this.transform.GetChild(i).GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // when trigger is hand
        if(other.gameObject.CompareTag("Hand"))
        {
            // hand has stone -> wants to put stone in
            if(other.gameObject.GetComponent<XRDirectInteractor>().selectTarget != null && other.gameObject.GetComponent<XRDirectInteractor>().selectTarget.CompareTag("Stone"))
            {
                // look through each socket
                foreach(Transform socket in this.transform)
                {
                    // if empty socket available
                    if(socket.GetComponent<StoneSocket>().selectTarget == null)
                    {
                        // enable collider to invoke available select enter
                        socket.GetComponent<CapsuleCollider>().enabled = true;
                        break;
                    }
                    // if there is no empty available do nothing
                }
            }
            // hand has no stone -> wants to take stone out
            else if(other.gameObject.GetComponent<XRDirectInteractor>().selectTarget == null)
            {
                // look through each socket
                foreach(Transform socket in this.transform)
                {
                    // if filled available
                    if(socket.GetComponent<StoneSocket>().selectTarget != null)
                    {
                        // enable collider to invoke available select exit
                        socket.GetComponent<CapsuleCollider>().enabled = true;
                        // enable stone mesh and collider to make it visible and grabbable
                        socket.GetComponent<StoneSocket>().attachTransform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                        socket.GetComponent<StoneSocket>().attachTransform.GetChild(0).GetComponent<MeshCollider>().enabled = true;
                        break;
                    }
                    // if there is no stone available do nothing
                }
            }
        } 
        
        // Debug.Log("Trigger enter.");     
    }

    void OnTriggerExit(Collider other) 
    {
        int availableStones = 0;

        foreach(Transform socket in this.transform)
        {
            // disable all socket colliders and meshes
            socket.GetComponent<CapsuleCollider>().enabled = false;
            if(socket.GetComponent<StoneSocket>().selectTarget != null)
            {
                socket.GetComponent<StoneSocket>().attachTransform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                socket.GetComponent<StoneSocket>().attachTransform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
                // count all filled sockets
                availableStones++;
            }
        }

        this.stoneCount = availableStones;

        // Debug.Log("Trigger exit.");
    }

    public Material GetStandardMaterial()
    {
        return this.standardMaterial;
    }

    public void SetStandardMaterial(Material _mat)
    {
        this.standardMaterial = _mat;
    }
}
