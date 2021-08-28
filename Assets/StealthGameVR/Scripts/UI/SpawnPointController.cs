using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public Transform xrRig;
    public Transform avatar;
    public DialogManager dialogManager;

    #nullable enable
    public void GoTo(Transform spawnPoint)
    {
        // reset dialoges
        dialogManager.ResetDialogs();

        // let avatar and xr rig spawn to spawn point
        xrRig.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        avatar.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    public void Cancel(Dialog currentDialog)
    {
        dialogManager.ResetDialog(currentDialog);
    }
}
