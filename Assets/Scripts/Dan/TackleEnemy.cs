using UnityEngine;

public class TackleEnemy : MonoBehaviour
{
    public float speed = 5.0f;
    public float detectionRadius = 10.0f;
    public float cooldownTime = 3.0f;
    public LayerMask playerLayer;
    public float impactForce = 500.0f;
    private RaycastHit hit;
    private Transform player;
    private bool coolingDown;
    private float cooldownTimer;

    //private PlayerMechanicResponse playerMovement; // Referencia al script PlayerMovement

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        coolingDown = false;
        cooldownTimer = 0.0f;
        //playerMovement = player.GetComponent<PlayerMechanicResponse>();
    }

    void Update()
    {
        if (coolingDown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0.0f)
            {
                coolingDown = false;
            }
        }
        else
        {
            //if (Physics.SphereCast(transform.position, detectionRadius, Vector3.zero, out hit))
            //{
            //    GameObject hit = 
            //    Vector3 direction = (player.position - transform.position).normalized;
            //    transform.position += direction * speed * Time.deltaTime;
            //}
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !coolingDown)
        {
            // Empuja al jugador
            //playerMovement.AddImpact(new Vector2(impactForce, impactForce));

            // Enfriamiento y el temporizador de enfriamiento
            coolingDown = true;
            cooldownTimer = cooldownTime;
        }
    }
}