SELECT 
	[disk]
	,[Total]
	,[Free]
	,[Limit]
FROM dbo.disk_size
WHERE [SERVER] = 'DB01'
and
[Total]<>1