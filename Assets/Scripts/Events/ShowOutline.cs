using UnityEngine;

public class ShowOutline : MonoBehaviour
{
    public GameObject outlinePrefab; // Prefab viền đã thiết kế sẵn
    private GameObject outlineInstance;

    private bool isShowing = false;

    void Update()
    {
        Vector2 point;

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            point = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                CheckAndShowOutline(point);
            }
            else
            {
                HideOutline();
            }
        }
        else
        {
            HideOutline();
        }
#else
        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CheckAndShowOutline(point);
#endif
    }

    void CheckAndShowOutline(Vector2 point)
    {
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!isShowing)
            {
                ShowOutlineInstance();
            }
        }
        else
        {
            HideOutline();
        }
    }

    void ShowOutlineInstance()
    {
        outlineInstance = Instantiate(outlinePrefab, transform.position, Quaternion.identity, transform);
        outlineInstance.transform.localPosition = Vector3.zero;

        // Đồng bộ SpriteRenderer nếu cần
        SpriteRenderer mainSR = GetComponent<SpriteRenderer>();
        SpriteRenderer outlineSR = outlineInstance.GetComponent<SpriteRenderer>();

        if (mainSR != null && outlineSR != null)
        {
            outlineSR.sprite = mainSR.sprite;
            outlineSR.flipX = mainSR.flipX;
            outlineSR.flipY = mainSR.flipY;
            outlineSR.sortingLayerID = mainSR.sortingLayerID;
            outlineSR.sortingOrder = mainSR.sortingOrder - 1; // nằm sau object chính
        }

        isShowing = true;
    }


    void HideOutline()
    {
        if (outlineInstance != null)
        {
            Destroy(outlineInstance);
            isShowing = false;
        }
    }
}
