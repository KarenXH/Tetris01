using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFingerDown : MonoBehaviour
{
    public void RotateTetris()
    {
        Piece.Instance.CheckClicked();
    }
}
