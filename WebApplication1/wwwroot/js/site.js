$('#mainCheckbox').click(function () {
    if ($(this).is(':checked')) {
        $('input:checkbox').prop('checked', true);
    } else {
        $('input:checkbox').prop('checked', false);
    }
});

$('input:checkbox').click(function () {
    if ($(this).is(':checked')) { }
    else { $('#mainCheckbox').prop('checked', false); }
});

var id = [];
function CheckedID()
{
    var object = $('td input[type ="checkbox"]:checked');

    $.each(object, function (index, value) {
        console.log('Index: ' + index + '; Value ' + value.id);
        id[index] = value.id;
    });
    return id;
}

console.log(CheckedID());