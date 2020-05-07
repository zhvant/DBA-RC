// Получение всех баз данных сервера
async function GetDatabases() {
    // отправляет запрос и получаем ответ
    const response = await fetch("https://localhost:5001/api/database?server=localhost", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const databases = await response.json();
        let rows = document.querySelector("tbody"); 
        databases.forEach(database => {
            // добавляем полученные элементы в таблицу
            rows.append(row(database));
        });
    }
}

function row(database) {

    let server = "localhost";

    const tr = document.createElement("tr");
    //tr.setAttribute("data-rowid", 1);
    const idTd = document.createElement("td");
    const nameLink = document.createElement("a");
    nameLink.setAttribute("href","/storage?server="+server+"&database="+database.name);
    nameLink.append(database.name);  
    //idTd.append(database.name);   
    tr.append(nameLink);
     
    return tr;
}

GetDatabases();
 