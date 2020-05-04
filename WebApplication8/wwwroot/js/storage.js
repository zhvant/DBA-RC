sp = window.location.href.indexOf("?server=") + 8;
ep = window.location.href.indexOf("&");
if (ep === -1) { ep = window.location.href.length; }
srv = window.location.href.substr(sp, ep);

sp = window.location.href.indexOf("&database=") + 10;
//ep = window.location.href.indexOf("&");
if (ep === -1) { ep = window.location.href.length; }
database = window.location.href.substr(sp, ep);




// Получение всех файловых групп
async function GetFileGroups() {
    // отправляет запрос и получаем ответ
    const response = await fetch("https://localhost:5001/api/filegroup?server="+srv+"&database="+database, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const filegroups = await response.json();
        let rows = document.getElementById("FileGroupsTable"); 
        filegroups.forEach(filegroup => {
            // добавляем полученные элементы в таблицу
            rows.append(fg_row(filegroup));
        });
    }
}

function fg_row(filegroup) {

    const tr = document.createElement("tr");
    //tr.setAttribute("data-rowid", 1);

    const idTd = document.createElement("td");
    idTd.append(filegroup.fileGroupName);
    tr.append(idTd);

    const nameTd = document.createElement("td");
    nameTd.append(filegroup.availableSpace/1024);
    tr.append(nameTd);
    
    return tr;
}

GetFileGroups();
 

// Получение всех файлов данных
async function GetDataFiles() {
    // отправляет запрос и получаем ответ
    const response = await fetch("https://localhost:5001/api/datafile?server=" + srv + "&database=" + database, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const filegroups = await response.json();
        let rows = document.getElementById("DataFilesTable"); 
        filegroups.forEach(datafile => {
            // добавляем полученные элементы в таблицу
            rows.append(df_row(datafile));
        });
    }
}

function df_row(datafile) {

    const tr = document.createElement("tr");
    //tr.setAttribute("data-rowid", 1);

    const filegroupNameTd = document.createElement("td");
    filegroupNameTd.append(datafile.fileGroupName);
    tr.append(filegroupNameTd);

    const nameTd = document.createElement("td");
    nameTd.append(datafile.name);
    tr.append(nameTd);

    const usedSpaceTd = document.createElement("td");
    usedSpaceTd.append(datafile.usedSpace/1024);
    tr.append(usedSpaceTd);

    const maxSpaceTd = document.createElement("td");
    maxSpaceTd.append(datafile.maxSpace/1024);
    tr.append(maxSpaceTd);

    const availableSpaceTd = document.createElement("td");
    availableSpaceTd.append(datafile.availableSpace/1024);
    tr.append(availableSpaceTd);
    
    return tr;
}

GetDataFiles();