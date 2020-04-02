SELECT 
	[disk]
	,[Total]
	,[Free]
	,[Limit]
FROM dbo.disk_size
WHERE [SERVER] = 'DB04'
and
[Total]<>1