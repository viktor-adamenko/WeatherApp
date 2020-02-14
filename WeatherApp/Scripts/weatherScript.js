$(function() {
    let $cityInput = $('#cityInput');

    $('#input-form').submit(function(e) {
        e.preventDefault();

        getWeatherData();
    });

    function getWeatherData() {
        
        let value = $cityInput.val();

        $.ajax({
            url: '/Home/GetWeatherData',
            data: {
                cityName: value
            },
            success: function (data) {

                appendData(data);
                showRainAlert(data);

            },
            error: function(err) {
                console.log(err);
            }
        });
    }

    function appendData(data) {

        if (data.Cod === "200") {
            let iconUrl = `http://openweathermap.org/img/w/${data.Weather[0].Icon}.png`;

            $(".error-section").hide();

            $('#minTemp').text(data.Main.TempMin);
            $('#maxTemp').text(data.Main.TempMax);

            if (data.Rain) {
                $('#precipitation').text(data.Rain.Precipitation);
                $('.rain-info').show();
            } else 
                $('.rain-info').hide();

            $('#weatherIcon').css("background", `url('${iconUrl}') repeat scroll 0% 0% transparent`);

            $(".data-section").show();

        } else {

            $(".data-section").hide();
            $(".error-section").show();

            $('#error-text').text(data.Message);
        }
    }

    function showRainAlert(data) {
        var rainNotify = getCookie(data.Name);
        var rainShowAgain = getCookie("RainNotifyShowAgain");

        if (rainNotify && !rainShowAgain.includes(data.Name)) {
            alert("It rains!");
        }
    }

    function getCookie(cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    window.getWeatherData = getWeatherData;
});