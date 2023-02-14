using UnityEngine;

public class MyFingerDown : MonoBehaviour
{
    public void RotateTetris()
    {
        Piece.Instance.CheckClicked();
    }
    public void MoveLeftTetris()
    {
        Piece.Instance.CheckMovedLeft();
    }
    public void MoveRightTetris()
    {
        Piece.Instance.CheckMovedRight();
    }
    public void DropTetris()
    {
        Piece.Instance.CheckMovedDrop();
    }



}
