아직 문서를 작성하지 못해서 간단한 설명입니다. 읽어주시면 감사하겠습니다.
게임 플레이 영상 : https://www.youtube.com/watch?v=ieC0TfMf44E&feature=youtu.be
1. Player
 - PlayerController : Update에서 사용자로부터 키입력을 받아 이동,공격,회피(구르기) 실행. 피격,사망 이벤트 포함
 - PlayerGauge : UI의 체력, 마력바를 갱신하는 함수. PlayerStat 스크립트의 static 이벤트에 연결되어 실행됨.
 - PlayerDamaged : 쓰이지않는 스크립트

2. Enemy
 - EnemyAI : 코루틴함수를 통해 적캐릭터의 현재 상태를 갱신, 상태에 따라 수행. 다른스크립트에 명령을 내림. 순찰,추적,공격,사망 등
 - EnemyMove : Scene의 순찰포인트 지점을 랜덤으로 순환(내비게이션 컴포넌트 이용). 주변에 플레이어가 오면 추적.
 - EnemyAttack : 플레이어가 공격범위내에 있으면 공격. 공격시점에 플레이어가 사정거리(거리,각도)내에 있으면 데미지를 줌. 
                      플레이어의 Info 스크립트를 이용해 체력을 계산된 값(공격력, 방어력에 따른 데미지)만큼 감소시킴.
 - EnemyDamaged : 적 캐릭터가 공격받을 때 체력 갱신, 머리위에 체력바 표시(EnemyHpBar 스크립트 이용).
 - EnemyDie : 체력이 0이하로 떨어지면 Die 애니메이션 실행. 코루틴으로 몇초뒤에 비활성화.
 - EnemyHpBar : 적 캐릭터의 머리위에 표시되는 체력바에 대한 스크립트. 오브젝트 풀링을 이용.

3. Manager
 - GameManger : 게임 실행, 재시작, 종료 등에 관한 스크립트. 싱글턴
 - PoolManager : EnemyHpBar를 활용한 ObjectPooling에 대한 스크립트
 - CoroutineManager : Stat 스크립트에서 코루틴을 실행하기 위한 스크립트

4. Stat
 - CommonStat : 최상위 부모클래스. 체력,방어력 등 공통적인 요소들로 구성
 - EnemyStat : 아직 별다른 기능을 추가하지않아 상속만 받은상태.
 - PlayerStat : 이벤트 추가. 프로퍼티로 체력,마력이 변경될때마다 PlayerGauge에서 이벤트에 연결한 함수가 실행됨.
 - Info : 각 게임오브젝트마다 Stat을 저장하기위한 스크립트.
