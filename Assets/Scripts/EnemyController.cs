using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;

public class EnemyController : MonoBehaviour
{
    [SerializeField]private PlayerStats stats;
    public float lookRadius = 50f;
    private Transform target;
    private HP playerHP;
    NavMeshAgent agent;
    public float lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = stats.speed;
        target = PlayerManager.Instance.player.transform;
        playerHP = target.GetComponent<HP>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                if (Time.time >= lastAttackTime + stats.attackSpeed)
                {
                    playerHP?.TakeDamage(stats.strength);
                    Debug.Log(playerHP.currentHealth);
                    lastAttackTime = Time.time;
                    lookRadius = 150f;
                }
                FaceTarget();
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void FaceTarget()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5);
    }
}
