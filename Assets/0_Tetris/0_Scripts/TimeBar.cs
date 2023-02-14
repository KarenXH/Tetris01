using UnityEngine;

public class TimeBar : MonoBehaviour
{
    public GameObject bar;
    static TimeBar _instance;
    public static TimeBar Instance { get => _instance;}

    private void Awake()
    {
        TimeBar._instance = this;
    }
    public void AnimaBar()
    {
        LeanTween.scaleY(bar, 0, Piece.Instance.stepDelay);  
    }
    public void ResetAnimaBar()
    {
        LeanTween.pauseAll();
        bar.LeanScaleY(1, 0);
    }
}
