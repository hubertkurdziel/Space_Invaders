using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites;
    public float animationTime = 1.0f;
    public int score = 10;

    [Header("Power-Ups")]
    public GameObject lifePowerUpPrefab;
    [Range(0f, 1f)] public float lifeDropChance = 0.05f;

    public GameObject bunkerPowerUpPrefab;
    [Range(0f, 1f)] public float bunkerDropChance = 0.03f;

    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        _animationFrame++;

        if (_animationFrame >= this.animationSprites.Length)
        {
            _animationFrame = 0;
        }

        _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            GameManager.Instance.OnInvaderKilled(this);

            CheckForDrop();

            this.gameObject.SetActive(false);
        }
    }

    private void CheckForDrop()
    {
        float roll = UnityEngine.Random.value;

        if (bunkerPowerUpPrefab != null && roll <= bunkerDropChance)
        {
            Instantiate(bunkerPowerUpPrefab, this.transform.position, Quaternion.identity);
        }

        else if (lifePowerUpPrefab != null && roll <= (bunkerDropChance + lifeDropChance))
        {
            Instantiate(lifePowerUpPrefab, this.transform.position, Quaternion.identity);
        }
    }
}