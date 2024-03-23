using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Monster
{
    public override IEnumerator StateCoroutine()
    {
        while (!isDie)
        {
            if (monsterState == MonsterState.DIE)
                yield break;
            float zDistance = transform.position.z - Player.instance.transform.position.z;

            //������ And ���������̰�, �÷��̾� �νĹ��� ���̸� ����
            if (0 >= zDistance || (monsterState == MonsterState.RUN && Player.instance.RecognitionDistance < zDistance))
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
            else if (recognitionDistance > zDistance)
                monsterState = MonsterState.RUN;
            yield return null;
        }
    }

    public override IEnumerator ActionCoroutine()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.RUN:
                    transform.position += new Vector3(0f, 0f, speed * Time.deltaTime);
                    break;
            }
            yield return null;
        }
    }
}
