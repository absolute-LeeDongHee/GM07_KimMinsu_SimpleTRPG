# 간단한 TRPG 게임 구현
> **C# 콘솔창 TRPG 구현 <br/>** **개발기간: 26.05.13 ~ 진행중**

## 개발자 소개
<img width="172" height="172" alt="image" src="https://github.com/user-attachments/assets/cd6f66e6-10b8-4ad8-bd50-de77b7d3e485" />  

이름: 김민수

소개: 현재 오즈코딩스쿨 게임 개발 과정 7기 수료중


## 프로젝트 소개
해당 프로젝트는 지금까지 배웠던 C# 문법들을 활용하는 것을 목적으로 시작된 프로젝트입니다. 그중에서 클래스의 역할 분리, 상속, List와 Dictionary 같은 컬렉션 활용에 초점을 두어 개발을 진행하고 있습니다.

## 플레이 영상

<video src="https://github.com/user-attachments/assets/a8e939c7-bb28-4896-9a3f-d81a5ab296fb" controls width="700"></video>

## Requirement

### Envirionment
![Visual Studio](https://img.shields.io/badge/Visual%20Studio%202022-5C2D91?style=for-the-badge&logo=visualstudio&logoColor=white)

### Language
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)

### Framework
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

## 수정사항
26.05.19 - 스킬 구현

## 아키텍쳐
```bash
Program
└─ GameApp: 전반적인 게임 진행을 관리
   ├─ Data: 데이터 파일과 데이터 로드 관련 폴더
   │  ├─ DataLoader
   │  ├─ JobData
   │  ├─ MonsterData
   │  ├─ SkillData
   │  ├─ ExpTable
   │  ├─ Jobs.json
   │  ├─ Monsters.json
   │  └─ Skills.json
   │
   ├─ Screen: 게임을 구성하는 화면 관련 폴더
   │  ├─ MainMenuScreen
   │  ├─ NameInputScreen
   │  ├─ JobSelectScreen
   │  └─ BattleScreen
   │
   ├─ Battle: 전투 진행 로직
   │  ├─ StageManager
   │  ├─ BattleManager
   │  ├─ TurnManager
   │  └─ BuffController
   │
   ├─ Units: 유닛의 스탯, 행동
   │  ├─ Unit
   │  ├─ Player
   │  ├─ Monster
   │  └─ Stat
   │
   ├─ Actions: 유닛의 전투 행동 구현
   │  ├─ BattleAction
   │  ├─ BasicAttack
   │  ├─ SingleTargetAttack
   │  ├─ AllTargetAttack
   │  ├─ BuffSkillAction
   │  ├─ UseSkill
   │  └─ UseItem
   │
   ├─ Input: 사용자의 키입력을 담당
   │  └─ InputManager
   │
   ├─ Interfaces: 게임에 필요한 인터페이스의 집합
   │  ├─ IDamageable
   │  ├─ IHasId
   │  ├─ IRequiresTarget
   │  └─ IUsableSkill
   │
   └─ Utils: 게임 진행 유틸 관련
      ├─ BattleUtils
      └─ StageScaling
```
