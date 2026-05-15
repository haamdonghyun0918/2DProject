using UnityEngine;
using System.Collections;
public class Monster : MonoBehaviour
{
    [SerializeField] private Animator monsterAnim;

    public void TakeDamage()
    {
        monsterAnim.SetBool("isDamaged", true);

        StartCoroutine(CoDestroyMonster());
    }
    IEnumerator CoDestroyMonster()
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }
}
