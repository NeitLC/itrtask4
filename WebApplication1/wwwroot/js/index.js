document.getElementById("mainCheckbox").onclick = function checkAll(event) {
    let elements = document.getElementsByClassName("row-checkbox");
    for (var i = 0; i < elements.length; i++) {
        elements[i].checked = event.target.checked;
    }
};

function CheckID() {
    var object = $('td input[type ="checkbox"]:checked');

    $.each(object, function (index, value) {
        console.log('Index:' + index + '; Value' + value.id);
        id[index] = value.id;
    });
}

document.getElementById("buttonBlock").onclick = function block(event) {
    
}



