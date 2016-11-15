--select * from tblemployeerole

--update tblemployeerole set parentid=2 where id in (100,101)

--select * from tbllogin where id=164

--select * from tblemployee where id=111

--update tbllogin set tenantid='61897140-BDA0-437A-A6CB-7B2565D34B29' where id= 164

--update tblemployee set tenantid='61897140-BDA0-437A-A6CB-7B2565D34B29' where id= 111

--select * from tblcitynew

--select * from tblcity

--select * from tblstate

--select * from tblcountry


--insert into tblcitynew (TenantId,Code,StateId,CityName,CreatedDate,ModifiedDate,Status) select '7EA74E34-4F73-4609-A4F6-E0FA46E14D9E'as TenantId,Code,StateId,CityName,CreatedDate,ModifiedDate,Status from tblcity

--insert into tblStatenew (TenantId,Code,CountryId,StateName,CreatedDate,ModifiedDate,Status) select '7EA74E34-4F73-4609-A4F6-E0FA46E14D9E'as TenantId,Code,CountryId,StateName,CreatedDate,ModifiedDate,Status from tblstate

--insert into tblCountrynew (TenantId,Code,CountryName,MobileCode,CreatedDate,ModifiedDate,Status) select '7EA74E34-4F73-4609-A4F6-E0FA46E14D9E'as TenantId,Code,CountryName,MobileCode,CreatedDate,ModifiedDate,Status from tblcountry


---select * from [dbo].[TblEmpHierarchyNew]

---insert into TblEmpHierarchyNew (TenantId,Code,EDescription,ParentId) select '7EA74E34-4F73-4609-A4F6-E0FA46E14D9E'as TenantId,Code,EDescription,ParentId from TblEmpHierarchy

--select * from [dbo].[TblMileStoneStageNew]

--insert into [TblMileStoneStageNew] (TenantId,MileStoneStage,Roles) select '7EA74E34-4F73-4609-A4F6-E0FA46E14D9E'as TenantId,MileStoneStage,Roles from [dbo].[TblMileStoneStage]

--select * from  [dbo].[TblProductTypeNew]

--insert into [TblProductTypeNew] (TenantId,ProductType,CreatedDate,ModifiedDate,Status) select '7EA74E34-4F73-4609-A4F6-E0FA46E14D9E'as TenantId,ProductType,getdate(),getdate(),1 from [dbo].[TblProductType]

--select * from [dbo].[TblTransEmployeeTarget]

--insert into [TblTransEmployeeTargetNew] (TenantId,EmpId,EmployeeRoleId,FinancialYearId,TargetHike,IsAutomatic,Budget,QuarterlyTarget,MonthlyTarget,WeeklyTarget,CreatedDate,ModifiedDate,Status) select '7EA74E34-4F73-4609-A4F6-E0FA46E14D9E'as TenantId,EmpId,EmployeeRoleId,FinancialYearId,TargetHike,IsAutomatic,Budget,QuarterlyTarget,MonthlyTarget,WeeklyTarget,getdate(),getdate(),status from [dbo].[TblTransEmployeeTarget]

--select * from tblemployee

--select * from TblTransEmployeeTargetNew

--SELECT ET.*, FY.FinancialYear,ER.Code EmployeeRole,ER.Description FROM TblTransEmployeeTarget ET 
--JOIN TblFinancialYear FY ON FY.Id = 3
--JOIN TblEmployeeRole ER ON ER.Id =2 
--WHERE   ET.Status = 1 AND FY.Status = 1 AND ER.Status = 1 and ET.FinancialYearId = 3 and ET.EmployeeRoleId = 2 and ET.EmpId = 2

--SELECT ET.*, FY.FinancialYear,ER.Code EmployeeRole,ER.Description FROM TblEmployeeTargetNew ET 
--JOIN TblFinancialYear FY ON FY.Id = 3
--JOIN TblEmployeeRole ER ON ER.Id = 3
--WHERE   ET.Status = 1 AND FY.Status = 1 AND ER.Status = 1 
--and ET.FinancialYearId = 3
--and ET.EmployeeRoleId = 3
--and ET.TenantId='7EA74E34-4F73-4609-A4F6-E0FA46E14D9E'


