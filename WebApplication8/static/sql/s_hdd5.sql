SELECT 
	[disk]
	,[Total]
	,[Free]
	,[Limit]
FROM dbo.disk_size
WHERE [SERVER] = 'DB05'
and
[Total]<>1