// =========================================
// DOBLE CLICK PARA EDITAR
// =========================================
$(document).on("dblclick", ".celda-editable", function () {

    let div = $(this);

    if (div.find("input").length > 0)
        return;

    let valor = div.text().trim();

    let input = $(`
                    <input
                        type="number"
                        class="form-control input-edicion"
                        value="${valor}"
                    />
                `);

    div.html(input);

    input.focus();

    input.select();
});

// =========================================
// GUARDAR EDICIÓN
// =========================================
$(document).on("blur", ".input-edicion", function () {

    let input = $(this);

    let nuevoValor =
        parseFloat(input.val()) || 0;

    let div = input.parent();

    let valorOriginal =
        parseFloat(div.data("original")) || 0;

    let pesoUnitario =
        parseFloat(div.data("peso")) || 0;

    if (valorOriginal === nuevoValor) {
        div.html(valorOriginal);

        return;
    }

    // volver a texto
    div.html(nuevoValor);

    //actualizar valor original
    div.data("original", nuevoValor);

    //para recalcular Footer
    actualizarTotales();

    // actualizar peso
    let tdCantidad = div.closest("td");

    let tdPeso = tdCantidad.next();

    tdPeso.find(".peso-semana")
        .html((nuevoValor * pesoUnitario).toFixed(2));

    let semanaStr = div.data("semana");

    let partes = semanaStr.split("-W");

    let item = {

        codDoc: div.data("coddoc"),

        numDoc: div.data("numdoc"),

        numIte: div.data("numite"),

        anio: parseInt(partes[0]),

        semana: parseInt(partes[1]),

        cantidad: nuevoValor
    };

    //Evitar duplicados
    let index = cambios.findIndex(c =>
        c.codDoc === item.codDoc &&
        c.numDoc === item.numDoc &&
        c.numIte === item.numIte &&
        c.anio === item.anio &&
        c.semana === item.semana
    );

    if (index >= 0) {
        cambios[index] = item;
    }
    else {
        cambios.push(item);
    }

    // cambios.push(item);

    div.css("background-color", "#fff3cd");
    //Borrar despues
    console.log(cambios);
});