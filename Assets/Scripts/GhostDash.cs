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
    private Vector2 lastPosition;
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
        ghostLifetimeSeconds = ghostLifetime;
        lastPosition = transform.position;
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, lastPosition) > 0.01f)
        {
            if (ghostDelaySeconds <= 0)
            {
                createGhost(lastPosition);
                ghostDelaySeconds = ghostDelay;
                lastPosition = transform.position;
            }
            else
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
        }
    }

    void createGhost(Vector2 position)
    {
        GameObject currentGhost = Instantiate(ghostPrefab, position, transform.rotation);
        if (ghostParent != null)
        {
            currentGhost.transform.SetParent(ghostParent);

            SpriteRenderer ghostSprite = currentGhost.GetComponent<SpriteRenderer>();
            SpriteRenderer characterSprite = GetComponent<SpriteRenderer>();

            if (ghostSprite.sprite != null && characterSprite.sprite != null)
            {
                ghostSprite.sprite = characterSprite.sprite;
                ghostSprite.flipX = characterSprite.flipX;
                // Debug.Log("Ghost created");
            }

            Destroy(currentGhost, ghostLifetimeSeconds);
        }
    }
}
