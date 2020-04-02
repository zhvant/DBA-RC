SELECT БД, [Размер БД],[Размер ЖТ] FROM dbo.db02v
where [БД] not in ('msdb','master','model')
order by [БД] 