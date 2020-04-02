SELECT БД, [Размер БД],[Размер ЖТ] FROM dbo.db01v
where [БД] not in ('msdb','master','model')
order by [БД] 