CREATE DATABASE IF NOT EXISTS develop2024;
use develop2024;

-- USER table
CREATE TABLE user(
    id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(14) NOT NULL,
    today_posts INT,
    total_posts INT,
    registration_date DATETIME
);

INSERT INTO user
(`username`,`today_posts`,`total_posts`,`registration_date`)
VALUES ('user1',0,0,NOW());

INSERT INTO user
(`username`,`today_posts`,`total_posts`,`registration_date`)
VALUES ('user2',0,4,NOW());

INSERT INTO user
(`username`,`today_posts`,`total_posts`,`registration_date`)
VALUES ('user3',0,1,NOW());

INSERT INTO user
(`username`,`today_posts`,`total_posts`,`registration_date`)
VALUES ('user4',0,0,NOW());

-- POST-TYPE table
CREATE TABLE post_type(
    id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(60),
    description VARCHAR(255)
);

INSERT INTO post_type (NAME, DESCRIPTION) VALUES('ORIGINAL', 'Post written by the user, not an interaction');
INSERT INTO post_type (NAME, DESCRIPTION) VALUES('REPOST', 'A repost from an original or a quote post, possible by an interaction from the user');
INSERT INTO post_type (NAME, DESCRIPTION) VALUES('QUOTE', 'A respost from an original or a quote post but with a comment from the user, possible by interaction');

-- POST table
CREATE TABLE post(
    id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    id_type INT NOT NULL,
    id_user INT NOT NULL,
    id_original_post INT,
    content VARCHAR(777),
    post_date DATETIME NOT NULL,
    FOREIGN KEY (id_type) REFERENCES post_type(id),
    FOREIGN KEY (id_user) REFERENCES user(id)
);


DELIMITER $$
CREATE PROCEDURE STP_GET_POST(IN post_id INT)
BEGIN
	SELECT p1.id, p1.id_type, post_type.name, p1.id_user, user.username, p1.id_original_post, p1.content, p1.post_date, (select p2.content from post as p2 where p2.id = p1.id_original_post) as 'original_content' FROM post as p1
	INNER JOIN post_type ON p1.id_type = post_type.id
	INNER JOIN user ON p1.id_user = user.id
	WHERE p1.id = post_id;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE STP_GET_POSTS(
	IN page_number INT, 
    IN page_size INT,
    IN user_id INT,
    IN start_date DATETIME,
    IN end_date DATETIME
)
BEGIN
	DECLARE offset_value INT;
	SET offset_value = (page_number - 1) * page_size;
    
    SET @page_size = page_size;
    SET @offset_value = offset_value;
    SET @start_date = start_date;
    SET @end_date = end_date;
    SET @user_id = user_id;
    
    -- BASE QUERY 
    SET @sql = CONCAT('SELECT p1.id, p1.id_type, post_type.name, p1.id_user, user.username, p1.id_original_post, p1.content, p1.post_date, 
		p2.content as second_content, p2.id_type as second_type, u2.username as second_username, p2.post_date as second_post_date, p2.id_original_post as second_id_original, 
		p3.content as first_content, p3.id_type as first_type, u3.username as first_username, p3.post_date as first_post_date FROM post as p1');
    SET @sql = CONCAT(@sql, ' INNER JOIN post_type ON p1.id_type = post_type.id
		INNER JOIN user ON p1.id_user = user.id 
		LEFT JOIN post AS p2 ON p2.id = p1.id_original_post 
		LEFT JOIN post AS p3 ON p3.id = p2.id_original_post 
		LEFT JOIN user AS u2 ON u2.id = p2.id_user 
		LEFT JOIN user AS u3 ON u3.id = p3.id_user');
    SET @sql = CONCAT(@sql, ' WHERE p1.post_date >= \'', @start_date, '\'');
    
    -- FILTER TO user_id FILLED
    IF user_id IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND p1.id_user = ', @user_id);
    END IF;
    
    -- FILTER TO end_date FILLED
    IF end_date IS NOT NULL THEN
        SET @sql = CONCAT(@sql, ' AND p1.post_date <= \'', @end_date, '\'');
    END IF;
    
    -- ADD ORDER BY AND PAGINATION
    SET @sql = CONCAT(@sql, ' ORDER BY p1.post_date DESC LIMIT ', @page_size, ' OFFSET ',@offset_value);
    
    PREPARE stmt FROM @sql;
    EXECUTE stmt;    
    DEALLOCATE PREPARE stmt;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE STP_GET_POST_TYPES()
BEGIN
	SELECT * FROM post_type;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE STP_INSERT_POST(IN pid_type INT, IN pid_user INT, IN pid_original_post INT, IN pcontent VARCHAR(777), IN ppost_date DATETIME)
BEGIN
	INSERT INTO post
	(id_type,id_user,id_original_post,content,post_date)
	VALUES
	(pid_type,pid_user,pid_original_post,pcontent,ppost_date);
    
    UPDATE user 
    SET 
    today_posts = today_posts + 1,
    total_posts = total_posts + 1
    WHERE id = pid_user;
END$$
DELIMITER ;


DELIMITER $$
CREATE PROCEDURE STP_GET_USER(IN user_id INT)
BEGIN
	SELECT * FROM user WHERE id = user_id;
END$$
DELIMITER ;


SET GLOBAL event_scheduler = ON;
DROP EVENT IF EXISTS es_reset_today_posts;

-- Create event to reset today_posts field everyday
CREATE EVENT es_reset_today_posts
ON SCHEDULE EVERY 1 DAY
STARTS CURRENT_DATE + INTERVAL 1 DAY
DO
    UPDATE user 
    SET today_posts = 0;

