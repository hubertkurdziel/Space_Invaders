using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public enum PowerUpType
    {
        AddLife,
        ResetBunkers
    }



    public PowerUpType type;
    public float fallSpeed = 2.0f;

    void Update()
    {

        this.transform.position += Vector3.down * fallSpeed * Time.deltaTime;


        Vector3 bottomEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        if (this.transform.position.y < bottomEdge.y - 1.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            if (type == PowerUpType.AddLife)
            {
                GameManager.Instance.AddLife();
            }
            else if (type == PowerUpType.ResetBunkers)
            {
                GameManager.Instance.ResetAllBunkers();
            }


            Destroy(this.gameObject);
        }
    }
}