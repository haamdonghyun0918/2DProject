using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // GameManager 역시 게임당 하나만 존재해야 하므로 싱글톤 패턴을 사용
    public static GameManager Instance { get; private set; }

    // 게임 상태를 만들어서 턴제 게임을 가능하게 만듦
    public enum GameState { PlayerTurn, EnemyTurn, GameOver }
    public GameState currentState;
    // 게임 캐릭터를 담을 그릇을 만듦(애니메이션을 표현하기 위해서)
    private GameCharacter playerCharacter;
    // 몬스터 리스트에서 활성화되는 몬스터들을 담을 리스트를 가져옴
    private List<GameMonster> activeMonsters = new List<GameMonster>();
    // 사용하려는 카드를 받는 변수
    private CardData selectedCard;
    // 현재 게임 스테이지를 받는 변수
    private GameStage currentStage;
    // 현재 턴을 계산하는 변수 추가
    public int currentTurn { get; private set; } = 1;

    private bool isBossPhase = false;
    private string currentBossId = "";
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartBattle(GameCharacter character, List<GameMonster> monsters, GameStage stage, string bossId)
    {
        playerCharacter = character;
        currentStage = stage;
        activeMonsters.Clear();
        activeMonsters.AddRange(monsters);

        currentBossId = bossId;
        isBossPhase = false;
        
        // 전투 시작 시 턴을 1로 초기화
        currentTurn = 1;
        
        if (currentStage != null)
        {
            currentStage.UpdateTurnText(currentTurn);
        }
        // 게임이 시작하면 무조건 플레이어 턴으로 시작
        currentState = GameState.PlayerTurn;
        Debug.Log("플레이어의 턴입니다!");
    }

    public bool UseCardOnMonster(CardData card, GameMonster targetMonster)
    {
        if (currentState != GameState.PlayerTurn) return false; // 카드를 내지 않으면 끝나지 플레이어의 턴이 끝나지 않음

        // 카드를 내면 상대편 턴으로 바꿉니다.
        currentState = GameState.EnemyTurn;

        StartCoroutine(ProcessPlayerAttackRoutine(card, targetMonster));
        return true;
    }

    private IEnumerator ProcessPlayerAttackRoutine(CardData card, GameMonster targetMonster)
    {
        playerCharacter.PlayAttackAnim();
        targetMonster.TakeDamage(card.Damage);
        Debug.Log($"{card.Name} 카드로 공격!! (데미지: {card.Damage})");
        // 공격 했을 때 피격받는 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(0.5f);

        if (card.Heal > 0)
        {
            playerCharacter.HealHp(card.Heal);
            Debug.Log($"흡혈로 인하여 체력이 {card.Heal}만큼 회복되었습니다");
        }

        if (card.Bleed > 0)
        {
            targetMonster.AddBleed(card.Bleed);
            Debug.Log($"{targetMonster.name}에게 출혈을 주었습니다. 매 턴마다 {targetMonster.GetCurrentBleed()}의 데미지를 줍니다");
        }

        if (targetMonster.IsDead())
        {
            if (isBossPhase)
            {
                targetMonster.PlayBossDieAnim();
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                // 몬스터가 사라지기 전에 애니메이션을 보고 사라지게 함
                yield return new WaitForSeconds(0.5f);
            }
            // 제거
            activeMonsters.Remove(targetMonster);
            Destroy(targetMonster.gameObject);
        }

        if (currentStage != null)
        {
            currentStage.RefillUsedCard();
        }

        if (activeMonsters.Count == 0)
        {
            if (!isBossPhase && !string.IsNullOrEmpty(currentBossId))
            {
                Debug.Log("보스 몬스터 등장!!");
                isBossPhase = true;

                GameMonster boss = currentStage.SpawnBossMonster(currentBossId);
                activeMonsters.Add(boss);

                yield return new WaitForSeconds(0.5f);
                currentState = GameState.PlayerTurn;
                yield break;
            }
            else
            {
                GameOver(true);
                yield break; // 전투 종료 시 에러 방지를 위해 코루틴을 완벽히 탈출합니다.
            }
        }

        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        currentState = GameState.EnemyTurn;
        Debug.Log("상대방 턴");
        // 몬스터 턴 도중 출혈 때문에 몬스터가 죽을 수 있으므로 .ToArray()를 붙여서 살아있는 몬스터 목록의 복사본을 임시 배열로 만들어 루프 돌리는 작업
        foreach (var monster in activeMonsters.ToArray())
        {
            if (monster == null) continue;

            monster.PlayAttackAnim();
            yield return new WaitForSeconds(0.5f);

            int damage = monster.GetAttackPower();
            playerCharacter.TakeDamage(damage);

            // 플레이어가 맞고 움찔하는 시간을 줍니다.
            yield return new WaitForSeconds(0.5f);

            if (playerCharacter.IsDead())
            {
                // 플레이어가 죽었을 때도 바로 팝업이 뜨지 않고 쓰러지는 걸 볼 시간을 줍니다.
                yield return new WaitForSeconds(0.6f);
                GameOver(false);
                yield break;
            }

            if (monster.GetCurrentBleed() > 0)
            {
                monster.ApplyBleedDamage();

                // 출혈 데미지를 받고 움찔하는 시간을 줍니다.
                yield return new WaitForSeconds(0.5f);

                if (monster.IsDead())
                {
                    if (isBossPhase)
                    {
                        monster.PlayBossDieAnim();
                        yield return new WaitForSeconds(1.2f);
                    }
                    else
                    {
                        //출혈로 죽었을 때도 완전히 모션이 끝날 때까지 기다립니다.
                        yield return new WaitForSeconds(0.6f);
                    }

                    activeMonsters.Remove(monster);
                    Destroy(monster.gameObject);

                    if (activeMonsters.Count == 0)
                    {
                        if (!isBossPhase && !string.IsNullOrEmpty(currentBossId))
                        {
                            Debug.Log("보스 몬스터 등장!!");
                            isBossPhase = true;

                            GameMonster boss = currentStage.SpawnBossMonster(currentBossId);
                            activeMonsters.Add(boss);

                            yield return new WaitForSeconds(0.3f);
                        }
                        else
                        {
                            GameOver(true);
                            yield break;
                        }
                    }
                }
            }
        }

        Debug.Log("상대 턴 종료! 플레이어 턴!!");
        currentTurn++;
        if (currentStage != null)
        {
            currentStage.UpdateTurnText(currentTurn);
        }
        currentState = GameState.PlayerTurn;
    }

    private void GameOver(bool isWin)
    {
        currentState = GameState.GameOver;
        // 스테이지 승리 시, 현재 스테이지에서 사용한 턴 수를 누적 턴수에 저장하여 StageManager의 AddStageTurns에 연결
        if (isWin && StageManager.Instance != null)
        {
            StageManager.Instance.AddStageTurns(currentTurn);
        }

        if (StageManager.Instance != null && playerCharacter != null)
        {
            StageManager.Instance.SaveBattleResult(isWin, playerCharacter.GetCurrentHp());
        }

        if (isWin)
        {
            if (StageManager.Instance != null && StageManager.Instance.currentStageNum == 6)
            {
                Debug.Log("최종 스테이지를 클리어하였습니다! FinalClearUi가 열립니다");
                UiManager.Instance.OpenFinalClearUi();
            }
            else
            {
                Debug.Log("스테이지 클리어! ClearPopUp 오픈");
                UiManager.Instance.OpenClearPopUp();
            }
        }
        else
        {
            Debug.Log("스테이지 실패... FailPopUp 오픈");
            UiManager.Instance.OpenFailPopUp();
        }
    }
}