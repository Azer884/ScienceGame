using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerStats stat;
    [SerializeField] private Movement movement;
    [SerializeField] private Transform PlayerCam;
    [SerializeField] private LayerMask PickupLayer;
    [HideInInspector] public ObjectGrabbable Object;
    private float xRot;
    private float zRot;
    public Transform armTarget;

    private readonly float rotationSpeed = 5f;
    private float targetXRot = 0f;
    private float targetZRot = 0f;
    public LayerMask SinkLayer;
    [SerializeField] private float pickUpDistance = 7f;
    [SerializeField] private LiquidPropertiesCollection liquids;
    private PlayerInput inputActions;

    private bool isItemActive = false; // Boolean to track the state
    private bool canPerformAction = true;
    [SerializeField]private LayerMask enemyLayer;
    [SerializeField]private Transform attackPoint;
    [SerializeField]private float attackRange;
    private HP enemyHP;

    private void Awake()
    {
        inputActions = new PlayerInput();
        stat =  movement.stats[movement.index];
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        if (!transform.GetChild(0).gameObject.activeSelf)
        {
            // Grab
            if (!Object && Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit raycastHit, pickUpDistance, PickupLayer) && !isItemActive)
            {
                if (inputActions.PlayerControls.Hold.ReadValue<float>() > 0)
                {
                    if (raycastHit.transform.TryGetComponent(out Object))
                    {
                        Object.Grab(transform);
                        isItemActive = true;
                    }
                }
            }
            // Drop
            else if (inputActions.PlayerControls.Hold.ReadValue<float>() <= 0)
            {
                if (Object != null)
                {
                    Object.Drop();
                    StartCoroutine(SetItemActiveAfterDelay(1f));
                }
                Object = null;
            }
            if (Object != null)
            {
                Object.transform.rotation = armTarget.rotation;
            }

            UseItem();

            // Opening/Closing the sink
            if (Object == null && Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit sink, pickUpDistance, SinkLayer))
            {
                if (sink.transform.name == "SinkTap")
                {
                    if (inputActions.PlayerControls.Interact.triggered)
                    {
                        if (sink.transform.TryGetComponent(out Sink sink1))
                        {
                            sink.transform.GetChild(0).gameObject.SetActive(true);
                            StartCoroutine(DisableGameObjectAfterDelay(sink.transform.GetChild(0).gameObject, 0.566f));
                            sink1.IsOpened = !sink1.IsOpened;
                        }
                    }
                }
            }

            // Rotate arm
            if (Object == null || (Object.gameObject.layer == LayerMask.NameToLayer("Default" /*change to sword*/)))
            {
                targetXRot = inputActions.PlayerControls.Interact.ReadValue<float>() > 0 ? 85f : 0f;
                xRot = Mathf.Lerp(xRot, targetXRot, Time.deltaTime * rotationSpeed);
                armTarget.localRotation = Quaternion.Euler(0f, 0f, xRot);
            }
        }
        else
        {
            // Attack rotation
            if (canPerformAction && inputActions.PlayerControls.Hold.triggered && targetZRot == 0f)
            {
                targetZRot = 45f;
                Attack();
                StartCoroutine(PerformAttack());
            }
            // Defense with cooldown
            else if (canPerformAction && inputActions.PlayerControls.Interact.triggered && targetXRot == 0f)
            {
                targetXRot = 85f;
                StartCoroutine(PerformDef());
            }
            zRot = Mathf.Lerp(zRot, targetZRot, Time.deltaTime * rotationSpeed * 5);
            xRot = Mathf.Lerp(xRot, targetXRot, Time.deltaTime * rotationSpeed * 5);
            armTarget.localRotation = Quaternion.Euler(zRot, 0f, xRot);

            if (movement.index == 9)
            {
                enemyHP.transform.localScale *= movement.stats[9].scale;
                movement.index = 0;
            }
            else if (movement.index == 13)
            {
                enemyHP.currentHealth -= enemyHP.currentHealth / 8;
                movement.index = 0;
            }
            else if (movement.index == 11 || movement.index == 16)
            {
                enemyHP.GetComponent<EnemyController>().lastAttackTime += 5f;
                enemyHP.GetComponent<EnemyController>().lookRadius = 5f;
                movement.index = 0;
            }
            else if (movement.index == 12)
            {
                float timer = 0f;
                timer += Time.deltaTime;
                if (timer >= 5)
                {
                    enemyLayer = LayerMask.NameToLayer("Default");
                    timer = 0f;
                    movement.index = 0;
                }
                enemyLayer = LayerMask.NameToLayer("Enemy");

            }
            else if (movement.index == 14)
            {
                enemyHP.currentHealth -= 20;
                movement.index = 0;
            }
            else if (movement.index == 6)
            {
                attackRange *= 2;
                movement.index = 0;
            }
        }

        
    }

    private IEnumerator PerformAttack()
    {
        canPerformAction = false;
        targetZRot = 45f;
        yield return new WaitForSeconds(stat.attackSpeed / 5);
        targetZRot = 0f;
        yield return new WaitForSeconds(stat.attackSpeed);
        canPerformAction = true;
    }

    private void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            enemyHP = enemy.GetComponent<HP>();
            enemyHP?.TakeDamage(stat.strength);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    private IEnumerator PerformDef()
    {
        canPerformAction = false;
        PlayerManager.Instance.player.GetComponent<HP>().isDef = true;
        targetXRot = 85f;
        yield return new WaitForSeconds(stat.defense / 2);
        targetXRot = 0f;
        PlayerManager.Instance.player.GetComponent<HP>().isDef = false;
        yield return new WaitForSeconds(stat.defense);
        canPerformAction = true;
    }

    private IEnumerator DisableGameObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    private void UseItem()
    {
        if (Object && Object.gameObject.layer == LayerMask.NameToLayer("Grabbable") && Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit raycastHit, pickUpDistance))
        {
            if (inputActions.PlayerControls.Interact.triggered)
            {
                if (raycastHit.transform.parent.GetComponentInChildren<Liquid>())
                {
                    Liquid container = raycastHit.transform.parent.GetComponentInChildren<Liquid>();

                    if (!container.combos.stringLists.Contains(Object.gameObject.tag))
                    {
                        container.combos.stringLists.Add(Object.gameObject.tag);
                    }

                    container.fillAmount += 0.1f;
                    container.StartCoroutine(container.UpdateLiquid(container, liquids.liquidPropertiesList[TagToNum(Object.gameObject)], 1));
                    Destroy(Object.gameObject);
                    Object = null;
                    StartCoroutine(SetItemActiveAfterDelay(1.0f)); // Start the coroutine
                }
            }
        }
    }

    private IEnumerator SetItemActiveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isItemActive = false;
    }

    private int TagToNum(GameObject Object)
    {
        return Object.tag switch
        {
            "Berry" => 1,
            "Flower" => 2,
            "Root" => 3,
            "Wood" => 4,
            "Obsidian" => 5,
            _ => 0,
        };
    }
}
