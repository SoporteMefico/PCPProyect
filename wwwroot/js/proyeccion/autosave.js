//==========================
//Guardado automatico en intervalo de tiempo
//=========================
function guardarCambiosAutomatico() {

    if (cambios.length === 0)
        return;

    $.ajax({

        url: '/Proyeccion/GuardarLote',

        method: 'POST',

        contentType: 'application/json',

        data: JSON.stringify(cambios),

        success: function () {
            let cantidadGuardada = cambios.length;

            console.log(
                "AutoGuardado: "
                + new Date().toLocaleTimeString()
            );

            $("#lblAutoSave").text(
                "Último guardado: "
                + new Date().toLocaleTimeString()
            );

            cambios = [];

            $(".celda-editable")
                .css("background-color", "");

            mostrarToast(cantidadGuardada + " Proyección guardada automáticamente");
        },

        error: function () {

            console.log("Error AutoGuardado");
        }
    });
}