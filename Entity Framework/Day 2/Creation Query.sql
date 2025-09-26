create database Student

-- Create the student database table
CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    City VARCHAR(100) NOT NULL,
);

-- Insert 10 random student entries
INSERT INTO Students (name, age, city) VALUES
('Emma Johnson', 20, 'New York'),
('Liam Chen', 22, 'San Francisco'),
('Sophia Martinez', 19, 'Los Angeles'),
('Noah Williams', 21, 'Chicago'),
('Olivia Brown', 20, 'Boston'),
('Lucas Garcia', 23, 'Miami'),
('Ava Davis', 19, 'Seattle'),
('Mia Rodriguez', 22, 'Austin'),
('Ethan Wilson', 21, 'Denver'),
('Isabella Lee', 20, 'Portland');
select * from Students