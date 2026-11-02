-- Team Task Tracker smoke test
-- Purpose: quick "is the data flowing?" check after using the application.
-- Run this after seeding a user and creating/updating/deleting a task through
-- the API, so you can see both current state and audit history.

USE TeamTaskTracker;

-- Latest 5 tasks: current state only.
SELECT * FROM Tasks ORDER BY TaskId DESC LIMIT 5;

-- Latest 5 audit events: who did what, when, and before/after values.
SELECT AuditLogId, TaskId, ActorUserId, ActionType,
       ChangedFields, OldValues, NewValues, OccurredAtUtc
FROM TaskAuditLogs
ORDER BY AuditLogId DESC
LIMIT 5;

-- After deleting a task, its audit rows should remain with TaskId = NULL.
-- This query proves the ON DELETE SET NULL rule worked.
SELECT AuditLogId, TaskId, ActorUserId, ActionType, OldValues, OccurredAtUtc
FROM TaskAuditLogs
WHERE ActionType = 'Deleted'
ORDER BY AuditLogId DESC;

-- Counts help confirm no partial writes: a task mutation must always have an
-- audit event. For every task row, check its history in the second query.
SELECT (SELECT COUNT(*) FROM Users)         AS UserCount,
       (SELECT COUNT(*) FROM Tasks)         AS TaskCount,
       (SELECT COUNT(*) FROM TaskAuditLogs) AS AuditCount;
