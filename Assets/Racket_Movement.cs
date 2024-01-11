using UnityEngine;

public class Racket_Movement : MonoBehaviour
{
    public float racket_speed_t = 8.6f;
    public int movement_dir = 1;
    public int movment_sync = 1;

    private Transform L_Racket_Tr;
    private Transform R_Racket_Tr;

    private void Start()
    {
        L_Racket_Tr = transform.GetChild(0);
        R_Racket_Tr = transform.GetChild(1);
    }

    private void Update()
    {
        UpdateLRacketPosition();
        UpdateRRacketPosition();
    }

    private void UpdateLRacketPosition()
    {
        float verticalInput = Input.GetAxis("Vertical") * movement_dir;
        float newYPosition =
            L_Racket_Tr.position.y + verticalInput * racket_speed_t * Time.deltaTime;
        L_Racket_Tr.position = new Vector3(L_Racket_Tr.position.x, newYPosition, 0);
    }

    private void UpdateRRacketPosition()
    {
        float syncFactor = movment_sync > 0 ? 1 : -1;
        R_Racket_Tr.position = new Vector3(
            R_Racket_Tr.position.x,
            L_Racket_Tr.position.y * syncFactor,
            0
        );
    }
}
