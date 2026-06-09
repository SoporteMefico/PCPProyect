// =========================================
// CONSTRUIR TABLA
// =========================================
function construirTabla() {

    // destruir tabla anterior
    if ($.fn.DataTable.isDataTable('#tablaProyeccion')) {

        $('#tablaProyeccion').DataTable().destroy();
        // $('#tablaProyeccion').empty();

        $('#tablaProyeccion thead').empty();

        $('#tablaProyeccion tbody').empty();

        $('#tablaProyeccion tfoot').html('<tr></tr>');
    }

    let desde = $("#fechaDesde").val();

    let hasta = $("#fechaHasta").val();

    semanasGlobal = generarSemanas(desde, hasta);

    // =========================================
    // COLUMNAS FIJAS
    // =========================================
    let columnas = [
        { title: "Pedido", data: "pedido", width: "120px" },

        { title: "Cliente", data: "cliente", width: "220px" },

        { title: "Documento", data: "numDoc", width: "120px" },

        { title: "Item", data: "numIte", width: "90px" },

        { title: "Artículo", data: "codArt", width: "100px" },

        { title: "Descripción", data: "desArt", width: "100px" },

        { title: "Cantidad", data: "cantidad", width: "90px" },
        { title: "Peso", data: "pesoUnitario", width: "90px" }
    ];

    // =========================================
    // COLUMNAS DINÁMICAS
    // =========================================
    semanasGlobal.forEach(sem => {

        // cantidad
        columnas.push({

            title: sem + " CANT",

            data: null,

            render: function (data, type, row) {

                let info = row.semanas[sem];

                let cantidad = info ? info.cantidad : 0;

                return `
                                <div
                                    class="celda-editable"
                                    data-semana="${sem}"
                                    data-coddoc="${row.codDoc}"
                                    data-numdoc="${row.numDoc}"
                                    data-numite="${row.numIte}"
                                    data-peso="${row.pesoUnitario}"
                                    data-original="${cantidad}"
                                >
                                    ${cantidad}
                                </div>
                            `;
            }
        });

        // peso
        columnas.push({

            title: sem + " PESO",

            data: null,

            render: function (data, type, row) {

                let info = row.semanas[sem];

                let peso = info ? info.peso : 0;

                return `
                                <div class="text-end fw-bold peso-semana">
                                    ${parseFloat(peso).toFixed(2)}
                                </div>
                            `;
            }
        });
    });

    // =========================================
    // DATATABLE
    // =========================================
    tabla = $("#tablaProyeccion").DataTable({

        processing: true,

        serverSide: true,

        searching: true,

        paging: true,

        pageLength: 10,
        autoWidth: false,

        lengthMenu: [10, 25, 50, 100],

        scrollX: true,
       /* scrollY: "500px",*/
        fixedHeader: true,
        //Para fijar columnas
        scrollCollapse: true,
         fixedColumns: {
             left:8
         },

        destroy: true,

        deferRender: true,

        ajax: function (dtParams, callback, settings) {

            let page =
                (dtParams.start / dtParams.length) + 1;

            let filtro = {

                fechaDesde: $("#fechaDesde").val(),

                fechaHasta: $("#fechaHasta").val(),

                saldoPP01Minimo:
                    parseFloat($("#saldoPP01Minimo").val()) === "" ? null : parseFloat($("#saldoPP01Minimo").val()),

                page: page,

                pageSize: dtParams.length,

                buscar: dtParams.search.value
            };

            $.ajax({

                url: '/Proyeccion/ObtenerGrid',

                method: 'POST',

                contentType: 'application/json',

                data: JSON.stringify(filtro),

                success: function (resp) {

                    callback({

                        draw: dtParams.draw,

                        recordsTotal: resp.total,

                        recordsFiltered: resp.total,

                        data: resp.data
                    });
                },

                error: function (xhr) {

                    console.log(xhr);

                    alert("Error cargando datos");
                }
            });
        },

        columns: columnas,

        footerCallback: function (row, data) {

            let api = this.api();

            let footer = $(api.table().footer());

            footer.empty();

            let tr = $("<tr></tr>");

            tr.append("<th colspan='8' class='text-end'>TOTALES:</th>");

            semanasGlobal.forEach(sem => {

                let totalCantidad = 0;

                let totalPeso = 0;

                data.forEach(row => {

                    let info = row.semanas[sem];

                    if (info) {

                        totalCantidad += parseFloat(info.cantidad || 0);

                        totalPeso += parseFloat(info.peso || 0);
                    }
                });

                tr.append(`
                                <th class="text-end">
                                    ${totalCantidad.toFixed(2)}
                                </th>
                            `);

                tr.append(`
                                <th class="text-end">
                                    ${totalPeso.toFixed(2)}
                                </th>
                            `);
            });

            footer.append(tr);
            //Para que actualice en cada busqueda o guardado?
            actualizarTotales();
        }
    });

    tabla.on('draw', function () {

        actualizarTotales();
    });
}