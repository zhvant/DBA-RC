        $(document).ready(function(){
            $(".modalbox").fancybox();
        });
		
		
        // Получение всех сессий
        async function GetSessions() {
            // отправляет запрос и получаем ответ
            const response = await fetch("https://localhost:5001/api/session?server=localhost", {
                method: "GET",
                headers: { "Accept": "application/json" }
            });
            // если запрос прошел нормально
            if (response.ok === true) {
                // получаем данные
                const sessions = await response.json();
                let rows = document.querySelector("tbody"); 
                sessions.forEach(session => {
                    // добавляем полученные элементы в таблицу
                    rows.append(row(session));
                });
            }
        }

        
		// Создание всплывающего окна в фоне
		function CreatePopupWindow(pid, data)
		{
            var style = document.createElement("style");
            style.innerHTML = "#sqlText-"+pid+" {display:none;}";
			document.body.append(style);
			//"<style>#sqlText-"+pid+" {display:none;}</style>"+
			var div = document.createElement('div');
            div.setAttribute("id","sqlText-"+pid);          
            div.innerHTML = "<pre class='prettyprint lang-sql linenums:1'>"+data;           
            document.body.append(div);
            PR.prettyPrint();
		}
		

		
		/**
        * Generates a GUID string.
        * @returns {string} The generated GUID.
        * @example af8a8416-6e18-a307-bd9c-f2c947bbb3aa
        * @author Slavik Meltser.
        * @link http://slavik.meltser.info/?p=142
        */
       function guid() {
           function _p8(s) {
               var p = (Math.random().toString(16)+"000000000").substr(2,8);
               return s ? "-" + p.substr(0,4) + "-" + p.substr(4,4) : p ;
           }
           return _p8() + _p8(true) + _p8(true) + _p8();
       }
		
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
			a.append("Просмотр");
			return a
            //document.body.appendChild(a);
            //a.click();
            //window.setTimeout(() => {
            //    document.body.removeChild(a);
            //    window.URL.revokeObjectURL(url); 
            //    }, 2000);
        }


        function row(session) {
		
            const tr = document.createElement("tr");
            tr.setAttribute("data-rowid", session.sessionId);
 
            const durationColumn = document.createElement("td");
            durationColumn.append(session.duration);
            tr.append(durationColumn);
 
            const databaseNameColumn = document.createElement("td");
            databaseNameColumn.append(session.databaseName);
            tr.append(databaseNameColumn);
 
            const sessionIdColumn = document.createElement("td");
			const sessionIdLink = document.createElement("a");
            sessionIdLink.setAttribute("href","/kill?pid="+session.sessionId);
            sessionIdLink.setAttribute("onclick","return confirm('Kill spid "+session.sessionId+"?')");
            sessionIdLink.append(session.sessionId);
			sessionIdColumn.append(sessionIdLink);
            tr.append(sessionIdColumn);
			
			const hostNameColumn = document.createElement("td");
            hostNameColumn.append(session.hostName);
            tr.append(hostNameColumn);
			
			
			const loginNameColumn = document.createElement("td");
            loginNameColumn.append(session.loginName);
            tr.append(loginNameColumn);
			
			
			const blockingSessionIdColumn = document.createElement("td");
            blockingSessionIdColumn.append(session.blockingSessionId);
            tr.append(blockingSessionIdColumn);
			
			
			const blockedSessionCountColumn = document.createElement("td");
            blockedSessionCountColumn.append(session.blockedSessionCount);
            tr.append(blockedSessionCountColumn);
			
			
			const waitInfoColumn = document.createElement("td");
            waitInfoColumn.append(session.waitInfo);
            tr.append(waitInfoColumn);
			
			
			const programNameColumn = document.createElement("td");
            programNameColumn.append(session.programName);
            tr.append(programNameColumn);
			
			
			const cpuColumn = document.createElement("td");
            cpuColumn.append(session.cpu);
            tr.append(cpuColumn);
			
			
			const readsColumn = document.createElement("td");
            readsColumn.append(session.reads);
            tr.append(readsColumn);
			
			
			const physicalReadsColumn = document.createElement("td");
            physicalReadsColumn.append(session.physicalReads);
            tr.append(physicalReadsColumn);
			
			
			const writesColumn = document.createElement("td");
            writesColumn.append(session.writes);
            tr.append(writesColumn);
			

			const sqlCommandColumn = document.createElement("td");
			const sqlCommandLink = document.createElement("a");
            sqlCommandLink.setAttribute("href", "#sqlText-" + session.sessionId);
            sqlCommandLink.setAttribute("rel", "nofollow");
            sqlCommandLink.setAttribute("class", "modalbox");
			sqlCommandLink.append("Просмотр");
			sqlCommandColumn.append(sqlCommandLink);
            tr.append(sqlCommandColumn);
			CreatePopupWindow (session.sessionId,session.sqlCommand);
			
						
			const sqlTextColumn = document.createElement("td");
			const sqlTextLink = document.createElement("a");
            sqlTextLink.setAttribute("href", "#sqlText-" + session.sessionId);
            sqlTextLink.setAttribute("rel", "nofollow");
            sqlTextLink.setAttribute("class", "modalbox");
			sqlTextLink.append("Просмотр");
			sqlTextColumn.append(sqlTextLink);
            tr.append(sqlTextColumn);
			CreatePopupWindow (session.sessionId,session.sqlText);

			
			const queryPlanColumn = document.createElement("td");
			if  (session.queryPlan!="") {  queryPlanColumn.append(downloadFile(session.sessionId+"_queryPlan("+guid()+").sqlplan",session.queryPlan)) }      
            tr.append(queryPlanColumn);
			
						
			const statusColumn = document.createElement("td");
            statusColumn.append(session.status);
            tr.append(statusColumn);
			
			
			const percentCompleteColumn = document.createElement("td");
            percentCompleteColumn.append(session.percentComplete);
            tr.append(percentCompleteColumn);
			
						
            return tr;
        }

        GetSessions();
 
