SELECT 
	[disk]
	,[Total]
	,[Free]
	,[Limit]
FROM dbo.disk_size
WHERE [SERVER] = 'DB02'
and
[Total]<>1