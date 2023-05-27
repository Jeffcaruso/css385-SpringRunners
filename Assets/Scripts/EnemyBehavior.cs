using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    //public GameObject waypoint1;
    //public GameObject waypoint2;
    private GameObject target;
    //public GameObject healthbar;
    public GameObject bulletPrefab;
    public float speed = 10.0f;
    public float shootingForce = 10.0f;
    public float bulletArcHeight = 5.0f;
    public float minShootingInterval = 0.5f;
    public float maxShootingInterval = 2.0f;
    //public int maxHealth = 100;
    //public int currentHealth;

    //private bool drifting = false;
    //private Vector3 driftCenter;
    private Transform parentTransform;
    private SpriteRenderer objectSpriteRenderer;
    private Color originalColor;

    private Camera mainCamera;
    private bool shouldShoot = false;
    private Coroutine shootingCoroutine;

    void Start()
    {
        target = GameObject.Find("Hero square");
        parentTransform = transform.parent;
        objectSpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = objectSpriteRenderer.color;
        //currentHealth = maxHealth;
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if the enemy should start shooting based on its position relative to the camera
        if (!shouldShoot && IsEnemyVisibleOnScreen())
        {
            shouldShoot = true;
            shootingCoroutine = StartCoroutine(ShootAtRandomInterval());
            Debug.Log("Start shooting");
        }
        // Check if the enemy should stop shooting based on its position relative to the camera
        else if (shouldShoot && !IsEnemyVisibleOnScreen())
        {
            shouldShoot = false;
            StopCoroutine(shootingCoroutine);
            Debug.Log("Stop shooting");
        }
    }


    private IEnumerator ShootAtRandomInterval()
    {
        while (true)
        {
            Debug.Log("Coroutine is running.");
            yield return new WaitForSeconds(Random.Range(minShootingInterval, maxShootingInterval));
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        StartCoroutine(MoveBulletInArc(bullet));
    }

    private IEnumerator MoveBulletInArc(GameObject bullet)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = target.transform.position;
        Vector3 controlPoint = startPosition + (endPosition - startPosition) / 2 + Vector3.up * bulletArcHeight;

        float t = 0;
        float duration = Vector3.Distance(startPosition, endPosition) / shootingForce;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            bullet.transform.position = CalculateBezierPoint(t, startPosition, controlPoint, endPosition);
            yield return null;
        }

        Destroy(bullet);
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 point = uu * p0 + 2 * u * t * p1 + tt * p2;

        return point;
    }

    private bool IsEnemyVisibleOnScreen()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1;
    }

    // private IEnumerator MoveToWaypoint()
    // {
    //     float distanceToWaypoint2 = Vector2.Distance(transform.position, waypoint2.transform.position);
    //     float currentSpeed = speed;
    //     float easingDistance = 0.5f;

    //     transform.position = waypoint1.transform.position;

    //     while (distanceToWaypoint2 > 0f)
    //     {
    //         distanceToWaypoint2 = Vector2.Distance(transform.position, waypoint2.transform.position);

    //         if (distanceToWaypoint2 <= easingDistance)
    //         {
    //             float t = distanceToWaypoint2 / easingDistance;
    //             currentSpeed = speed * Mathf.Lerp(0.1f, 1f, t * t);
    //         }

    //         transform.position = Vector3.MoveTowards(transform.position, waypoint2.transform.position, currentSpeed * Time.deltaTime);
    //         yield return null;
    //     }

    //     drifting = true;
    //     driftCenter = waypoint2.transform.position;
    //     StartCoroutine(ShootAtRandomInterval());
    // }

    // private void Drift()
    // {
    //     float xDrift = 0.8f * Mathf.Sin(0.7f * Time.time);
    //     float yDrift = 0.25f * Mathf.Cos(0.7f * Time.time);

    //     Vector3 relativeDrift = new Vector3(xDrift, yDrift, 0);
    //     driftCenter = waypoint2.transform.position;
    //     transform.position = driftCenter + relativeDrift;
    // }

}