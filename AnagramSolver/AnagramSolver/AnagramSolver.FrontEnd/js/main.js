const controller = new Controller();
const api = new Api();
var page = 1;
 
// Example starter JavaScript for disabling form submissions if there are invalid fields
(function() {
    'use strict';
    window.addEventListener('load', function() {
        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var forms = document.getElementsByClassName('needs-validation');
        // Loop over them and prevent submission
        var validation = Array.prototype.filter.call(forms, function(form) {
            form.addEventListener('submit', function(event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });
    }, false);
})();


function showSearchPage() {
    //hide other pages
    document.getElementById("words-page").style.display = "none";
    document.getElementById("about-page").style.display = "none";
    //show search page
    document.getElementById("search-page").style.display = "block";

    document.getElementById("anagrams-list").innerHTML = '';
    document.getElementById("anagrams-heading").innerHTML = '';
    document.getElementById("search-input").value = '';  
}

function searchAnagrams() {
    var phrase = document.getElementById("search-input").value;
    controller.getAnagrams(phrase);
   // document.getElementById('search-input').value = '';  
}

function showWordsPage() {
    //hide other pages
    document.getElementById("search-page").style.display = "none";
    document.getElementById("about-page").style.display = "none";
    
    const urlParams = new URLSearchParams(window.location.search);
    const currentPage = urlParams.get('pagenumber');

    console.log("page", page);
    if(page < 2)
        document.getElementById("prevBtn").disabled = true;
    else if(page > 100)
        document.getElementById("nextBtn").disabled = true;

    //show words page
    document.getElementById("words-page").style.display = "block";  
    //fetch words
    controller.fetchWords(currentPage ?? 1);
}

function showAboutPage() {
    //hide other pages
    document.getElementById("search-page").style.display = "none";
    document.getElementById("words-page").style.display = "none";
    //show about page
    var elem = document.getElementById("about-page")
    elem.style.display = "block";
    elem.innerHTML= '<div> <h1>Abuot page</h1> </div>';
}

function addNewWord(){
    var idValue = document.getElementById("input-id").value;

    if(idValue == ''){ //insert
        var cat = document.getElementById("category").value;
        var word = document.getElementById("word").value;
        controller.insertWord(word, cat);      
    }
    else if(idValue != ''){ //update
        var cat = document.getElementById("category").value;
        var word = document.getElementById("word").value;
        controller.updateWord(idValue, word, cat);
    }
    controller.fetchWords();
}

async function updateWordAction(id){
    var data = await controller.getWordById(id);

    $('#exampleModal').modal('show');
    document.getElementById('category').value = data.category;
    document.getElementById('word').value = data.word;
    document.getElementById('input-id').value = data.id;
    controller.fetchWords();
}

function deleteWordAction(id){
    alert("You sure you want to delete this word?")
    console.log("delete veikia", id);
    controller.deleteWord(id);
    controller.fetchWords();
}

function openModal(){
    $('#exampleModal').modal('show');
    document.getElementById('category').value = '';
    document.getElementById('word').value = '';
    document.getElementById('input-id').value = '';
}

function prevPage() {  
    if(page > 1) {
        document.getElementById("prevBtn").disabled = false;
        controller.fetchWords(page - 1);
        page--;
    }
}

function nextPage(){
    if(page < 100) {  
        document.getElementById("prevBtn").disabled = false;
        document.getElementById("nextBtn").disabled = false;
        controller.fetchWords(page + 1);
        page++;
        if(page == 100)
            document.getElementById("nextBtn").disabled = true;
    }
}