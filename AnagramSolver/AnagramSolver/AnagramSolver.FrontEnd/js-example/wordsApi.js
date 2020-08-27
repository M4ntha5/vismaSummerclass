class WordsApi {
    constructor() {
        this.API_URL = 'https://localhost:44389/api';
    }

    getWords() {
        fetch('https://localhost:44389/api/words')
            .then(function (response) {
                return response.json();
            }) 
            .then(function (data) {
                console.log(data);
                return data;//appendData(data);
            })
            .catch(function (err) {
                console.log( err);
            }
        );
    }

    deleteWord(id) {
        fetch(`${this.API_URL}/words/${id}/delete`, {
            method: 'DELETE'
        })
        .catch((error) => {
            console.error('Error:', error);
        });
    }
    insertWord(word, category) {
        fetch(`${this.API_URL}/words/insert?word=${word}&category=${category}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json'},
        })
        .catch((error) => {
            console.error('Error:', error);
        });
    }

    updateWord(id, word, category) {
        fetch(`${this.API_URL}/words/${id}/update?word=${word}&category=${category}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json'},
        })
        .catch((error) => {
            console.error('Error:', error);
        });
    } 
}