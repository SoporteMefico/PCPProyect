// proyeccion.js

let tabla;
let semanasGlobal = [];
let cambios = [];

$(document).ready(function () {

    $("#btnBuscar").click(function () {
        construirTabla();
    });

    setInterval(function () {
        guardarCambiosAutomatico();
    }, 15000);

    window.addEventListener("beforeunload",
        function (e) {

            if (cambios.length > 0) {

                e.preventDefault();
                e.returnValue = '';
            }
        });

    // =========================================
    // GUARDAR CAMBIOS - actualmente no se utiliza porque se guarda automaticamente cada 15 segundos.
    // =========================================
    $("#btnGuardar").click(function () {

        if (cambios.length === 0) {

            alert("No hay cambios");

            return;
        }

        guardarCambiosAutomatico();
        alert("Guardado correctamente");
    });
});