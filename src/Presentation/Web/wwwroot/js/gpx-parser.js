window.onload = function () {
    let load = $("#load-gpx-file");
    if (load !== undefined && load !== null) {
        load.on("click", function () {
            $(".spinner-modal").show();
            var fd = new FormData();
            fd.append('files', $('#GpxFile')[0].files[0]);

            $.ajax({
                "async": true,
                "crossDomain": true,
                "url": "/Ajax/ProcessGps",
                "method": "POST",
                "headers": {
                    RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                },
                "processData": false,
                "contentType": false,
                "mimeType": "multipart/form-data",
                "data": fd
            }).done(function (gpx) {
                const data = JSON.parse(gpx);
                console.log("Success");
                console.log(data);
                $("#PositiveElevation").val(parseInt(data.positiveElevation));
                $("#NegativeElevation").val(parseInt(data.negativeElevation));
                $("#Distance").val(data.distance.toFixed(3));
                $("#Duration").val(data.duration);
                // $("#StartDateTime").val(data.startDateTime.split('T')[0]);
                var startDate = new Date(data.startDateTime);
                var formattedDateTime = startDate.toISOString().slice(0, 16);
                $("#StartDateTime").val(formattedDateTime);
                $("#GpxId").val(data.gpxId);

            }).fail(function (data) {
                console.log("")
                console.log(data);
                alert("The is an error with processing of GPX file!");
            }).always(function () {
                $(".spinner-modal").hide();
            });
        });
    }

    let uploadPictures = $('#upload-pictures');
    if (uploadPictures !== undefined && uploadPictures !== null) {
        // uploadPictures.prop( "disabled", true );
        uploadPictures.on('click', function (){
            $(".spinner-modal").show();
            console.log('Upload pictures button is clicked!');
            
            let imageInput = document.getElementById('Pictures');
            console.log(imageInput);
            let fd = new FormData();
            let files = imageInput.files;
            console.log(files);
            for (let i = 0; i !== files.length ; i++) {
                fd.append("files", files[i]);
            }
            
            $.ajax({
                "async": true,
                "crossDomain": true,
                "url": "/Ajax/ProcessImages",
                "method": "POST",
                "headers": {
                    RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                },
                "processData": false,
                "contentType": false,
                "mimeType": "multipart/form-data",
                "data": fd
            }).done(function (images) {
                const data = JSON.parse(images);
                let files = [];
                for (let i = 0; i < data.length; i++) {
                    console.log(data[i]);
                    files.push(data[i]);
                }
                
                $("#PicturesList").val(files.join(';'));
                console.log("Success");
                console.log(data);

            }).fail(function (data) {
                console.log("Failure");
                console.log(data);
                alert("The is an error with processing of images file!");
            }).always(function () {
                $(".spinner-modal").hide();
            });
        })
    }
}