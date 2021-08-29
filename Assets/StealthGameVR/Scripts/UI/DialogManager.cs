using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public List<Dialog> dialogs;

    public void ResetDialogs()
    {
        foreach(Dialog dialog in this.dialogs)
        {
            dialog.ResetPages();
        }
    }

    public void ResetDialog(Dialog dialog)
    {
        dialog.ResetPages();
    }

    public void DestroyDialog(Dialog dialog)
    {
        dialog.Destroy();
    }
}
