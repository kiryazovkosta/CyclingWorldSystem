window.onload = (event) => {
    console.log("page is fully loaded");

    let waypointsList = getCoordinates();
    
    let map = new maplibregl.Map({
        container: 'map',
        style: 'https://api.maptiler.com/maps/streets/style.json?key=get_your_own_OpIi9ZULNHzrESv6T2vL',
        center: waypointsList[0],
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
                    'coordinates': waypointsList
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
            console.log(response)
            for (let i = 0; i < response.length; i++) {
                coordinates.push([response[i].longitude, response[i].latitude]);
            }
            console.log(coordinates);
        }).fail(function (data) {
            alert("There is an error with fetching tack for activity!");
        }).always(function () {
        });

        return coordinates;
    }

    let likeButton = document.getElementById('like-activity');
    if (likeButton !== undefined && likeButton !== null) {
        likeButton.addEventListener('click', likeActivity);
    }

    let dislikeButton = document.getElementById('dislike-activity');
    if (dislikeButton !== undefined && dislikeButton !== null) {
        dislikeButton.addEventListener('click', dislikeActivity);
    }
    
    async function likeActivity(){
        let activityId = document.getElementById('Id').value;
        let token = document.getElementsByName("__RequestVerificationToken")[0].value;
        let formData = new FormData();
        formData.append('activityId', activityId);
        const fetchOptions = {
            method: 'POST',
            body: formData,
            headers: {
                'X-CSRF-TOKEN': token,
                'RequestVerificationToken': token
            }
        };
        const result = await request('/Ajax/LikeActivity', fetchOptions);
        if(result.isSuccess !== undefined && result.isSuccess === true) {
            let span = document.getElementById('likes-count');
            span.textContent = (Number(span.textContent) + 1) + '';

            likeButton.removeEventListener('click', likeActivity);
            likeButton.remove();
        }
    }

    async function dislikeActivity(){
        let activityId = document.getElementById('Id').value;
        let token = document.getElementsByName("__RequestVerificationToken")[0].value;
        let formData = new FormData();
        formData.append('activityId', activityId);
        const fetchOptions = {
            method: 'POST',
            body: formData,
            headers: {
                'X-CSRF-TOKEN': token,
                'RequestVerificationToken': token
            }
        };
        const result = await request('/Ajax/DislikeActivity', fetchOptions);
        if(result.isSuccess !== undefined && result.isSuccess === true) {
            let span = document.getElementById('likes-count');
            span.textContent = (Number(span.textContent) - 1) + '';

            dislikeButton.removeEventListener('click', dislikeActivity);
            dislikeButton.remove();
        }
    }

    async function request(url, options) {

        try {
            const response = await fetch(url, options);

            if (response.ok === false) {
                const error = await response.json();
                console.log(error);
            }

            if (response.status === 200) {
                return response.json();
            } else {
                return response.json();
            }

        } catch (err) {
            console.log(err.message);
            throw err;
        }
    }

    function createOptions(method = 'get', data) {
        const options = {
            method,
            headers: {}
        };

        if (data) {
            options.headers['Content-Type'] = 'application/json';
            options.body = data;
        }

        options.headers['RequestVerificationToken'] = 
            document.getElementsByName("__RequestVerificationToken")[0].value;
        return options;
    }
};



