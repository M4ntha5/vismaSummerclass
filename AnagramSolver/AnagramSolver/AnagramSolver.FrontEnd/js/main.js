const app = new App('#app');
const api = new API();


const wordsTemplate = (word) => `
    <table class="table pt-5">
        <tbody>
            <tr class="row pl-3">
                <th class="col-sm-6">${word.word}</th>
                <td class="col-sm-6">
                    <a href="#/words/${word.word}">Details</a> |
                    <a href="${word.id}">Update</a> |
                    <a href="${word.id}">Delete</a>
                </td>          
            </tr>
        </tbody>
    </table>
`;

const wordDetailsTemplate = (anagrams) => `
<div class="pl-3">
    <hr/>
        <dl class="row">
            <dt class="col">
                ${anagrams.word}
            </dt>         
        </dl>
    <hr/>
</div>
`;


app.addComponent({
    name: 'words',
    model: {
        words: []
    },
    view(model) {
        const wordsHTML = model.words.reduce((html, word) => html + wordsTemplate(word), '')
        return wordsHTML;
    },
    controller(model) {
        api
            .getWords()
            .then(result => {
                model.words = result;
                console.log(result);
            });
    }
});

app.addComponent({
    name: 'anagrams',
    model: {
        anagrams: []
    },
    view(model) {
        const anagramsHTML = model.anagrams.reduce((html, anagram) => html + wordDetailsTemplate(anagram), '')
        return anagramsHTML;
    },
    controller(model) {
        api
            .getWord(router.params)
            .then(result => {
                model.anagrams = result;
                console.log(result);
            });
    }
});


const router = new Router(app);
router.addRoute('about', '#/about');
router.addRoute('search', '#/search');
router.addRoute('words', '#/words');
router.addRoute('anagrams', '#/words/{:id}');