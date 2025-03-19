using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDash : MonoBehaviour
{
    public GameObject ghostPrefab;
    public Transform ghostParent;
    public float ghostDelay;
    public float ghostLifetime;
    private float ghostDelaySeconds;
    private float ghostLifetimeSeconds;
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
        ghostLifetimeSeconds = ghostLifetime;
    }
    void Update()
    {
        if (ghostDelaySeconds > 0)
        {
            ghostDelaySeconds -= Time.deltaTime;
        }
        else
        {

            Vector3 ghostPosition = new Vector3(transform.position.x - 0.1f, transform.position.y, 0f);

            GameObject currentGhost = Instantiate(ghostPrefab, ghostPosition, transform.rotation);

            SpriteRenderer ghostSprite = currentGhost.GetComponent<SpriteRenderer>();
            ghostSprite.sprite = GetComponent<SpriteRenderer>().sprite;
            ghostDelaySeconds = ghostDelay;
            Destroy(currentGhost, ghostLifetimeSeconds);
        }
    }
}
