
window.onload = () => {
    document.querySelector('.profile-right').classList.add("col-12");
}



var show_edit_profile = () => { 
    document.querySelector('.profile-left').style.display = 'block';
    document.querySelector('.profile-left').classList.add("col-12");
    document.querySelector('.profile-right').classList.remove("col-12");
    document.querySelector('.profile-right').style.display = 'none';
}

$("#cancel_edit_profile").click(function (event) {
    event.preventDefault()
    $.confirm({
        title: 'Confirm!',
        content: 'Do you want cancel ?',
        buttons: {
            confirm: function () {
                window.location.reload();
            },
            cancel: function () {
                
            }
            
        }
    });
});


var submit_form = () => {
    var imgNew = document.querySelector('#imgNew');
    var imgOld = document.querySelector('#imgOld');
    var valueImgNew = imgNew.value;
    if (valueImgNew != "") {
        imgNew.setAttribute('name', 'image');
    } else {
        imgOld.setAttribute('name', 'image');
    }
}



