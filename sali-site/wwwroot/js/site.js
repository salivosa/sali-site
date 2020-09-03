// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function callCtrler() {
    var req = new XMLHttpRequest();
    req.open('GET', '/Home/RefreshTwitterData', true);
    req.setRequestHeader('Content-Type', 'application/json');

    req.onload = function () {
        if (req.status >= 200 && req.status < 400)
            location.reload();
        else
            console.log('Ocurrió un error actualizando!');
    }

    req.send();
}

//Update data variable
function RefreshData() {
    callCtrler();
}

var element = document.getElementById('refresh_data');
element.addEventListener('click', RefreshData);