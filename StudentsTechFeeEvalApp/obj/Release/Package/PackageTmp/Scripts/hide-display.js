$(document).ready(function () {

    $('input[type="radio"]').click(function () {

        if ($(this).attr('id') == 'approved') {
            $('#comment').show();
            $('#rank').show();
        } else if ($(this).attr('id') == 'rejected') {
            $('#comment').hide();
            $('#rank').hide();
        }
    });
   
});