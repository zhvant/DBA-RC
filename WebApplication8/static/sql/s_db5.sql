SELECT БД, [Размер БД],[Размер ЖТ] FROM dbo.db05v
where [БД] not in ('msdb','master','model')
order by [БД] 