const controller = new Controller();
const api = new Api();
var page = 1;


function showSearchPage() {
    //hide other pages
    document.getElementById("words-page").style.display = "none";
    document.getElementById("about-page").style.display = "none";
    //show search page
     
    document.getElementById("search-page").style.display = "block";
    
    document.getElementById("anagrams-list").innerHTML = '';
    document.getElementById("anagrams-heading").innerHTML = '';
    document.getElementById("search-input").innerHTML = '';  
    document.getElementById("invalid-feedback").innerHTML = '';
    document.getElementById("valid-feedback").innerHTML = '';
}

function searchAnagrams() {
    var phrase = document.getElementById("search-input").value;
    if(!validateSearchForm(phrase))
        return;

    controller.getAnagrams(phrase);
    
    document.getElementById('search-input').innerHTML = '';  
}

function showWordsPage() {
    //hide other pages
    document.getElementById("search-page").style.display = "none";
    document.getElementById("about-page").style.display = "none";
    
    /*const urlParams = new URLSearchParams(window.location.search);
    const currentPage = urlParams.get('pagenumber');*/

    if(page < 2)
        document.getElementById("prevBtn").disabled = true;
    else if(page > 100)
        document.getElementById("nextBtn").disabled = true;

    //show words page
    document.getElementById("words-page").style.display = "block";  
    //fetch words
    controller.fetchWords(page ?? 1);
    
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
        if(!validateModalCategory(cat) || !validateModalWord(word))
            return;
        controller.insertWord(word, cat);      
    }
    else if(idValue != ''){ //update
        var cat = document.getElementById("category").value;
        var word = document.getElementById("word").value;
        if(!validateModalCategory(cat) || !validateModalWord(word))
            return;
        controller.updateWord(idValue, word, cat);
    }
    controller.fetchWords(page ?? 1);
}

function validateModalWord(word){
    if(word.length > 10 || word.length < 1 || !word.match("[a-zA-Z]+")){
        document.getElementById("modal-word-valid-feedback").innerHTML = '';
        document.getElementById("modal-word-invalid-feedback").innerHTML = "<p>Word required, it cannot be longer than 10 characters and could only have letters</p>";
        return false;
    }
    else{          
        document.getElementById("modal-word-invalid-feedback").innerHTML = '';
        document.getElementById("modal-word-valid-feedback").innerHTML = 'Looks good!';
        return true;
    }
}

function validateModalCategory(cat){
    //validation
    if(cat.length > 20 || cat.length < 1 || !cat.match("[a-zA-Z]+")){
        document.getElementById("modal-cat-valid-feedback").innerHTML = '';
        document.getElementById("modal-cat-invalid-feedback").innerHTML = "<p>Category required, it cannot be longer than 20 characters and could only  letters</p>";
        return false;
    }
    else {          
        document.getElementById("modal-cat-invalid-feedback").innerHTML = '';
        document.getElementById("modal-cat-valid-feedback").innerHTML = 'Looks good!';
        return true;
    } 
}

function validateSearchForm(phrase){
    //validation
    if(phrase.length > 50 || phrase.length < 1 ){
        document.getElementById("valid-feedback").innerHTML = '';
        document.getElementById("invalid-feedback").innerHTML = "<p>Phrase required and cannot be longer than 50 characters</p>";
        return false;
    }
    else {
        document.getElementById("invalid-feedback").innerHTML = '';
        document.getElementById("valid-feedback").innerHTML = "<p>Looks good!</p>";
        return true;
    }
}

async function updateWordAction(id){
    var data = await controller.getWordById(id);

    $('#exampleModal').modal('show');
    document.getElementById('category').value = data.category;
    document.getElementById('word').value = data.word;
    document.getElementById('input-id').value = data.id;
    controller.fetchWords(page ?? 1);
}

function deleteWordAction(id){
    alert("You sure you want to delete this word?")
    controller.deleteWord(id);
    controller.fetchWords(page ?? 1);  
}

function openModal(){
    document.getElementById("modal-cat-invalid-feedback").innerHTML = '';
    document.getElementById("modal-cat-valid-feedback").innerHTML = '';
    document.getElementById("modal-word-invalid-feedback").innerHTML = '';
    document.getElementById("modal-word-valid-feedback").innerHTML = '';

    document.getElementById("valid-feedback").innerHTML = '';
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