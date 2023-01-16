-- ROLES

INSERT INTO public."Roles"
("RoleId", "RoleCode", "RoleName")
VALUES('7890a897-e989-471f-8944-9a5064831477', 'DRIVER', 'Driver');
INSERT INTO public."Roles"
("RoleId", "RoleCode", "RoleName")
VALUES('94f33521-757f-4bd6-9101-9c76ea274a71', 'SALES', 'Sales');
INSERT INTO public."Roles"
("RoleId", "RoleCode", "RoleName")
VALUES('b3a39022-2a67-41b2-aa69-0a854afc0c9a', 'SA	Super', 'Admin');
INSERT INTO public."Roles"
("RoleId", "RoleCode", "RoleName")
VALUES('c17e55af-f2b8-4755-a3db-b98d5a63c10f', 'SPV', 'SPV');
INSERT INTO public."Roles"
("RoleId", "RoleCode", "RoleName")
VALUES('e1c771c9-0bac-41bf-8593-f552f69b4cda', 'OPR', 'Operator');

-- SUPER ADMINSTRATOR
INSERT INTO public."Accounts"
("AccountId", "Password", "CurrentToken", "RoleCode", "RoleId", "LastLoginDt", "IsPushNotifActive", "FCMToken", "DeviceId", "CreatedDt", "UpdatedDt", "CreatedBy", "UpdatedBy", "IsActive", "IsDeleted")
VALUES('d3c1609f-ee73-4da7-8cf7-6d80b629b631', '9b29b1d990ae54e3154843982ded7e6a11f161e454ddf94b82b7849584c92e28', '', 'SA', 'b3a39022-2a67-41b2-aa69-0a854afc0c9a', '2018-04-04 13:40:32.990', false, NULL, NULL, '2018-04-04 13:40:32.990', NULL, 'System', NULL, 1, 0);

INSERT INTO public."Users"
("UserId", "AccountId", "UserCode", "UserName", "UserEmail", "UserPhone", "CreatedDt", "UpdatedDt", "CreatedBy", "UpdatedBy", "IsActive", "IsDeleted", "FullName")
VALUES('ce0b067f-207c-474c-96da-76bf43b4acfd', 'd3c1609f-ee73-4da7-8cf7-6d80b629b631', 'SPADM', 'admin', 'super.admin@mail.com', '6282129290963', '2018-04-04 13:46:30.286', NULL, 'System', NULL, 1, 0, NULL);


-- Assignment status
INSERT INTO "AssignmentStatuses" 
("AssignmentStatusCode", "AssignmentStatusName", "Description")
VALUES(N'AGENT_ARRIVED', N'AGENT ARRIVED', N'Agent arrived to store location');
INSERT INTO "AssignmentStatuses" 
("AssignmentStatusCode", "AssignmentStatusName", "Description")
VALUES(N'AGENT_STARTED', N'AGENT STARTED', N'Agent start the task');
INSERT INTO "AssignmentStatuses" 
("AssignmentStatusCode", "AssignmentStatusName", "Description")
VALUES(N'TASK_COMPLETED', N'COMPLETED', N'Agent complete the task');
INSERT INTO "AssignmentStatuses" 
("AssignmentStatusCode", "AssignmentStatusName", "Description")
VALUES(N'TASK_FAILED', N'FAILED', N'Agent set task to failed');
INSERT INTO "AssignmentStatuses" 
("AssignmentStatusCode", "AssignmentStatusName", "Description")
VALUES(N'TASK_RECEIVED', N'RECEIVED', N'Task receive to agent');
