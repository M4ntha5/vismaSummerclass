class Controller {

    constructor() {
        this.api = new Api();
        this.helpers = new Helpers();
    }

    fetchWords(pageNumber){
        this.api.fetchWordsList(pageNumber)
            .then(result => {
                console.log("fetchWords-controller", result);
                this.helpers.appendWordsData(result);         
            });
    }

    getAnagrams(word) {
        this.api.fetchAnagrams(word)
            .then(result => {
                console.log("getAnagrams-controller", result);
                this.helpers.appendAnagramsData(result);         
            });
    }

    getWordById(id) {
        return this.api.getWordById(id);
           /* .then(result => {
                console.log("getWordById-controller", result);
                return await result;
                //updateWordAction(result);         
            });*/
    }

    deleteWord(id) {
        this.api.deleteWord(id)
            .then(result => {
                console.log("deleteWord-controller", result);  
            });
    }

    insertWord(word, category) {
        this.api.insertWord(word, category)
            .then(data => {
                console.log("insertWord-controller", data);  
                //if success close modal and clear modal fields
                if(data.status == '200')
                    this.helpers.hideModal();
            });
    }

    updateWord(id, word, category) {
        this.api.updateWord(id, word, category)
            .then(data => {
                console.log("insertWord-controller", data);  
                //if success close modal and clear modal fields
                if(data.status == '200')
                    this.helpers.hideModal();
            });
    }
  
}