-- Access DDL and sample data for AttendanceDB.accdb

CREATE TABLE Students (
  StudentID COUNTER PRIMARY KEY,
  RegNo TEXT(50),
  FirstName TEXT(255),
  LastName TEXT(255),
  DOB DATETIME,
  [Class] TEXT(255),
  Section TEXT(255),
  Gender TEXT(50),
  PhotoPath TEXT(255)
);

CREATE TABLE Attendance (
  AttendanceID COUNTER PRIMARY KEY,
  StudentID LONG,
  AttDate DATETIME,
  [Status] TEXT(50),
  Remarks TEXT(255)
);

-- Sample Inserts
INSERT INTO Students (RegNo,FirstName,LastName,DOB,[Class],Section,Gender,PhotoPath) VALUES ('REG001','John','Doe',#2008-05-12#,'7','A','Male','');
INSERT INTO Students (RegNo,FirstName,LastName,DOB,[Class],Section,Gender,PhotoPath) VALUES ('REG002','Jane','Smith',#2009-08-03#,'7','A','Female','');
INSERT INTO Students (RegNo,FirstName,LastName,DOB,[Class],Section,Gender,PhotoPath) VALUES ('REG003','Ali','Khan',#2008-11-20#,'7','B','Male','');
INSERT INTO Students (RegNo,FirstName,LastName,DOB,[Class],Section,Gender,PhotoPath) VALUES ('REG004','Maria','Gonzalez',#2009-01-15#,'6','A','Female','');
INSERT INTO Students (RegNo,FirstName,LastName,DOB,[Class],Section,Gender,PhotoPath) VALUES ('REG005','Chen','Li',#2008-03-02#,'7','B','Male','');

INSERT INTO Attendance (StudentID,AttDate,[Status],Remarks) VALUES (1,#2025-12-10#,'Present','');
INSERT INTO Attendance (StudentID,AttDate,[Status],Remarks) VALUES (2,#2025-12-10#,'Absent','Sick');
INSERT INTO Attendance (StudentID,AttDate,[Status],Remarks) VALUES (3,#2025-12-10#,'Present','');
INSERT INTO Attendance (StudentID,AttDate,[Status],Remarks) VALUES (4,#2025-12-10#,'Late','Traffic');
INSERT INTO Attendance (StudentID,AttDate,[Status],Remarks) VALUES (5,#2025-12-10#,'Present','');
INSERT INTO Attendance (StudentID,AttDate,[Status],Remarks) VALUES (1,#2025-12-11#,'Absent','Sick');
INSERT INTO Attendance (StudentID,AttDate,[Status],Remarks) VALUES (2,#2025-12-11#,'Present','');
INSERT INTO Attendance (StudentID,AttDate,[Status],Remarks) VALUES (3,#2025-12-11#,'Present','');
