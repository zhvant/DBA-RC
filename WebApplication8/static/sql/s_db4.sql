SELECT БД, [Размер БД],[Размер ЖТ] FROM dbo.db04v
where [БД] not in ('msdb','master','model')
order by [БД] 