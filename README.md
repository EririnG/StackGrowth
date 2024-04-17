# Krafton Jungle 12-13 Weeks Project
# Mini Board
## 요구사항
- [ ] 회원가입 / 로그인
- [ ] 게시물 작성
- [ ] 게시물 목록 보기
- [ ] 게시물 읽기
- [ ] 댓글 작성

++++++++++++++++

- [ ] 클라이언트 움직임
- [ ] 동적 생성
- [ ] 게시물 삭제
- [ ] 게시물 수정

## 구성

- Client : Unity (2022.3.24f1)
- Server : C++
- DB : MySQL
![image](https://github.com/EririnG/StackGrowth/assets/78525107/5b79b68b-22ae-4e46-8e6b-abb021a80eb6)

# Client
![image](https://github.com/EririnG/StackGrowth/assets/78525107/0ef313bf-8a7d-4c4c-ac89-93dd8925b6d9)

LoginScene
- UGUI 상호작용
- 로그인 유효검사
  - PW와 RepeatPW가 일치하는지 검사
- 씬 이동, 이벤트 처리

![image](https://github.com/EririnG/StackGrowth/assets/78525107/38bf0b83-cb21-4a66-82aa-06d4649f5921)

InGameScene
- 움직임 처리
- UGUI 상호작용
- 비 정상적인 애니메이션 처리
- 프리팹 처리
- 동적 생성

# Server
![image](https://github.com/EririnG/StackGrowth/assets/78525107/b9f449f2-add2-4a8b-8970-d38823e42480)

![image](https://github.com/EririnG/StackGrowth/assets/78525107/1246d489-8bad-4b5b-ab9e-acdf7c74e4c2)

![image](https://github.com/EririnG/StackGrowth/assets/78525107/092dc88a-a90b-488a-8bb8-d69e762c315f)

- 클라이언트 - 서버 연결
- DB 연결
  - 게시물 작성
- DB와 서버 - 클라이언트 상호작용
  - 회원가입 / 로그인
  - 게시물 목록 보기
  - 게시물 읽기
 




