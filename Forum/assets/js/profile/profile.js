
window.onload = () => {
    document.querySelector('.profile-right').classList.add("col-12");
    this.nameOld = document.querySelector('#name_profile').value;
}
var nameValueOld = this.nameOld;
var cancelEditProfile = () => {
    this.nameNew = document.querySelector('#name_profile').value;
    if (this.nameOld != this.nameNew) {
        $.confirm({
            title: 'Confirm!',
            content: 'You can wanna cancel!',
            buttons: {
                confirm: function () {
                    location.reload();
                },
                cancel: function () {

                }

            }
        });
    } else {
        location.reload();
    }
    
}

var show_edit_profile = () => {
    document.querySelector('.profile-left').style.display = 'block';
    document.querySelector('.profile-left').classList.add("col-12");
    document.querySelector('.profile-right').classList.remove("col-12");
    document.querySelector('.profile-right').style.display = 'none';
}
var mySubmitFunction = (event) => {
    var name_profile = document.querySelector('#name_profile').value
    var lenght_name_profile = name_profile.length;
    if (lenght_name_profile < 1) {
        event.preventDefault();
        document.querySelector('.form-message').innerHTML = '<p>Required !</p>'
        document.querySelector('.form-message').style.display = 'block';
    } else if (lenght_name_profile > 255) {
        event.preventDefault();
        document.querySelector('.form-message').innerHTML = '<p>Not more than 255 characters !</p>'
        document.querySelector('.form-message').style.display = 'block';
    }
    return true;
}






