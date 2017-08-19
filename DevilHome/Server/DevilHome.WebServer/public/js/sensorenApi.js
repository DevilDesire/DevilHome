function renderChart(){
	var tempSensoren = 'http://192.168.178.47:9000/api/sensoren/get?sensorId=1';
	var tempsValue = [];
	var tempsDate = [];
	var presets = window.chartColors;
	var inputs = {
		min: -15,
		max: 50,
		count: 24,
		decimals: 2,
		continuity: 1
	};

	var tempctrl = document.getElementById("wohnzimmertemp");
	$.getJSON(tempSensoren, function(data){
		$.each(data, function(i, obj) {
			tempsValue.push(obj.Value);
			var localDate = obj.Date;
			tempsDate.push(localDate);
			tempctrl.innerHTML = obj.Value + " °C";
		});
		
		new Chart('tempChart', {
				type: 'line',
				data: {
					labels: tempsDate,
					datasets: [{
						backgroundColor: "#78909C",
						borderColor: "#455A64",
						data: tempsValue,
						label: 'Temperatur in °C',
						fill: "start"
					}]
				}
			});
		
	});
}

function renderChart2(){
	var presets = window.chartColors;
	var inputs = {
		min: 0,
		max: 100,
		count: 24,
		decimals: 2,
		continuity: 1
	};
	var luftSensoren = 'http://192.168.178.47:9000/api/sensoren/get?sensorId=2';
	var luftsValue = [];
	var luftsDate = [];
	var luftsctrl = document.getElementById("wohnzimmerluft");
	$.getJSON(luftSensoren, function(data){
		$.each(data, function(i, obj) {
			luftsValue.push(obj.Value);
			var localDate = obj.Date;
			luftsDate.push(localDate);
			luftsctrl.innerHTML = obj.Value + " %";
		});
		
		new Chart('luftChart', {
				type: 'line',
				data: {
					labels: luftsDate,
					datasets: [{
						backgroundColor: "#78909C",
						borderColor: "#455A64",
						data: luftsValue,
						label: 'relative Luftfeuchtigkeit in %',
						fill: "start"
					}]
				}
			});
		
	});
}