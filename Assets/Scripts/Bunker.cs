using UnityEngine;

public class Bunker : MonoBehaviour
{
    // Define la salud máxima del búnker.
    [Tooltip("Cuántos golpes aguanta esta pieza antes de romperse.")]
    public int totalHealth = 3;

    private int _currentHealth; // Salud restante.

    private void Start()
    {
        // Inicializa la salud al máximo al inicio de la partida.
        ResetBunker();
    }


    // Se activa cuando otro objeto (Invader, Missile o Laser) golpea este búnker.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprueba si el objeto que colisionó está en una de las capas de ataque.
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") ||
            other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {

            _currentHealth--; // El búnker recibe daño.

            // Si la salud llega a cero, desactiva el objeto (se "destruye").
            if (_currentHealth <= 0)
            {
                this.gameObject.SetActive(false);
            }

            // Si el objeto que golpeó es un proyectil (misil o láser), lo destruye.
            if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
                other.gameObject.layer == LayerMask.NameToLayer("Laser"))
            {
                Destroy(other.gameObject);
            }
        }
    }


    // Función pública para restaurar el búnker (usada al inicio de la ronda o por Power-Up).
    public void ResetBunker()
    {
        // Activa el objeto si estaba inactivo.
        this.gameObject.SetActive(true);

        // Restaura la salud a su valor máximo.
        _currentHealth = totalHealth;
    }
}