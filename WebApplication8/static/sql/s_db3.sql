SELECT БД, [Размер БД],[Размер ЖТ] FROM dbo.db03v
where [БД] not in ('msdb','master','model')
order by [БД] 