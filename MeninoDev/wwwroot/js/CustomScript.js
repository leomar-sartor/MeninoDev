function confirmaDelete(uniqueId, isDeleteClicked) {

    console.log("confirmaDelete", isDeleteClicked);

    var deleteSpan = 'deletaSpan_' + uniqueId;
    console.log(deleteSpan);
    var confirmaDeleteSpan = 'confirmaDeletaSpan_' + uniqueId;
    console.log(confirmaDeleteSpan);

    if (isDeleteClicked) {

        console.log("clicou");

        $('#' + deleteSpan).hide();
        $('#' + confirmaDeleteSpan).show();
    } else {

        console.log("não clicou");

        $('#' + deleteSpan).show();
        $('#' + confirmaDeleteSpan).hide();
    }
}