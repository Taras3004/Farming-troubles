using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyBulletVisual : MonoBehaviour
{
    [SerializeField] private MMF_Player destroyFeedback;
    private EnemyBullet enemyBullet;

    private void Start()
    {
        enemyBullet = GetComponent<EnemyBullet>();
        enemyBullet.OnBulletHit += EnemyBulletOnBulletHit;
    }

    private void EnemyBulletOnBulletHit(object sender, System.EventArgs e)
    {
        destroyFeedback.PlayFeedbacks();
    }
}
