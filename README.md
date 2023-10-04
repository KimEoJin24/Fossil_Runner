# Fossil Runner

![fossilrunner](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/5bb8d23d-8899-4735-9c3e-aa71462a24b9)


## 목차
   1. 게임 설명 및 개발 기간
   2. 개발 환경
   3. 구현 목록 및 담당자
   4. 플레이 영상
   5. 게임 방법
   6. 클래스 설명
   7. 오류
  
## 게임 설명 및 개발 기간
- 게임명 : `Fossil Runner`
- 설명 : 게임개발종합반 B04 오늘 점심은 4000탕짬면! 게임개발의 숙련 프로젝트
- 장르 : 서바이벌 게임
- 스토리 : 현실세계에서 화산의 대대대대대대ㅐ댇대ㅐ대대대대대대ㅐ폭발로 시공간이 뒤틀려서 시대가 섞인 섬에 들어왔다. 섬에서 생활을 하다보니 화산 폭발의 원인이 불의 용인 사실을 알게 되었고, 원래 세계로 돌아가기 위해서는 잠자는 하늘섬에 있는 용을 잡아야한다고 전해진다. 하지만 지금 가지고 있는 것은 아무것도 없는데 어떡해야햐지? 섬에 있는 다양한 자원을 통해 힘을 길러 용을 처지하고 현실세계로 돌아가자!
- 조작법 :

          상하좌우 WASD, 점프 SPACEBAR, 달리기 SHIFT, 상호작용 E, 인벤토리 TAP, 공격 MOUSE
- 개발 기간 : 23.09.25 ~ 23.10.04

## 개발 환경
![Unity](https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white)

![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)

![GitHub](https://img.shields.io/badge/github-%23121011.svg?style=for-the-badge&logo=github&logoColor=white)

## 구현 목록 및 담당자
- 필수 구현 기능
  1. 자원 수집 및 가공
  2. 식사와 수분 관리
  3. 건축 및 생존 기지 구축
  4. 적과의 전투
  5. 생존 관리 시스템
  6. 자원 리스폰

- 선택 구현 기능
  1. 다양한 적 종류
  2. 고급 AI 시스템
  3. 사운드 및 음악

- 담당자

  ![역할](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/39781db2-2a9e-4745-9062-a8256be83f83)

## 플레이 영상

https://www.youtube.com/watch?v=Le8jc3p3Z68

## 게임 방법
![Main](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/90b17704-8b0f-4b02-8ed2-145e74400b28)

![ConditionUI](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/2726a45e-f170-4a0e-823f-b92774aee749)

![Dead](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/23e0b7e1-c6b1-47af-913a-5ad916ef2013)

![KeyGuide](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/c163a791-4f75-4658-ba2e-6dc75310a399)

![Attack&Interaction](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/b45ff12f-7def-4fa3-9f5d-3ce8665c4fca)

![Inventory](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/11aa010c-a492-4515-b770-e50c39de4c9b)

![Items](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/796e1bfa-46fd-4c40-8baa-c3c09b0f41b2)

![Environment](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/b041eefd-fc54-49d2-b5e5-b7cd7d2ffc26)

![Environment2](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/f1681ac0-fcc8-44f0-a801-8962660fc0c0)

![Environment3](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/c31257a2-9eb1-4e48-bd28-5af899b468a5)

![Environment4](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/dd63160f-327d-4b52-ae08-ba4b10a4cd08)

![Environment5](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/986bf01c-417a-480c-ae73-068a41e1a6d0)

![Boss](https://github.com/KimEoJin24/Fossil_Runner/assets/142365240/ee13e007-8981-436b-85d9-32c8468c22a7)

## 클래스 설명
팀 노션 Framework를 참고해주세요.

https://teamsparta.notion.site/04-4000-5c302fed25bc4e459f70ba20f25227e1

## 오류
| 오류 | 오류 내용 | 담당자 | 진행도 | 해결방법 |
| ---- | --------- | --- | --- | ------ |
| 콜라인더인 관계 | 자식의 콜라인터를 부모의 스크립트에서 사용하고 싶었음 | 정재훈 | 완료 | 부모한테 리지드바디를 주고 자식한테는 없다면 하나로 인식하여 자식의 콜라인더를 사용할 수 있다. |
| 애니메이션 | 애니메이션을 애니스테이로 하여 하나만 작동하게끔 했는데 동시에 작동하는 경우가 발생 | 정재훈 | 완료 | 애니 스테이끼리는 서로 조건이 충족될 때마다 바로바로 실행 한다는 사실을 알았고 시간차를 두면서 다른 애니메이션이 실행될 때는 못하게 해야한다는 사실을 알았다 |
| 중력 미작용 | 아이템을 위로 올리는 힘을 가했는데 작동되지 않음 | 정재훈 | 완료 |
| 패키지 추출 | 다른 유니티에서 프리팹으로 추출해서 옮기려니 용량도 많이 잡아먹고 오류도 발생했다. | 정재훈 | 완료 |
| 인벤토리 슬롯 null 오류 | 인벤토리 실행시 null exception이 발생하는 오류 | 박지원 | 완료 | 비어있는 슬롯 확인 후 채워주니 오류가 해결되었다 |
| 변수 초기화 | 많은 수의 오브젝트를 한번에 넣는 게 힘들었다 | 정재훈 | 완료 | mySelf = this.gameObject; 이 하나의 코드로 모든일이 해결되었다.
| 인벤토리 오류 | 인벤토리에서 아이템 착용 및 해제가 안 되었다 | 김어진 | 완료 | 인벤토리 코드를 변경하고 equipped가 받아주는 값을 컴포넌트에 넣었다.
