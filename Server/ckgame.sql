# user 테이블 생성
CREATE TABLE user
(
    id       VARCHAR(32)    NOT NULL,
    password VARBINARY(256) NOT NULL,
    PRIMARY KEY (id)
);

# 새 user 만들기
set @id = 'test_id';
set @pw = 'test1234';
INSERT user
VALUES (@id, SHA2(@pw, 256));

# 모든 user 가져오기
SELECT *
from user;

# 유저 찾기(Login)
SELECT 1
FROM user
WHERE id = @id
  AND password = SHA2(@pw, 256);


# record 테이블 생성
CREATE TABLE record
(
    user_id    VARCHAR(32) NOT NULL,
    distance   FLOAT       NOT NULL,
    created_at DATETIME    NOT NULL,
    FOREIGN KEY (user_id) REFERENCES user (id) ON DELETE CASCADE
);

# 새 record 만들기
set @user_id = 'test';
set @distance = 10.5;
INSERT record
VALUES (@user_id, @distance, NOW());
