class Helpers {
    constructor(){

    }

    appendWordsData(data) {
        var table = document.getElementById("words-table");
        table.innerHTML = '';

        for (var i = 0; i < data.length; i++) {
            var row = table.insertRow(0);
            var cell1 = row.insertCell(0);
            var cell2 = row.insertCell(1);

            cell1.innerHTML = data[i].word;
            cell2.innerHTML = '<a class="btn btn-warning" href="#" onclick="updateWordAction('+ data[i].id + ')">Update</a> '
                + '<a class="btn btn-danger" href="#" onclick="deleteWordAction(' + data[i].id + ')">Delete</a>';
        }

        // Create an empty <thead> element and add it to the table:
        var header = table.createTHead();
        // Create an empty <tr> element and add it to the first position of <thead>:
        var headerRow = header.insertRow(0);    
        // Insert a new cell (<td>) at the first position of the "new" <tr> element:
        var headerCell1 = headerRow.insertCell(0);
        var headerCell2 = headerRow.insertCell(1);
        // Add some bold text in the new cell:
        headerCell1.innerHTML = "<b>Words</b>"; 
        headerCell2.innerHTML = "<b>Actions</b>";
    }

    appendAnagramsData(data) {
        var table = document.getElementById("anagrams-list");
        table.innerHTML = "";
        for (var i = 0; i < data.length; i++) {
            var row = table.insertRow(0);
            var cell1 = row.insertCell(0);
            cell1.innerHTML = data[i];
        }
        //heading 
        var heading = document.getElementById("anagrams-heading");
        heading.innerHTML = "Anagrams found:";
    }

    hideModal(){
        $('#exampleModal').modal('hide');
        document.getElementById('category').value = '';
        document.getElementById('word').value = '';
        document.getElementById('input-id').value = '';
    }

    updateWordAction(data){
        $('#exampleModal').modal('hide');
        document.getElementById('category').value = data.category;
        document.getElementById('word').value = data.word;
        document.getElementById('input-id').value = data.id;
    }
}