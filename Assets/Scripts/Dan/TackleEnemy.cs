using UnityEngine;

public class TackleEnemy : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float detectionDistance = 5f;
    public float cooldownTimer = 2f;

    public float impactForce = 10f;

    private PlayerMechanicResponse playerMovement; // Referencia al script PlayerMechanicResponse

    private GameObject player;

    [SerializeField]private EnemyState currentState;

    private RaycastHit hit;

    private enum EnemyState
    {
        Inactive,
        FollowPlayer,
        Cooldown
    }

    private void Start()
    {
        currentState = EnemyState.Inactive;

        player = GameObject.FindGameObjectWithTag("Player");

        playerMovement = player.GetComponent<PlayerMechanicResponse>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Inactive:

                // El enemigo no hace nada en este estado
                break;

            case EnemyState.FollowPlayer:

                if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, detectionDistance))
                {
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                        transform.position += transform.forward * movementSpeed * Time.deltaTime;
                    }
                }
                break;

            case EnemyState.Cooldown:

                // El enemigo no hace nada mientras se enfría

                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    currentState = EnemyState.Inactive;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador fue golpeado");

            playerMovement.AddImpact(new Vector2(impactForce, impactForce), impactForce);

            currentState = EnemyState.Cooldown;
        }
    }

    public void DetectPlayer()
    {
        //Pruebas de niveles de accesibilidad
        Debug.Log("El jugador ha sido detectado 1");

        if (currentState == EnemyState.Inactive && Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, detectionDistance))
        {
            //Pruebas de niveles de accesibilidad
            Debug.Log("El jugador ha sido detectado 2");

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                //Pruebas de niveles de accesibilidad
                Debug.Log("El jugador ha sido detectado 3");

                currentState = EnemyState.FollowPlayer;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }
}