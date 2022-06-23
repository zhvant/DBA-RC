/**
* Инициирует загрузку файла
* @param {string} filename - имя загружаемого файла
* @param {string} data - данные в файле
*/
function downloadFile(filename, data) 
{
    var a = document.createElement('a');
    //a.style = "display: none";  
    var blob = new Blob([data], {type: "application/octet-stream"});
    var url = window.URL.createObjectURL(blob);
    a.href = url;
    a.download = filename;
	a.append("View");
	return a
    //document.body.appendChild(a);
    //a.click();
    //window.setTimeout(() => {
    //    document.body.removeChild(a);
    //    window.URL.revokeObjectURL(url); 
    //    }, 2000);
}