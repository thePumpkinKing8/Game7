using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryExit : Interactable
{
    public override void Interact()
    {
        GameManager.Instance.NextScene();
    }
}
