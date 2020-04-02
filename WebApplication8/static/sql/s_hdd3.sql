SELECT 
	[disk]
	,[Total]
	,[Free]
	,[Limit]
FROM dbo.disk_size
WHERE [SERVER] = 'DB03'
and
[Total]<>1