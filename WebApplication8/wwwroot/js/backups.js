// Получение всех бекапов
async function GetBackups() {
    // отправляет запрос и получаем ответ
    const response = await fetch("https://localhost:5001/api/backup?server=localhost", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const backups = await response.json();
        let rows = document.querySelector("tbody"); 
        backups.forEach(backup => {
            // добавляем полученные элементы в таблицу
            rows.append(row(backup));
        });
    }
}

function row(backup) {

    const tr = document.createElement("tr");
    //tr.setAttribute("data-rowid", 1);

    const idTd = document.createElement("td");
    idTd.append(backup.dbName);
    tr.append(idTd);

    const nameTd = document.createElement("td");
    nameTd.append(backup.dateOfLastBackup);
    tr.append(nameTd);

    const backup_typeTd = document.createElement("td");
    backup_typeTd.append(backup.backup_type);
    tr.append(backup_typeTd);
	
	const physical_Device_nameTd = document.createElement("td");
    physical_Device_nameTd.append(backup.physical_Device_name);
    tr.append(physical_Device_nameTd);
     
    return tr;
}

GetBackups();
 