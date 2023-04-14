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
