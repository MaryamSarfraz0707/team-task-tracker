-- Team Task Tracker schema (MySQL 8.0+)
-- Database-first: this script is the source of truth for the database branch.
-- Run as a database administrator/developer in MySQL Workbench.
--
-- Why this file exists:
--   It creates the whole database from zero: the database itself, three tables,
--   keys, constraints, and indexes. Anyone who clones the repo can rebuild the
--   exact same database by running this one file. The MySQL server stores the
--   real data; this file is the reproducible recipe for that server.

CREATE DATABASE IF NOT EXISTS TeamTaskTracker
  CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE TeamTaskTracker;

CREATE TABLE IF NOT EXISTS Users (
    UserId          BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
    FullName        VARCHAR(100) NOT NULL,
    Email           VARCHAR(254) NOT NULL,
    PasswordHash    VARCHAR(255) NOT NULL,
    IsActive        BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAtUtc    DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAtUtc    DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
                                      ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (UserId),
    CONSTRAINT UQ_Users_Email UNIQUE (Email),
    CONSTRAINT CK_Users_FullName_NotBlank CHECK (CHAR_LENGTH(TRIM(FullName)) > 0),
    CONSTRAINT CK_Users_Email_NotBlank CHECK (CHAR_LENGTH(TRIM(Email)) > 0)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS Tasks (
    TaskId              BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
    Title               VARCHAR(100) NOT NULL,
    Description         VARCHAR(500) NULL,
    Priority            ENUM('Low','Medium','High') NOT NULL DEFAULT 'Medium',
    Status              ENUM('ToDo','InProgress','Done') NOT NULL DEFAULT 'ToDo',
    DueDate             DATE NULL,
    CreatedByUserId     BIGINT UNSIGNED NOT NULL,
    UpdatedByUserId     BIGINT UNSIGNED NULL,
    CreatedAtUtc        DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAtUtc        DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
                                         ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (TaskId),
    KEY IX_Tasks_Status_DueDate (Status, DueDate),
    KEY IX_Tasks_CreatedByUserId (CreatedByUserId),
    KEY IX_Tasks_UpdatedByUserId (UpdatedByUserId),
    CONSTRAINT CK_Tasks_Title_NotBlank CHECK (CHAR_LENGTH(TRIM(Title)) > 0),
    CONSTRAINT FK_Tasks_CreatedByUser FOREIGN KEY (CreatedByUserId)
        REFERENCES Users(UserId) ON UPDATE RESTRICT ON DELETE RESTRICT,
    CONSTRAINT FK_Tasks_UpdatedByUser FOREIGN KEY (UpdatedByUserId)
        REFERENCES Users(UserId) ON UPDATE RESTRICT ON DELETE SET NULL
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS TaskAuditLogs (
    AuditLogId          BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
    TaskId              BIGINT UNSIGNED NULL,
    ActorUserId         BIGINT UNSIGNED NOT NULL,
    ActionType          ENUM('Created','Updated','StatusChanged','Deleted') NOT NULL,
    ChangedFields       JSON NULL,
    OldValues           JSON NULL,
    NewValues           JSON NULL,
    OccurredAtUtc       DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (AuditLogId),
    KEY IX_TaskAuditLogs_TaskId_OccurredAtUtc (TaskId, OccurredAtUtc),
    KEY IX_TaskAuditLogs_ActorUserId_OccurredAtUtc (ActorUserId, OccurredAtUtc),
    CONSTRAINT FK_TaskAuditLogs_Task FOREIGN KEY (TaskId)
        REFERENCES Tasks(TaskId) ON UPDATE RESTRICT ON DELETE SET NULL,
    CONSTRAINT FK_TaskAuditLogs_Actor FOREIGN KEY (ActorUserId)
        REFERENCES Users(UserId) ON UPDATE RESTRICT ON DELETE RESTRICT
) ENGINE=InnoDB;

-- IMPORTANT:
-- Never store a plaintext password. PasswordHash must contain a hash created by
-- a trusted password-hashing implementation (e.g. ASP.NET Core PasswordHasher).
-- Do not seed a real password or a reusable production credential in this file.
-- Audit rows for task deletion retain the actor and values while TaskId becomes NULL.
