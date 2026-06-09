// =========================================
// ISO WEEK
// =========================================
function getISOWeek(date) {

    let d = new Date(Date.UTC(
        date.getFullYear(),
        date.getMonth(),
        date.getDate()
    ));

    let dayNum = d.getUTCDay() || 7;

    d.setUTCDate(d.getUTCDate() + 4 - dayNum);

    let yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));

    return Math.ceil((((d - yearStart) / 86400000) + 1) / 7);
}


// =========================================
// GENERAR SEMANAS
// =========================================
function generarSemanas(desde, hasta) {

    let semanas = [];

    let fecha = new Date(desde);

    while (fecha <= new Date(hasta)) {

        let anio = fecha.getFullYear();

        let semana = getISOWeek(fecha);

        let key = `${anio}-W${semana}`;

        if (!semanas.includes(key)) {
            semanas.push(key);
        }

        fecha.setDate(fecha.getDate() + 7);
    }

    return semanas;
}