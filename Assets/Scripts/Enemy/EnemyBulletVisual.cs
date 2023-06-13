using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyBulletVisual : MonoBehaviour
{
    [SerializeField] private MMF_Player destroyFeedback;
    private EnemyBullet enemyBullet;

    private void Start()
    {
        enemyBullet = GetComponent<EnemyBullet>();
        enemyBullet.OnBulletHitted += EnemyBullet_OnBulletHitted;
    }

    private void EnemyBullet_OnBulletHitted(object sender, System.EventArgs e)
    {
        destroyFeedback.PlayFeedbacks();
    }
}
