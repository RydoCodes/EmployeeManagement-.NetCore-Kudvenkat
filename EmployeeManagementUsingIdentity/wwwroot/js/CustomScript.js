function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show(); //Meanin Delete Button was clicked and we want the deletebutton yo hide and show confirm delete div
    } else {
        $('#' + deleteSpan).show(); // Meanin No wass clicked and we want the delete back again and hide confirm delete div.
        $('#' + confirmDeleteSpan).hide();
    }
}