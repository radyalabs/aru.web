var MapsGoogle = function () {
    var mapGeocoding = function () {
        
        var map = new GMaps({
            div: '#gmap_geocoding',
            lat: -12.043333,
            lng: -77.028333
        });
        
        var marker = new google.maps.Marker({
            position: { lat: -12.043333, lng: -77.028333 },
            map: map,
            title: 'Hello World!'
        });
        
        map.addMarker(marker);

        var handleAction = function () {
            var text = $.trim($('#gmap_geocoding_address').val());
            GMaps.geocode({
                address: text,
                callback: function (results, status) {
                    if (status == 'OK') {
                        var latlng = results[0].geometry.location;

                        $("input[name*='Latitude']").val(latlng.lat());
                        $("input[name*='Longitude']").val(latlng.lng());

                        map.setCenter(latlng.lat(), latlng.lng());

                        marker.setPosition = latlng;
                        //map.addMarker({
                        //    lat: latlng.lat(),
                        //    lng: latlng.lng(),
                        //    draggable: true
                        //});
                        
                        Metronic.scrollTo($('#gmap_geocoding'));
                    }
                }
            });
        }

        $('#gmap_geocoding_btn').click(function (e) {
            e.preventDefault();
            handleAction();
        });

        $("#gmap_geocoding_address").keypress(function (e) {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode == '13') {
                e.preventDefault();
                handleAction();
            }
        });

    }
    
    return {
        init: function () {
            mapGeocoding();
        }

    };

}();