function renderSteckdosen() {
	var steckdosenApi = 'http://192.168.178.47:9000/api/steckdosen/get?raumId=1';
	$.getJSON(steckdosenApi,
        function (data) {
            var htmlString = "<div class=\"container\">\r\n";

			for (var i = 0; i < data.length; i++){
				htmlString += "<div class=\"container-fluid\">\r\n";
				htmlString += "<div class=\"row\">\r\n";
				htmlString += "<div class=\"col-xs-8\">";
				htmlString += "<h4>" + data[i].Name + "</h4>"; 
				htmlString += "</div>";
				htmlString += "<div class=\"col-xs-2\">";
				htmlString += "<a class=\"btn btn-success btn-lg\" type=\"submit\" onclick=\"outletOn('" + data[i].HausCode + "', '" + data[i].DeviceCode + "')\" >An </a>";
				htmlString += "</div>";
				htmlString += "<div class=\"col-xs-1\">";
				if(data[i].Secure == true) {
					htmlString += "<a class=\"btn btn-danger btn-lg\" data-toggle=\"modal\" data-outletname=\"" + data[i].Name + "\" data-target=\"#secureOff\" data-devicecode=\"" + data[i].DeviceCode + "\" data-homecode=\"" + data[i].HausCode + "\" type=\"submit\" >Aus</a>";
				}
				else {
					htmlString += "<a class=\"btn btn-danger btn-lg\" type=\"submit\" onclick=\"outletOff('" + data[i].HausCode + "', '" + data[i].DeviceCode + "')\" >Aus</a>";
				}
				htmlString += "</div>";
				htmlString += "</div>";
				htmlString += "<p> </p>";
				htmlString += "</div>";
			}
			
            htmlString += "</div>";
            document.getElementById("steckdosenContainer").insertAdjacentHTML('beforeend', htmlString);
        });
}

function outletOn(homeCode, handleCode) {
	var uri = "http://192.168.178.47:9000/api/steckdosen/switch?hc=" + homeCode + "&dc=" + handleCode + "&s=on";
    $.get(uri);
}

function outletOff(homeCode, handleCode) {
	var uri = "http://192.168.178.47:9000/api/steckdosen/switch?hc=" + homeCode + "&dc=" + handleCode + "&s=off";
	$.get(uri);
}


function renderSensors() {
    var sensorApi = 'http://192.168.178.47:9998/api/get/Sensor';
    $.getJSON(sensorApi,
        function (data) {
            var htmlString = "";

            for (var i = 0; i < data.length; i++) {
                var typ = "";
                var unit = "";

                if (data[i].Fk_SensorTyp_Id == "1") {
                    typ = "Temperatur: ";
                    unit = " °C";
                } else {
                    typ = "Luftfeuchtigkeit: ";
                    unit = " %";
                }

                htmlString += "<p>" + typ + data[i].LastValue + unit + "</p>";
            }

            document.getElementById("sensorsContainer").insertAdjacentHTML('beforeend', htmlString);
        });
}