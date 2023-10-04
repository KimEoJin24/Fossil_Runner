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
- 조작법 :

          상하좌우 WASD, 점프 SPACEBAR, 달리기 SHIFT, 줍기 E, 인벤토리 TAP, 공격 MOUSE
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
