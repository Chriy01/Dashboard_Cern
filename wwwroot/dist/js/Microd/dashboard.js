var Table_Comunita;
var myData;
var comunita_id;
$(document).ready(function () {
    Table_Comunita = $('#grd_Comunita').DataTable({
        "language":
        {
            "url": url,
            "decimal": ',',
            "thousands": '.',

        },
        buttons: [
            {
                extend: 'excelHtml5',
                className: 'btn btn-info',
                title: 'Consumer',
                exportOptions: {
                    format: {
                        body: function (data, row, column, node) {
                            data = $('<p>' + data + '</p>').text().replace(' warning', '');
                            return $.isNumeric(data.replace(',', '.').replace('€', '')) ? data.replace(',', '.').replace('€', '') : data;
                        }
                    }


                }


            }
        ],
        "paging": true,
        dom: 'Biflrtp',
        "lengthMenu": [[-1, 10, 25, 50], ['Tutti', 10, 25, 50]],
        "columns": [
            { data: 'NOME' },
            { data: 'ANNO' },
            { data: 'ZONA_DI_MERCATO' },
            { data: 'TIPOLOGIA' },
            { data: 'ZONA_GEOGRAFICA' },
            { data: 'BTN' }

        ]
    });


});
$(document).ready(function () {
    // Effettua una chiamata AJAX al controller
    _GetTableComunita();
});

function _GetTableComunita() {
    $.ajax({
        url: 'home/GetTable',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            // Itera sui dati e chiama la funzione JavaScript per ogni elemento
            response.forEach(function (item) {
                console.log(item);
                AddComunita(item.nome, item.anno, item.zdM, item.tipologia, item.zg, item.btn);
            });
            Table_Comunita.draw(true);

            $(".btncancel").click(function () {
                Swal.fire({
                    title: "Sei sicuro di voler cancellare?",
                    text: "Tutti i dati relativi alla comunità non saranno più recuperabili.",
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#1aa33f',
                    cancelButtonColor: '#d33',
                    confirmButtonText: "Conferma",
                    cancelButtonText: "Annulla"
                }).then((result) => {
                    if (result.value) {
                        _DeleteComunita();
                        Table_Comunita.draw(true);
                        Swal.fire(
                            "Cancellato",
                            '',
                            'success'
                        )
                    }
                })
            });
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}
function _NuovaComunita() {
    $.ajax({
        url: 'home/nuovaComunita',
        type: 'GET',
       
        success: function (item) {
            window.location.href = '/DatiGenerali';
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}
function _DeleteComunita() {
    console.log(comunita_id);
    var formData = {
        id: comunita_id
    };
    $.ajax({
        url: 'home/DeleteComunita',
        type: 'GET',
        data: formData,
        success: function (item) {
            Table_Comunita.clear().draw(true)
            _GetTableComunita();
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}


function AddComunita(Nome, Anno, ZdM, Tipologia,ZG, Btn) {

    console.log(Nome);

    var apo = String.fromCharCode(39);
    myData = [
        {
            "NOME": Nome,
            "ANNO": Anno,
            "ZONA_DI_MERCATO": ZdM,
            "TIPOLOGIA": Tipologia,
            "ZONA_GEOGRAFICA": ZG,
            "BTN": Btn
        }

    ];





    Table_Comunita.rows.add(myData);

}
function _SetId(id) {
    var formData = {
        id: id
    };
    comunita_id = id;
    $.ajax({
        url: 'datiutenze/SetIdComunita',
        type: 'GET',
        data: formData,
        success: function (item) {

        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}
function editComunita(Id) {

    var formData = {
        id: Id
    };
    $.ajax({
        url: 'home/EditComunita',
        type: 'GET',
        data: formData,
        success: function (data) {
            // Itera sui dati e chiama la funzione JavaScript per ogni elemento
            document.location.href = '/DatiGenerali?#';
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}

function Modi(Id) {

    var formData = {
        id: Id
    };
    $.ajax({
        url: 'home/ModificaComunita',
        type: 'GET',
        data: formData,
        success: function (data) {
            // Itera sui dati e chiama la funzione JavaScript per ogni elemento
            document.location.href = '/DatiUtenze?#';
        },
        error: function (xhr, status, error) {
            console.log(error);
            console.error("Errore durante la chiamata AJAX: " + error);
        }
    });
}
