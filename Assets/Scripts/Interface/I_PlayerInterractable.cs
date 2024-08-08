using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_PlayerInterractable 
{
    public void OnEnterCursor();
    public void OnExitCursor();
    public void OnClicked();
}
