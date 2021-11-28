function Delete(detailId) {
    var controllername = $("#GetControlName").val();
    postdata = {
        'detailId': detailId 
    };

    $.ajax({
        contentType: 'application/x-www-form-urlencoded',
        dataType: 'json',
        type: "Post",
        url: (controllername == "" ? "../Home/":"Home/") +"RemoveItemInMiniOrder/" + detailId,
        data: postdata,
        success: function (data) {
            if (data == true) {
                location.reload();
            }
            else if (data == false) {
                alert('bargasht');
                $("#innerview").load("Home/RemoveItemInMiniOrder/" + detailId);
            }
        }
    });
}