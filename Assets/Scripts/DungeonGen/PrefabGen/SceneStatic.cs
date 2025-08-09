using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SceneStatic : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private BoxCollider2D itemCollider;
    [SerializeField]
    private GameObject specificDropItem;

    private int objectHealth;
    private bool nonDestructable;

    [SerializeField]
    private GameObject hitFeedback, destoyFeedback;

    private AudioClip getHitSFX;
    private AudioClip destroySFX;

    public GameObject lightToSpawn;

    [Header("Flash Settings")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.05f;
    [SerializeField] private int flashCount = 2;

    public UnityEvent OnGetHit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Initialize(SceneStaticData sceneStaticData)
    {
        //set sprite
        spriteRenderer.sprite = sceneStaticData.sprite;
        objectHealth = sceneStaticData.health;
        nonDestructable = sceneStaticData.nonDestructible;
        getHitSFX = sceneStaticData.getHitSFX;
        destroySFX = sceneStaticData.destroySFX;
        specificDropItem = sceneStaticData.specificDropItem;
        //set sprite offset
        Vector2 offset = new Vector2(0.5f * sceneStaticData.size.x, 0.5f * sceneStaticData.size.y);
        spriteRenderer.transform.localPosition = offset;
        itemCollider.size = sceneStaticData.size;
        itemCollider.offset = offset;

        if (sceneStaticData.lightToSpawn != null)
        {
            // Place the light where the visible sprite is (same world position as the sprite)
            Vector3 lightPos = transform.position + (Vector3)offset;
            GameObject light = Instantiate(sceneStaticData.lightToSpawn, lightPos, Quaternion.identity, transform);
        }
    }

    public void StaticTakeDamage()
    {
        if (objectHealth > 0)
        {
            SFXManager.Instance.PlaySFX(getHitSFX);
            objectHealth -= 1;
            StartCoroutine(FlashSprite());
        }
        else if (objectHealth <= 0)
        {
            SFXManager.Instance.PlaySFX(destroySFX);

            if (specificDropItem != null)
            {
                GameObject spawnedObject = Instantiate(specificDropItem.gameObject, transform.position, Quaternion.identity);
            }
            else
            {
                InventoryItem item = GameManager.Instance.gameObject.GetComponent<LootTable>().GetDungeonLootItem();

                Vector2 dropOffset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                Vector2 dropPosition = (Vector2)transform.position + dropOffset;

                Instantiate(item.itemPrefab, dropPosition, Quaternion.identity);
            }

            Destroy(this.gameObject);
        }
    }

    private IEnumerator FlashSprite()
    {
        if (spriteRenderer == null) yield break;

        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

}

