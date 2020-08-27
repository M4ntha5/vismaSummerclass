class API {
    constructor() {
        this.API_URL = 'https://localhost:44389/api';
    }
    async getWords() {
        const res = await fetch(`${this.API_URL}/words`);
        return await res.json();
    }
    async getWord(id) {
        const res = await fetch(`${this.API_URL}/words/${id}`);
        return await res.json();
    }
  }