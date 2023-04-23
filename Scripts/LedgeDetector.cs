using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    [SerializeField] private float ledgeCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    private bool canDetectLedge = true;
    public bool isLedgeDetected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("canDetectLedge: " + canDetectLedge);
        if (canDetectLedge)
        {
            isLedgeDetected = Physics2D.OverlapCircle(transform.position, ledgeCheckRadius, whatIsGround);
        }
        Debug.Log("isLedgeDetected: " + isLedgeDetected);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetectLedge = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetectLedge = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, ledgeCheckRadius);
    }
}
