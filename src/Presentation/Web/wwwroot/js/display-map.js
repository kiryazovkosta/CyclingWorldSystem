window.onload = (event) => {
    console.log("page is fully loaded");

    let map = new maplibregl.Map({
        container: 'map',
        style: 'https://api.maptiler.com/maps/streets/style.json?key=get_your_own_OpIi9ZULNHzrESv6T2vL',
        center: getStartCoordinate(),
        zoom: 11
    });

    map.on('load', function () {
        map.addSource('route', {
            'type': 'geojson',
            'data': {
                'type': 'Feature',
                'properties': {},
                'geometry': {
                    'type': 'LineString',
                    'coordinates': getCoordinates()
                }
            }
        });

        map.addLayer({
            'id': 'route',
            'type': 'line',
            'source': 'route',
            'layout': {
                'line-join': 'round',
                'line-cap': 'round'
            },
            'paint': {
                'line-color': '#888',
                'line-width': 8
            }
        });
    });

    function getStartCoordinate() {
        let coordinate = [];
        let activityId = document.getElementById('Id').value;
        let formData = new FormData();
        formData.append('activityId', activityId);
        let verificationToken = $('input[name="__RequestVerificationToken"]').val();
        console.log(verificationToken);
        $.ajax({
            "async": false,
            "url": '/Ajax/GetStartCoordinate',
            "method": "Post",
            "headers": {
                RequestVerificationToken: verificationToken
            },
            "processData": false,
            "contentType": false,
            "data": formData
        }).done(function (response) {
                coordinate.push(response.longitude, response.latitude);
        }).fail(function (data) {
            alert("There is an error with fetching tack for activity!");
        }).always(function () {
        });

        console.log(coordinate);
        return coordinate;
    }

    function getCoordinates() {
        let coordinates = [];
        let activityId = document.getElementById('Id').value;
        let formData = new FormData();
        formData.append('activityId', activityId);
        let verificationToken = $('input[name="__RequestVerificationToken"]').val();
        console.log(verificationToken);
        $.ajax({
            "async": false,
            "url": '/Ajax/GetCoordinates',
            "method": "Post",
            "headers": {
                RequestVerificationToken: verificationToken
            },
            "processData": false,
            "contentType": false,
            "data": formData
        }).done(function (response) {
            for (let i = 0; i < response.length; i+=5) {
                coordinates.push([response[i].longitude, response[i].latitude]);
            }
        }).fail(function (data) {
            alert("There is an error with fetching tack for activity!");
        }).always(function () {
        });

        return coordinates;
    }
};



