sp = window.location.href.indexOf("?server=") + 8;
ep = window.location.href.indexOf("&");
if (ep === -1) { ep = window.location.href.length; }
srv = window.location.href.substr(sp, ep);


sp = window.location.href.indexOf("&database=") + 10;
//ep = window.location.href.indexOf("&");
if (ep === -1) { ep = window.location.href.length; }
database = window.location.href.substr(sp, ep);

//Скрыть таблицу
fileGroupsTable = document.getElementById("fileGroupsTable");
fileGroupsTable.style.visibility = "hidden";
//Скрыть таблицу
dataFilesTable = document.getElementById("dataFilesTable");
dataFilesTable.style.visibility = "hidden";

webserver = "localhost:5001";


// Получение всех файловых групп
async function GetFileGroups() {
    // отправляет запрос и получаем ответ
    const response = await fetch("https://" + webserver +"/api/filegroup?server="+srv+"&database="+database, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const filegroups = await response.json();
        let rows = document.getElementById("FileGroupsBody"); 
        filegroups.forEach(filegroup => {
            // добавляем полученные элементы в таблицу
            rows.append(fg_row(filegroup));
        });

        //Показать таблицу
        fileGroupsTable.style.visibility = "visible";

        //Убрать спиннер
        var elem = document.getElementById("fileGroupsSpinner");
        elem.parentNode.removeChild(elem);
    }
}

function fg_row(filegroup) {

    const tr = document.createElement("tr");
    //tr.setAttribute("data-rowid", 1);

    const idTd = document.createElement("td");
    idTd.append(filegroup.fileGroupName);
    tr.append(idTd);

    const nameTd = document.createElement("td");
    nameTd.append(parseFloat(filegroup.availableSpace / 1024).toFixed(2));
    tr.append(nameTd);
    
    return tr;
}

GetFileGroups();
 

// Получение всех файлов данных
async function GetDataFiles() {
    // отправляет запрос и получаем ответ
    const response = await fetch("https://" + webserver +"/api/datafile?server=" + srv + "&database=" + database, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const filegroups = await response.json();
        let rows = document.getElementById("DataFilesBody"); 
        filegroups.forEach(datafile => {
            // добавляем полученные элементы в таблицу
            rows.append(df_row(datafile));
        });

        //Показать таблицу
        dataFilesTable.style.visibility = "visible";

        //Убрать спиннер
        var elem = document.getElementById("dataFilesSpinner");
        elem.parentNode.removeChild(elem);
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
    usedSpaceTd.append(parseFloat(datafile.usedSpace / 1024).toFixed(2));
    tr.append(usedSpaceTd);

    const maxSpaceTd = document.createElement("td");
    maxSpace = parseFloat(datafile.maxSpace / 1024).toFixed(2);
    (maxSpace == 0.00) ? maxSpaceTd.append("Не ограничен") : maxSpaceTd.append(parseFloat(datafile.maxSpace / 1024).toFixed(2)) ;
    
    tr.append(maxSpaceTd);

    const availableSpaceTd = document.createElement("td");
    availableSpaceTd.append(parseFloat(datafile.availableSpace / 1024).toFixed(2));
    tr.append(availableSpaceTd);
    
    return tr;
}

GetDataFiles();