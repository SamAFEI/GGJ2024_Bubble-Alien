using UnityEngine;

public class PlatformEffector : MonoBehaviour
{
    public int Player1Layer = 7;
    public int Player2Layer = 8;
    public PlatformEffector2D effector { get; private set; }
    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }
    private void Update()
    {
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            effector.colliderMask &= ~(1 << Player1Layer);
        }
        else
        {
            effector.colliderMask |= (1 << Player1Layer);
        }

        if (Input.GetAxisRaw("Vertical_2") < 0)
        {
            effector.colliderMask &= ~(1 << Player2Layer);
        }
        else
        {
            effector.colliderMask |= (1 << Player2Layer);
        }
    }
}
