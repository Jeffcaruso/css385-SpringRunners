using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public GameObject waypoint1;
    public GameObject waypoint2;
    public GameObject target;
    //public GameObject healthbar;
    public GameObject bulletPrefab;
    public float speed = 10.0f;
    public float shootingForce = 10.0f;
    public float bulletArcHeight = 5.0f;
    public float minShootingInterval = 0.5f;
    public float maxShootingInterval = 2.0f;
    //public int maxHealth = 100;
    //public int currentHealth;

    private bool drifting = false;
    private Vector3 driftCenter;
    private Transform parentTransform;
    private SpriteRenderer objectSpriteRenderer;
    private Color originalColor;

    void Start()
    {
        parentTransform = transform.parent;
        objectSpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = objectSpriteRenderer.color;
        //currentHealth = maxHealth;
        StartCoroutine(MoveToWaypoint());
    }

    void Update()
    {
        if (drifting)
        {
            Drift();
        }
    }

    private IEnumerator MoveToWaypoint()
    {
        float distanceToWaypoint2 = Vector2.Distance(transform.position, waypoint2.transform.position);
        float currentSpeed = speed;
        float easingDistance = 0.5f;

        transform.position = waypoint1.transform.position;

        while (distanceToWaypoint2 > 0f)
        {
            distanceToWaypoint2 = Vector2.Distance(transform.position, waypoint2.transform.position);

            if (distanceToWaypoint2 <= easingDistance)
            {
                float t = distanceToWaypoint2 / easingDistance;
                currentSpeed = speed * Mathf.Lerp(0.1f, 1f, t * t);
            }

            transform.position = Vector3.MoveTowards(transform.position, waypoint2.transform.position, currentSpeed * Time.deltaTime);
            yield return null;
        }

        drifting = true;
        driftCenter = waypoint2.transform.position;
        StartCoroutine(ShootAtRandomInterval());
    }

    private void Drift()
    {
        float xDrift = 0.8f * Mathf.Sin(0.7f * Time.time);
        float yDrift = 0.25f * Mathf.Cos(0.7f * Time.time);

        Vector3 relativeDrift = new Vector3(xDrift, yDrift, 0);
        driftCenter = waypoint2.transform.position;
        transform.position = driftCenter + relativeDrift;
    }

    private IEnumerator ShootAtRandomInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minShootingInterval, maxShootingInterval));
            int attackNum = Random.Range(1, 4);
            Debug.Log(attackNum);
            Attack(attackNum);
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        StartCoroutine(MoveBulletInArc(bullet));
    }

    private void Attack(int num){
        if (num == 1){
            GameObject bullet1 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            GameObject bullet2 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            GameObject bullet3 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            GameObject bullet4 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            GameObject bullet5 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector3 bullet1Target = new Vector3(target.transform.position.x, target.transform.position.y - 200, target.transform.position.z);
            Vector3 bullet2Target = new Vector3(target.transform.position.x, target.transform.position.y - 100, target.transform.position.z);
            Vector3 bullet4Target = new Vector3(target.transform.position.x, target.transform.position.y + 100, target.transform.position.z);
            Vector3 bullet5Target = new Vector3(target.transform.position.x, target.transform.position.y + 200, target.transform.position.z);
            StartCoroutine(MoveBulletInArc(bullet1, bullet1Target));
            StartCoroutine(MoveBulletInArc(bullet2, bullet2Target));
            StartCoroutine(MoveBulletInArc(bullet3));
            StartCoroutine(MoveBulletInArc(bullet4, bullet4Target));
            StartCoroutine(MoveBulletInArc(bullet5, bullet5Target));
        } else if (num == 2){
            GameObject bullet1 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            GameObject bullet2 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            GameObject bullet3 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            GameObject bullet4 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            GameObject bullet5 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector3 bullet1Start = new Vector3(transform.position.x, transform.position.y - 150, transform.position.z);
            Vector3 bullet2Start = new Vector3(transform.position.x, transform.position.y - 75, transform.position.z);
            Vector3 bullet4Start = new Vector3(transform.position.x, transform.position.y + 75, transform.position.z);
            Vector3 bullet5Start = new Vector3(transform.position.x, transform.position.y + 150, transform.position.z);

            Vector3 bullet1Target = new Vector3(target.transform.position.x, target.transform.position.y - 150, target.transform.position.z);
            Vector3 bullet2Target = new Vector3(target.transform.position.x, target.transform.position.y - 75, target.transform.position.z);
            Vector3 bullet4Target = new Vector3(target.transform.position.x, target.transform.position.y + 75, target.transform.position.z);
            Vector3 bullet5Target = new Vector3(target.transform.position.x, target.transform.position.y + 150, target.transform.position.z);
            StartCoroutine(MoveBulletInArc(bullet1, bullet1Start, bullet1Target));
            StartCoroutine(MoveBulletInArc(bullet2, bullet2Start, bullet2Target));
            StartCoroutine(MoveBulletInArc(bullet3));
            StartCoroutine(MoveBulletInArc(bullet4, bullet4Start, bullet4Target));
            StartCoroutine(MoveBulletInArc(bullet5, bullet5Start, bullet5Target));
        } else {
            StartCoroutine(BurstAttack());
        }
    }

    private IEnumerator BurstAttack(){
        int count = 0;
        int offset = 200;
        while (count < 4){
            yield return new WaitForSeconds(.15f);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector3 bulletTarget = new Vector3(target.transform.position.x, target.transform.position.y + offset, target.transform.position.z);
            StartCoroutine(MoveBulletInArc(bullet, bulletTarget));
            count++;
            offset -= 100;
        }
        offset = 0;
        while (count < 8){
            yield return new WaitForSeconds(.15f);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector3 bulletTarget = new Vector3(target.transform.position.x, target.transform.position.y + offset, target.transform.position.z);
            StartCoroutine(MoveBulletInArc(bullet, bulletTarget));
            count++;
            offset += 100;
        }
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

    private IEnumerator MoveBulletInArc(GameObject bullet, Vector3 newEndPos)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = newEndPos;
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

    private IEnumerator MoveBulletInArc(GameObject bullet, Vector3 newStartPos, Vector3 newEndPos)
    {
        Vector3 startPosition = newStartPos;
        Vector3 endPosition = newEndPos;
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

    // public void isHit(int damage)
    // {
    //     //currentHealth -= damage;
    //     currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    //     healthbar.transform.localScale = new Vector3((float)currentHealth / maxHealth, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
    //     StartCoroutine(FlashObject());
    // }

    // private IEnumerator FlashObject()
    // {
    //     objectSpriteRenderer.color = Color.white;
    //     yield return new WaitForSeconds(0.2f);
    //     objectSpriteRenderer.color = originalColor;
    // }
}