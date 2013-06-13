delete from [_com_ccvonline_ResidencyProjectPointOfAssessment]
delete from [_com_ccvonline_ResidencyProject]
delete from [_com_ccvonline_ResidencyCompetency]
delete from [_com_ccvonline_ResidencyTrack]
delete from [_com_ccvonline_ResidencyPeriod]

INSERT INTO [_com_ccvonline_ResidencyPeriod] ([StartDate] ,[EndDate] ,[Name] ,[Description] ,[Guid]) VALUES 
  ('2013-08-01','2014-05-01','Fall 2013 - Spring 2014','First year of Residency Program at CCV',newid()),
  ('2014-08-01','2015-05-01','Fall 2014 - Spring 2015','Second year of Residency Program at CCV',newid()),
  ('2015-08-01','2016-05-01','Fall 2015 - Spring 2016','Third year of Residency Program at CCV',newid()),
  (null,null,'TBD','Some Period yet to be determind',newid())

INSERT INTO [_com_ccvonline_ResidencyTrack] ([Name], [Description], [ResidencyPeriodId], [Guid]) 
  select 'General', 'Required', [Id], newid() from _com_ccvonline_ResidencyPeriod
  union
  select 'Children and Family', '', [Id], newid() from _com_ccvonline_ResidencyPeriod
  union
  select 'Church Administration', '', [Id], newid() from _com_ccvonline_ResidencyPeriod
  union  
  select 'Church Planting', '', [Id], newid() from _com_ccvonline_ResidencyPeriod
  union
  select 'Inter-Cultural Studies', '', [Id], newid() from _com_ccvonline_ResidencyPeriod
  union
  select 'Pastoral Ministry', '', [Id], newid() from _com_ccvonline_ResidencyPeriod

insert into [_com_ccvonline_ResidencyCompetency] ([Name], [ResidencyTrackId], [Guid]) 
  select 'Events and Projects Management', [Id], newid() from _com_ccvonline_ResidencyTrack where Name = 'General'
  union
  select 'First Impressions', [Id], newid() from _com_ccvonline_ResidencyTrack where Name = 'General'
  union
  select 'Leadership Practices 500', [Id], newid() from _com_ccvonline_ResidencyTrack where Name = 'General'
  union
  select 'Leadership Practices 600', [Id], newid() from _com_ccvonline_ResidencyTrack where Name = 'General'
  union
  select 'Life of Christ (Israel Trip)', [Id], newid() from _com_ccvonline_ResidencyTrack where Name = 'General'

insert into [_com_ccvonline_ResidencyCompetency] ([Name], [ResidencyTrackId], [Guid]) 
  select 'Applied Homiletics', [Id], newid() from _com_ccvonline_ResidencyTrack where Name = 'Pastoral Ministry'
  union
  select 'Neighborhood Ministry', [Id], newid() from _com_ccvonline_ResidencyTrack where Name = 'Pastoral Ministry'
  union
  select 'Practical Ministry', [Id], newid() from _com_ccvonline_ResidencyTrack where Name = 'Pastoral Ministry'

insert into [_com_ccvonline_ResidencyProject] ([Name], [Description], [ResidencyCompetencyId], [Guid])
  select 'Project A', 'Perform directed biblical research for sermon preparation as directed', [Id], NEWID() from [_com_ccvonline_ResidencyCompetency] where Name = 'Applied Homiletics'
  union
  select 'Project B', 'Perform directed research for illustrated material or cultural analysis as directed', [Id], NEWID() from [_com_ccvonline_ResidencyCompetency] where Name = 'Applied Homiletics'

insert into [_com_ccvonline_ResidencyProjectPointOfAssessment] ([ResidencyProjectId], [AssessmentOrder], [AssessmentText], [Guid])
  select [Id], 1, 'Clearly understood the topic or text to be researched.', NEWID() from [_com_ccvonline_ResidencyProject] where [Name] = 'Project A'
  union
  select [Id], 2, 'Asked appropriate clarifying questions of the communicator necessary to do the research.', NEWID() from [_com_ccvonline_ResidencyProject] where [Name] = 'Project A'
  union
  select [Id], 3, 'Provided the research to the communicator in a timely fashion', NEWID() from [_com_ccvonline_ResidencyProject] where [Name] = 'Project A'
  union
  select [Id], 4, 'The material provided was used in the sermon or lesson', NEWID() from [_com_ccvonline_ResidencyProject] where [Name] = 'Project A'
  union
  select [Id], 5, 'Presented the material in an economy of words and ideas (not too much and not too little).', NEWID() from [_com_ccvonline_ResidencyProject] where [Name] = 'Project A'
  union
  select [Id], 6, 'Presented the material in a professional, clean, clearly articulated, and edited format.', NEWID() from [_com_ccvonline_ResidencyProject] where [Name] = 'Project A'

declare
  @groupTypeId int

select  @groupTypeId = [Id] from dbo.GroupType where Guid = '00043CE6-EB1B-43B5-A12A-4552B91A3E28';

delete from [dbo].[Group] where [Guid] = '4B7D22E8-B08C-42DC-B1F1-F2834BC8D1DF';

INSERT INTO [dbo].[Group] ([IsSystem],[ParentGroupId],[GroupTypeId],[CampusId],[Name],[Description],[IsSecurityRole],[IsActive],[Guid])
                            VALUES (0,null,@groupTypeId,null,'Residents - Fall 2013 to Spring 2014','Residents in the Residency program for the Fall 2013 to Spring 2014 period.',0,1,'4B7D22E8-B08C-42DC-B1F1-F2834BC8D1DF');
