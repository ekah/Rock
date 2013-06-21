delete from [_com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessmentPointOfAssessment]
delete from [_com_ccvonline_Residency_CompetencyPersonProjectAssignmentAssessment]
delete from [_com_ccvonline_Residency_CompetencyPersonProjectAssignment]
delete from [_com_ccvonline_Residency_CompetencyPersonProject]
delete from [_com_ccvonline_Residency_CompetencyPerson]
delete from [_com_ccvonline_Residency_ProjectPointOfAssessment]
delete from [_com_ccvonline_Residency_Project]
delete from [_com_ccvonline_Residency_Competency]
delete from [_com_ccvonline_Residency_Track]
delete from [_com_ccvonline_Residency_Period]

INSERT INTO [_com_ccvonline_Residency_Period] ([StartDate] ,[EndDate] ,[Name] ,[Description] ,[Guid]) VALUES 
  ('2013-08-01','2014-05-01','Fall 2013 - Spring 2014','First year of Residency Program at CCV',newid()),
  ('2014-08-01','2015-05-01','Fall 2014 - Spring 2015','Second year of Residency Program at CCV',newid()),
  ('2015-08-01','2016-05-01','Fall 2015 - Spring 2016','Third year of Residency Program at CCV',newid()),
  (null,null,'TBD','Some Period yet to be determind',newid())

INSERT INTO [_com_ccvonline_Residency_Track] ([Name], [Description], [DisplayOrder], [PeriodId], [Guid]) 
  select 'General', 'Required', 0, [Id], newid() from _com_ccvonline_Residency_Period
  union
  select 'Children and Family', '', 1, [Id], newid() from _com_ccvonline_Residency_Period
  union
  select 'Church Administration', '', 2, [Id], newid() from _com_ccvonline_Residency_Period
  union  
  select 'Church Planting', '', 3, [Id], newid() from _com_ccvonline_Residency_Period
  union
  select 'Inter-Cultural Studies', '', 4, [Id], newid() from _com_ccvonline_Residency_Period
  union
  select 'Pastoral Ministry', '', 5, [Id], newid() from _com_ccvonline_Residency_Period

insert into [_com_ccvonline_Residency_Competency] ([Name], [TrackId], [Guid]) 
  select 'Events and Projects Management', [Id], newid() from _com_ccvonline_Residency_Track where Name = 'General'
  union
  select 'First Impressions', [Id], newid() from _com_ccvonline_Residency_Track where Name = 'General'
  union
  select 'Leadership Practices 500', [Id], newid() from _com_ccvonline_Residency_Track where Name = 'General'
  union
  select 'Leadership Practices 600', [Id], newid() from _com_ccvonline_Residency_Track where Name = 'General'
  union
  select 'Life of Christ (Israel Trip)', [Id], newid() from _com_ccvonline_Residency_Track where Name = 'General'

insert into [_com_ccvonline_Residency_Competency] ([Name], [TrackId], [Guid]) 
  select 'Applied Homiletics', [Id], newid() from _com_ccvonline_Residency_Track where Name = 'Pastoral Ministry'
  union
  select 'Neighborhood Ministry', [Id], newid() from _com_ccvonline_Residency_Track where Name = 'Pastoral Ministry'
  union
  select 'Practical Ministry', [Id], newid() from _com_ccvonline_Residency_Track where Name = 'Pastoral Ministry'

insert into [_com_ccvonline_Residency_Project] ([Name], [Description], [CompetencyId], [Guid])
  select 'Project A', 'Perform directed biblical research for sermon preparation as directed', [Id], NEWID() from [_com_ccvonline_Residency_Competency] where Name = 'Applied Homiletics'
  union
  select 'Project B', 'Perform directed research for illustrated material or cultural analysis as directed', [Id], NEWID() from [_com_ccvonline_Residency_Competency] where Name = 'Applied Homiletics'

insert into [_com_ccvonline_Residency_ProjectPointOfAssessment] ([ProjectId], [AssessmentOrder], [AssessmentText], [Guid])
  select [Id], 1, 'Clearly understood the topic or text to be researched.', NEWID() from [_com_ccvonline_Residency_Project] where [Name] = 'Project A'
  union
  select [Id], 2, 'Asked appropriate clarifying questions of the communicator necessary to do the research.', NEWID() from [_com_ccvonline_Residency_Project] where [Name] = 'Project A'
  union
  select [Id], 3, 'Provided the research to the communicator in a timely fashion', NEWID() from [_com_ccvonline_Residency_Project] where [Name] = 'Project A'
  union
  select [Id], 4, 'The material provided was used in the sermon or lesson', NEWID() from [_com_ccvonline_Residency_Project] where [Name] = 'Project A'
  union
  select [Id], 5, 'Presented the material in an economy of words and ideas (not too much and not too little).', NEWID() from [_com_ccvonline_Residency_Project] where [Name] = 'Project A'
  union
  select [Id], 6, 'Presented the material in a professional, clean, clearly articulated, and edited format.', NEWID() from [_com_ccvonline_Residency_Project] where [Name] = 'Project A'

declare
  @groupTypeId int

select  @groupTypeId = [Id] from dbo.GroupType where Guid = '00043CE6-EB1B-43B5-A12A-4552B91A3E28';

delete from [dbo].[Group] where [Guid] = '4B7D22E8-B08C-42DC-B1F1-F2834BC8D1DF';

INSERT INTO [dbo].[Group] ([IsSystem],[ParentGroupId],[GroupTypeId],[CampusId],[Name],[Description],[IsSecurityRole],[IsActive],[Guid])
                            VALUES (0,null,@groupTypeId,null,'Residents - Fall 2013 to Spring 2014','Residents in the Residency program for the Fall 2013 to Spring 2014 period.',0,1,'4B7D22E8-B08C-42DC-B1F1-F2834BC8D1DF');
