class API {
    constructor() {
        this.API_URL = 'https://localhost:44389/api';
    }
    getWords() {
        return fetch(`${this.API_URL}/words`)
            .then(res => res.json());
    }
    getWord(id) {
        return fetch(`${this.API_URL}/words/${id}`)
            .then(res => res.json());
    }
  }