// =========================================
// ACTUALIZAR TOTALES
// =========================================
function actualizarTotales() {

    let api = tabla;

    let footer = $(api.table().footer());

    let ths = footer.find("th");

    let indexFooter = 1;

    semanasGlobal.forEach(sem => {

        let totalCantidad = 0;

        let totalPeso = 0;

        // SOLO FILAS VISIBLES/FILTRADAS
        api.rows({ search: 'applied' }).every(function () {

            let rowNode = this.node();

            let celda =
                $(rowNode).find(`.celda-editable[data-semana='${sem}']`);

            if (celda.length > 0) {

                let cantidad =
                    parseFloat(celda.text().trim()) || 0;

                let pesoUnitario =
                    parseFloat(celda.data("peso")) || 0;

                totalCantidad += cantidad;

                totalPeso += cantidad * pesoUnitario;
            }
        });

        $(ths[indexFooter])
            .text(totalCantidad.toFixed(2));

        $(ths[indexFooter + 1])
            .text(totalPeso.toFixed(2));

        indexFooter += 2;
    });
}