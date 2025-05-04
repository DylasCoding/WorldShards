using UnityEngine;

public class DamageSpawner : MonoBehaviour
{
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private GameObject criticalDamagePopupPrefab; // Popup cho damage crit
    [SerializeField] private GameObject healPopupPrefab; // Popup cho heal

    [SerializeField] private float heightOffset = 1f; // Độ cao trên đầu character

    public void SpawnDamagePopup(Vector2 position, int damage)
    {
        // Tính vị trí spawn trên đầu character
        Vector3 spawnPosition = new Vector3(position.x, position.y + heightOffset, 0f);

        // Tạo popup tại vị trí thế giới
        GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);

        // Gán giá trị damage
        PopUpDamage popupScript = popup.GetComponent<PopUpDamage>();
        popupScript.SetDamage(damage);
    }
}