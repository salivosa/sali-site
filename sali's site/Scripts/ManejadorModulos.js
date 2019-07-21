$("#formulario_login").click(function () {
    login();
});

$('#input_username').on('keypress', function (e) {
    if (e.which === 13) {
        login();
    }
});

$('#input_password').on('keypress', function (e) {
    if (e.which === 13) {
        login();
    }
});

$("#cerrar_sesion").click(function () {
    log_off();
});

function log_off() {
    $.ajax({
        type: 'GET',
        url: '/inicio/log_off',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            window.location.href = result;
        },
        error: function () {
            Lobibox.notify('error', {
                size: 'mini',
                sound: false,
                rounded: true,
                delayIndicator: false,
                msg: "¡Ocurrió un error!"
            });
        }
    });
}

function login() {
    
    var username = $('#input_username').val();
    var password = $('#input_password').val();

    $.ajax({
        type: 'GET',
        url: '/inicio/login',
        data: { user: username, password: password },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            if (result) 
                cargar_modulos(username);
            
            else {
                Lobibox.notify('error', {
                    size: 'mini',
                    sound: false,
                    rounded: true,
                    delayIndicator: false,
                    msg: "Usuario o contraseña incorrecta"
                });
            }

        }
        ,
        error: function () {
            Lobibox.notify('error', {
                size: 'mini',
                sound: false,
                rounded: true,
                delayIndicator: false,
                msg: "¡Ocurrió un error!"
            });
        }
    });
}


function cargar_modulos() {
    
    $.ajax({
        type: 'GET',
        url: '/inicio/modulos_disponibles',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            result.datos_modulo.forEach(function (valor) {
                $('#listado_modulos').append("<div><a class='modulos' id='" + valor.module_name + "'><i class='fa " + valor.module_icon + "'></i></a></div>");

                tippy('#' + valor.module_name, {
                    content: valor.module_description
                });

                $('#' + valor.module_name).click(function () {
                    window.location.href = valor.module_route;
                });

            });

            $('#form_login').fadeOut("fast");

            var mensaje = 'Bienvenido, ' + result.fullname;
            $('#usuario').text(mensaje);

            $('#user_details .modal-body').append("<img class='avatar' src='" + result.user_icon + "'>");
            $('#user_details .modal-body').append("<div id='contenedor_datos'></div>")
            $('#contenedor_datos').append("<div><b>Usuario</b>: " + result.usuario + "</div>");
            $('#contenedor_datos').append("<div><b>Nombre Completo</b>: " + result.fullname + "</div>");
            $('#contenedor_datos').append("<div><b>Fecha de Registro</b>: " + result.registration_date + "</div>");
            
            $('#usuario').fadeIn("fast");

            $('#listado_modulos').fadeIn("fast");
        }
        ,
        error: function () {
            Lobibox.notify('error', {
                size: 'mini',
                sound: false,
                rounded: true,
                delayIndicator: false,
                msg: "¡Ocurrió un error!"
            });
        }
    });
}

function verificar_logueo() {

    if (is_logged)
        cargar_modulos();

    else {
        $('#usuario').hide();
        $('#form_login').fadeIn("fast");
    }

}

verificar_logueo();