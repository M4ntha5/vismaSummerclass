class Api {
    constructor() {
        this.API_URL = 'https://localhost:44389/api';
    }

    async fetchAnagrams(word) {
        const res = await fetch(`${this.API_URL}/anagrams/${word}`)
        console.log("fetchAnagrams-api", res);
        return await res.json();
    }

    async fetchWordsList() {
        const res = await fetch(`${this.API_URL}/words`);
        console.log("fetchWordsList-api", res);
        return await res.json();
    }

    async getWordById(id) {
        const res = await fetch(`${this.API_URL}/words/${id}`)
        console.log("getWordById-api", res);
        return await res.json();
    }

    async deleteWord(id) {
        let vm = this;
        await fetch(`${this.API_URL}/words/${id}/delete`, {
            method: 'DELETE'
        })
        .then(function (data){
            if(data.status == 200)
                vm.fetchWordsList();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
    }

    async insertWord(word, category) {
        const res = await fetch(`${this.API_URL}/words/insert?word=${word}&category=${category}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json'},
        })
        console.log("insert-api", res);
        return res;
    }

    async updateWord(id, word, category) {
        const res = await fetch(`${this.API_URL}/words/${id}/update?word=${word}&category=${category}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json'},
        })
        console.log("update-api", res);
        return res;
    } 
}