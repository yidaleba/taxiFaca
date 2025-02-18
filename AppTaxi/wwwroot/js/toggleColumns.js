$(document).ready(function () {
    $(".column-toggle").change(function () {
        var columnClass = ".col-" + $(this).val();
        if ($(this).is(":checked")) {
            $(columnClass).show();  // Muestra la columna
        } else {
            $(columnClass).hide();  // Oculta la columna
        }
    });

    // Ocultar columnas al cargar la página si los checkboxes están desmarcados
    $(".column-toggle").each(function () {
        var columnClass = ".col-" + $(this).val();
        if (!$(this).is(":checked")) {
            $(columnClass).hide();
        }
    });
});
