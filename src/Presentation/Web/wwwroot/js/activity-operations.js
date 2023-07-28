window.onload = (event) => {
    console.log("page is fully loaded");
    
    const likeButton = document.getElementById('like-activity');
    if (likeButton !== undefined && likeButton !== null) {
        likeButton.addEventListener('click', function likeActivity(){
            console.log('like activity')
            let activityId = document.getElementById('Id').value;
            let formData = new FormData();
            formData.append('activityId', activityId);
            let verificationToken = $('input[name="__RequestVerificationToken"]').val();
            console.log(verificationToken);
        });
    }
}