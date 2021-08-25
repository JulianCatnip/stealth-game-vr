using UnityEngine;

public class DialogController : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform xrRig;
    public Transform avatar;

    void Start()
    {
        ResetDialoge();
    }

    public void Proceed()
    {
        // reset dialoge
        ResetDialoge();

        // let avatar and xr rig spawn to spawn point
        xrRig.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        avatar.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    public void GoBack()
    {
        ResetDialoge();
    }

    public void Talk()
    {
        StartDialoge();
    }

    private void ResetDialoge()
    {
        // reset dialoge
        this.transform.Find("Dialog2").gameObject.SetActive(false);
        this.transform.Find("Dialog1").gameObject.SetActive(true);
    }

    private void StartDialoge()
    {
        // reset dialoge
        this.transform.Find("Dialog1").gameObject.SetActive(false);
        this.transform.Find("Dialog2").gameObject.SetActive(true);
    }
}
